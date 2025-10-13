public class ActiveBuff
{
    public BuffData Data { get; private set; }
    public float RemainingTime { get; private set; }
    public bool IsPermanent => Data.Duration < 0f;

    // The current number of stacks for this buff instance.
    public int CurrentStacks { get; private set; }

    private BuffManager _manager;

    public ActiveBuff(BuffData data, BuffManager manager)
    {
        Data = data;
        _manager = manager;
        RemainingTime = data.Duration;
        // A buff always starts with 1 stack.
        CurrentStacks = 1;
    }

    /// <summary>
    /// Refreshes the duration of the buff.
    /// </summary>
    public void RefreshDuration()
    {
        RemainingTime = Data.Duration;
    }
    
    /// <summary>
    /// Adds a new stack to the buff, up to the maximum defined in BuffData.
    /// Returns true if a stack was successfully added.
    /// </summary>
    public bool AddStack()
    {
        if (CurrentStacks < Data.MaxStacks)
        {
            CurrentStacks++;
            return true;
        }
        return false;
    }

    /// <summary>
    /// Updates the buff's remaining time.
    /// </summary>
    /// <param name="deltaTime">The time elapsed since the last frame.</param>
    /// <returns>True if the buff has expired and should be removed.</returns>
    public bool Update(float deltaTime)
    {
        if (IsPermanent) return false;
        
        RemainingTime -= deltaTime;
        return RemainingTime <= 0f;
    }
}