public interface IConsumable : IItem
{
    float HealthRestore { get; }
    BuffData[] BuffsToApply { get; }
}