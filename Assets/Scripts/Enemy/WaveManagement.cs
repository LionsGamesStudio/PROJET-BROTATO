using System.Collections;
using System.Collections.Generic;
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

    private int aliveEnemies = 0;
    private int activeSpawningSequences = 0;

    [SerializeField]
    private int countWaves = 0;

    private int numberOfWaves;

    private bool inWave = false;

    public void Start()
    {
        InitializeComponent();
        NextWave();
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
        inWave = true;
        foreach (MonsterEntry entry in waves.Monsters)
        {
            activeSpawningSequences++;
            StartCoroutine(SpawningSequence(entry.monster, entry.spawnDelay, entry.count, SetStrategy(entry)));
        }
    }

    IEnumerator SpawningSequence(GameObject monster, int delay, int count, ISpawnStrategy strategy)
    {
        for (int i = 0; i < count; i++)
        {
            Debug.Log("La séquence de spawn est lancé !");
            List<Vector3> validPositions = strategy.GetValidPosition(count); // Get all the position of the monster needed for the spawing strategy
            count -= strategy.SpawnXMonster(monster, validPositions); // Spawn the monster and count how many it cost for the player
            //aliveEnemies++; à gérer + tard
            yield return new WaitForSeconds(delay); // Wait
            
            // Au cas ou gérer la liste des position valide dans ce script et dans les strategy
        }

        activeSpawningSequences--;
        CheckWaveEnd();

    }

    public void NotifyEnemyDeath() // To take when an enemy DIE, maybe to change with an Event ?
    {
        aliveEnemies--;
        CheckWaveEnd();
    }

    private void CheckWaveEnd()
    {
        if (activeSpawningSequences == 0 && aliveEnemies == 0 && inWave)
        {
            Debug.Log("Vague terminée !");
            inWave = false;
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
