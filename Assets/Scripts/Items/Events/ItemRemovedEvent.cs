using FluxFramework.Core;

public class ItemRemovedEvent : FluxEventBase
{
    public int ItemId { get; }
    public int Quantity { get; }
    public ItemRemovedEvent(int itemId, int quantity) { ItemId = itemId; Quantity = quantity; }
}