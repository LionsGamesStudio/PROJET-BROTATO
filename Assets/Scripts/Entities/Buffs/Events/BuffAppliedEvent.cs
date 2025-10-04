using FluxFramework.Core;

public class BuffAppliedEvent : FluxEventBase
{
    public string EntityId { get; }
    public BuffData BuffData { get; }
    public BuffAppliedEvent(BuffData buffData, string entityId) { BuffData = buffData; EntityId = entityId; }
}