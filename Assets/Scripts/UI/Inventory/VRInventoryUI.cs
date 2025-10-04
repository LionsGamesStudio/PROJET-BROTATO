using FluxFramework.UI;
using UnityEngine;

public class VRInventoryUI : FluxUIComponent
{
    [Header("VR Inventory UI")]
    [SerializeField] private Transform slotParent;
    [SerializeField] private VRInventorySlot slotPrefab;
    
    private VRInventorySlot[] slotUIs;
    private InventoryData inventoryData;

    protected override void OnFluxStart()
    {
        base.OnFluxStart();
        
        var inventoryManager = FindObjectOfType<InventoryManager>();
        if (inventoryManager != null)
        {
            inventoryData = inventoryManager.GetInventoryData();
        }
        
        if (inventoryData == null)
        {
            Debug.LogError("VRInventoryUI couldn't find InventoryData!");
            return;
        }
        
        CreateSlotUIs();
    }

    protected override void RegisterCustomBindings()
    {
        // S'abonner aux changements d'inventaire
        SubscribeToProperty<InventorySlot[]>("inventory.slots", OnInventoryChanged, fireOnSubscribe: true);
    }

    private void CreateSlotUIs()
    {
        // Créer les slots UI pour VR
        if (inventoryData != null && inventoryData.slots != null)
        {
            slotUIs = new VRInventorySlot[inventoryData.slots.Count];
            
            for (int i = 0; i < inventoryData.slots.Count; i++)
            {
                var slotUI = Instantiate(slotPrefab, slotParent);
                slotUI.Initialize(i, this);
                slotUIs[i] = slotUI;
            }
        }
    }

    private void OnInventoryChanged(InventorySlot[] newSlots)
    {
        if (slotUIs == null || newSlots == null) return;
        
        for (int i = 0; i < Mathf.Min(slotUIs.Length, newSlots.Length); i++)
        {
            slotUIs[i].UpdateSlot(newSlots[i]);
        }
    }

    public void OnSlotInteracted(int slotIndex)
    {
        // Logique d'interaction VR avec un slot
        var inventoryManager = FindObjectOfType<InventoryManager>();
        inventoryManager?.UseItem(slotIndex);
    }
}