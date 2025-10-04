public class ActiveBuff
{
    public BuffData Data { get; private set; }
    public float RemainingTime { get; private set; }
    public bool IsPermanent => Data.Duration < 0f;

    private BuffManager _manager;

    public ActiveBuff(BuffData data, BuffManager manager)
    {
        Data = data;
        RemainingTime = data.Duration;
        _manager = manager;
    }

    public void RefreshDuration(float newDuration)
    {
        RemainingTime = newDuration;
    }

    public bool Update(float deltaTime)
    {
        if (IsPermanent) return false;
        
        RemainingTime -= deltaTime;
        return RemainingTime <= 0f;
    }
}
