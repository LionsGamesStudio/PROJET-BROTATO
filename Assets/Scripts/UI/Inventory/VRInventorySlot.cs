using UnityEngine;

public class VRInventorySlot : MonoBehaviour
{
    [Header("Slot UI")]
    [SerializeField] private UnityEngine.UI.Image iconImage;
    [SerializeField] private TMPro.TextMeshProUGUI quantityText;
    
    private int slotIndex;
    private VRInventoryUI parentUI;
    
    public void Initialize(int index, VRInventoryUI parent)
    {
        slotIndex = index;
        parentUI = parent;
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
            iconImage.sprite = slot.itemData.Icon;
            iconImage.color = Color.white;
            quantityText.text = slot.quantity > 1 ? slot.quantity.ToString() : "";
        }
    }
    
    public void OnVRInteract()
    {
        parentUI?.OnSlotInteracted(slotIndex);
    }
}