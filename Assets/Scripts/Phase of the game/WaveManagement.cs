using System;
using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEngine;
using FluxFramework.Core;
using FluxFramework.Attributes;
using FluxFramework.Extensions;

[DisallowMultipleComponent]
public class WaveManagement : FluxMonoBehaviour
{
    [Header("Data to insert")]
    [SerializeField] private SOWaveManager sOWaveManagement;
    [SerializeField] private Transform playerTransform;

    [Header("Live Wave Data")]
    [SerializeField] private int currentWaveIndex = 0;
    [SerializeField] private List<GameObject> activeEnemies = new List<GameObject>();
    
    // --- Private State ---
    private int totalWaves;
    private int activeSpawningSequences = 0;
    
    // --- Control Flags & Subscriptions ---
    // Used to handle the subscription to the player's transform property to prevent memory leaks.
    private IDisposable _playerTransformSubscription;
    // A flag to ensure we only start the wave sequence once.
    private bool _wavesStarted = false;

    
    protected new void Start()
    {
        TryStartWaves(playerTransform);
    }

    /// <summary>
    /// This is the main entry point for starting the wave logic. It's called once the player's transform is available.
    /// </summary>
    /// <param name="newPlayerTransform">The player's transform, provided by the subscription.</param>
    private void TryStartWaves(Transform newPlayerTransform)
    {
        // Guard clauses: Do nothing if waves have already started or if the transform is somehow null.
        if (_wavesStarted || newPlayerTransform == null)
        {
            return;
        }

        // --- Dependencies are now met ---
        this.playerTransform = newPlayerTransform;

        // Now, we check the other dependency (the SOWaveManager asset).
        if (sOWaveManagement == null || sOWaveManagement.SOWaves.Count == 0)
        {
            Debug.LogError("SOWaveManager is not assigned or is empty! Waves cannot start.", this);
            this.enabled = false; // It's still correct to disable the component if setup fails.
            return;
        }

        // --- All checks passed, we can safely start the game loop ---
        _wavesStarted = true;
        totalWaves = sOWaveManagement.SOWaves.Count;
        _playerTransformSubscription?.Dispose();
        _playerTransformSubscription = null; // Clear the reference after disposing.
        Debug.Log("Player found and dependencies are set. Starting the first wave...");
        StartNextWave();
    }

    /// <summary>
    /// Determines which spawn strategy to use based on the monster's configuration.
    /// </summary>
    private ISpawnStrategy SetStrategy(MonsterEntry monsterEntry)
    {
        return monsterEntry.spawningMode switch
        {
            SpawningMode.Random => new RandomSpawnStrategy(playerTransform),
            SpawningMode.Line => new LineSpawnStrategy(playerTransform),
            _ => new RandomSpawnStrategy(playerTransform), // Default to random if undefined.
        };
    }

    /// <summary>
    /// Kicks off the next wave in the sequence.
    /// </summary>
    private void StartNextWave()
    {
        if (currentWaveIndex >= totalWaves)
        {
            Debug.Log("All waves completed! Game Over!");
            // TODO: Publish a game win event.
            return;
        }
        
        currentWaveIndex++;
        // this.PublishEvent(new WaveStartEvent(currentWaveIndex)); // Example of a useful event
        Debug.Log($"Starting Wave {currentWaveIndex}");

        SOWaves waveData = sOWaveManagement.SOWaves[currentWaveIndex - 1];
        foreach (MonsterEntry entry in waveData.Monsters)
        {
            if (entry.monsterData != null && entry.monsterData.MonsterPrefab != null)
            {
                // This StartCoroutine call is now safe because it only happens after successful initialization.
                StartCoroutine(SpawningSequence(entry.monsterData.MonsterPrefab, entry.spawnDelay, entry.count, SetStrategy(entry)));
            }
            else
            {
                Debug.LogWarning($"MonsterEntry in wave {currentWaveIndex} has missing data and will be skipped.");
            }
        }
    }

    /// <summary>
    /// The coroutine that handles spawning a single group of monsters over time.
    /// </summary>
    IEnumerator SpawningSequence(GameObject monsterPrefab, int delay, int count, ISpawnStrategy strategy)
    {
        activeSpawningSequences++;
        for (int i = 0; i < count; i++)
        {
            List<Vector3> validPositions = strategy.GetValidPosition(1);
            List<GameObject> spawned = strategy.SpawnXMonster(monsterPrefab, validPositions);

            foreach (GameObject enemy in spawned)
            {
                if (enemy != null)
                {
                    activeEnemies.Add(enemy);
                    enemy.transform.LookAt(playerTransform);
                }
            }
            
            yield return new WaitForSeconds(delay);
        }
        activeSpawningSequences--;
        CheckWaveEnd();
    }

    /// <summary>
    /// Event handler that listens for an enemy's death.
    /// </summary>
    [FluxEventHandler]
    private void OnEnemyDie(EnemyDieEvent evt)
    {
        // Clean up the list to prevent issues with destroyed objects.
        activeEnemies.RemoveAll(e => e == null || e == evt.enemy);
        CheckWaveEnd();
    }

    /// <summary>
    /// Checks if the conditions for ending the current wave are met.
    /// </summary>
    private void CheckWaveEnd()
    {
        // The wave is over only if no enemies are left AND all spawning coroutines have finished.
        if (activeEnemies.Count == 0 && activeSpawningSequences == 0)
        {
            Debug.Log($"Wave {currentWaveIndex} complete!");
            // this.PublishEvent(new WaveEndEvent(currentWaveIndex));
            StartCoroutine(WaitBeforeNextWave());
        }
    }

    /// <summary>
    /// A simple delay between waves.
    /// </summary>
    IEnumerator WaitBeforeNextWave()
    {
        yield return new WaitForSeconds(5f); // Consider making this delay configurable in the SOWaveManager.
        StartNextWave();
    }

    /// <summary>
    /// Clean up the subscription when this object is destroyed to prevent memory leaks.
    /// </summary>
    protected override void OnFluxDestroy()
    {
        _playerTransformSubscription?.Dispose();
        base.OnFluxDestroy();
    }
}