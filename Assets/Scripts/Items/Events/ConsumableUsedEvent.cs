using FluxFramework.Core;

public class ConsumableUsedEvent : FluxEventBase
{
    public ConsumableData Consumable { get; }
    public ConsumableUsedEvent(ConsumableData consumable) { Consumable = consumable; }
}
