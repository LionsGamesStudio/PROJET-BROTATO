using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    public ItemData itemData;
    public int quantity;

    [HideInInspector]
    public int slotIndex;

    public InventorySlot(ItemData item, int qty, int index)
    {
        itemData = item;
        quantity = qty;
        slotIndex = index;
    }

    public bool IsEmpty => itemData == null || quantity <= 0;
}