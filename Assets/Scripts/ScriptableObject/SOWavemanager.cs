using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewWaveManagement", menuName = "ScriptableObject/WaveManagement")]
public class SOWaveManager : ScriptableObject
{
    [SerializeField]
    private List<SOWaves> sOWaves;
    public List<SOWaves> SOWaves => sOWaves; // GET THE FUCKING WAVES

}
