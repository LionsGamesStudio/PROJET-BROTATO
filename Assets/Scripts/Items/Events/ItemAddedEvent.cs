using FluxFramework.Core;

public class ItemAddedEvent : FluxEventBase
{
    public ItemData Item { get; }
    public int Quantity { get; }
    public ItemAddedEvent(ItemData item, int quantity) { Item = item; Quantity = quantity; }
}