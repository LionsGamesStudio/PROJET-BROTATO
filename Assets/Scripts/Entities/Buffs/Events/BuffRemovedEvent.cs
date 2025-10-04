using FluxFramework.Core;

public class BuffRemovedEvent : FluxEventBase
{
    public string EntityId { get; }
    public BuffData BuffData { get; }
    public BuffRemovedEvent(BuffData buffData, string entityId) { BuffData = buffData; EntityId = entityId; }
}