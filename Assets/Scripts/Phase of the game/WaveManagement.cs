using System.Collections;
using System.Collections.Generic;
using Events;
using UnityEditor.Rendering;
using UnityEngine;

[DisallowMultipleComponent] // Evite d'avoir plusieurs waves management
public class WaveManagement : MonoBehaviour
{
    [Header("Data to insert")]
    [SerializeField]
    private SOWaveManager sOWaveManagement;

    [SerializeField]
    private GameObject player;

    [SerializeField]
    private int countWaves = 0;

    [SerializeField]

    private List<GameObject> enemyInWave = new List<GameObject>();

    // --------------------------------------------
    
    private int numberOfWaves;
    private int actualSequence = 0;

    public void Start()
    {
        InitializeComponent();
        NextWave();
        FluxFramework.Core.Flux.Manager.EventBus.Subscribe<EnemyDieEvent>(OnEnemyDie);
    }

    private void InitializeComponent()
    {
        if (player == null)
        {
            Debug.Log("Player is missing in the waveManager !");
        }

        // A faire

        // Regarde si SOWaves existe + est correct 
        // Regarder si MonstersEntry existe + est correct 
        // Regarder si le prefab est monstre qui à une interface d'enemy

        numberOfWaves = sOWaveManagement.SOWaves.Count;
    }

    private ISpawnStrategy SetStrategy(MonsterEntry monsterEntry)
    {
        ISpawnStrategy strategy = monsterEntry.spawningMode switch
        {
            SpawningMode.Random => new RandomSpawnStrategy(player),
            SpawningMode.Line => new LineSpawnStrategy(player),
            _ => new RandomSpawnStrategy(player),
        };

        return strategy;
    }

    private void NextWave()
    {
        countWaves++;

        if (countWaves <= numberOfWaves)
        {
            GetWave(sOWaveManagement.SOWaves[countWaves - 1]);
        }

        else
        {
            Debug.Log("Fin du jeu !"); 
        }


    }

    private void GetWave(SOWaves waves)
    {
        foreach (MonsterEntry entry in waves.Monsters)
        {
            StartCoroutine(SpawningSequence(entry.monster, entry.spawnDelay, entry.count, SetStrategy(entry)));
        }
    }

    IEnumerator SpawningSequence(GameObject monster, int delay, int count, ISpawnStrategy strategy)
    {
        actualSequence++;
        for (int i = 0; i < count; i++)
        {
            Debug.Log("La séquence de spawn est lancé !");

            List<Vector3> validPositions = strategy.GetValidPosition(count); // Get all the position of the monster needed for the spawing strategy without taken in consideration monsters

            List<GameObject> temp = strategy.SpawnXMonster(monster, validPositions); // Spawn the monster and count how many it cost for the player

            foreach (GameObject enemy in temp)
            {
                if (enemy != null) enemyInWave.Add(enemy);
                else Debug.LogWarning("Un monstre n'a pas pu être instancié !");
            }


            count -= temp.Count;  // We are deleting the number of new monster

            yield return new WaitForSeconds(delay); // Wait

        }
        actualSequence--;
        CheckWaveEnd();
    }

    private void OnEnemyDie(EnemyDieEvent evt)
    {
        RemoveEnemy(evt.enemy);
        CheckWaveEnd();
    }

    private void RemoveEnemy(GameObject enemy)
    {
        // enemyInWave.Remove(enemy); Work
        enemyInWave.RemoveAll(e => e == null || e == enemy); // Just in case
    }

    private void CheckWaveEnd()
    {
        if (enemyInWave.Count == 0 && actualSequence == 0)
        {
            Debug.Log("Vague terminée !");
            StartCoroutine(WaitBeforeNextWave());
        }
    }

    IEnumerator WaitBeforeNextWave()
    {
        yield return new WaitForSeconds(5f); // Wait before end of shop is to make after
        NextWave();
    }


    //A faire sois un timer sois je tue tous les mobs pour finir la wave 

}
