using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public enum SpawningMode
{
    Random,
    Line,  
}

[CreateAssetMenu(fileName = "NewWave", menuName = "ScriptableObject/Waves")]
public class SOWaves : ScriptableObject
{
    [Header("Data")]
    [SerializeField]

    private List<MonsterEntry> monsters;

    public List<MonsterEntry> Monsters => monsters;

}

[System.Serializable]
public class MonsterEntry
{
    public GameObject monster;
    public int count;
    public int spawnDelay;
    public SpawningMode spawningMode = SpawningMode.Random; // Récupérer ensuite par le waveManager

}
