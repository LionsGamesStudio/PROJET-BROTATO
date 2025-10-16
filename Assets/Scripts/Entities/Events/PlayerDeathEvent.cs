using FluxFramework.Core;

public class PlayerDeathEvent : FluxEventBase
{
    public int WavesCompleted;

    public PlayerDeathEvent(int wavesCompleted = 0)
    {
        WavesCompleted = wavesCompleted;
    }
}