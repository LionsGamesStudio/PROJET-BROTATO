using FluxFramework.Core;

/// <summary>
/// This event is published when a player interacts with an inventory slot in the UI.
/// </summary>
public class SlotInteractedEvent : FluxEventBase
{
    /// <summary>
    /// The index of the slot that was interacted with.
    /// </summary>
    public int SlotIndex { get; }

    public SlotInteractedEvent(int slotIndex)
    {
        SlotIndex = slotIndex;
    }
}