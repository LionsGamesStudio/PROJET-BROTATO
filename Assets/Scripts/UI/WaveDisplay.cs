using System;
using System.Collections;
using UnityEngine;


public class WaveDisplay : MonoBehaviour
{
    [SerializeField] private TMPro.TextMeshProUGUI waveText;

    private void Start()
    {
        UpdateWaveText(0);
    }

    public void UpdateWaveText(int waveNumber)
    {
        waveText.text = $"{waveNumber}";
    }
}