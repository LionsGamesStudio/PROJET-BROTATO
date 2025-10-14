using UnityEngine;
using FluxFramework.Core;
using FluxFramework.Extensions;


public class VRInventorySlot : FluxMonoBehaviour
{
    [Header("Slot UI")]
    [SerializeField] private UnityEngine.UI.Image iconImage;
    [SerializeField] private TMPro.TextMeshProUGUI quantityText;

    private int slotIndex;

    public void Initialize(int index)
    {
        slotIndex = index;
    }

    public void UpdateSlot(InventorySlot slot)
    {
        if (slot.IsEmpty)
        {
            iconImage.sprite = null;
            iconImage.color = Color.clear;
            quantityText.text = "";
        }
        else
        {
            string text = slot.itemData.Name + " : " + (slot.quantity > 0 ? slot.quantity.ToString() : "0");
            iconImage.sprite = slot.itemData.Icon;
            iconImage.color = Color.white;
            quantityText.text = text;
        }
    }

    /// <summary>
    /// Called when the player interacts with this slot in VR.
    /// </summary>
    public void OnVRInteract()
    {
        this.PublishEvent(new SlotInteractedEvent(slotIndex));
    }
}