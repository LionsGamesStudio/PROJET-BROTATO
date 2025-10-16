using FluxFramework.Core;
using UnityEngine;


namespace Events
{
    public class NextWaveEvent : FluxEventBase
    {
        public int WaveFinished;

        public NextWaveEvent(int waveFinished)
        {
            WaveFinished = waveFinished;
        }
    }
}