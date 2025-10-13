using FluxFramework.Core;
using FluxFramework.Extensions;
using FluxFramework.UI;
using System.Collections.Generic;
using UnityEngine;

public class VRInventoryUI : FluxUIComponent
{
    [Header("Dependencies")]
    [Tooltip("The InventoryManager that this UI will display. This is a required dependency.")]
    [SerializeField] private InventoryManager inventoryManager;

    [Header("VR Inventory UI")]
    [SerializeField] private Transform slotParent;
    [SerializeField] private VRInventorySlot slotPrefab;
    
    private VRInventorySlot[] slotUIs;
    private InventoryData inventoryData;

    protected override void OnFluxStart()
    {
        base.OnFluxStart();
        
        // --- Dependency Validation ---
        if (inventoryManager == null)
        {
            // Try to find it as a last resort, but log a warning.
            inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager == null)
            {
                Debug.LogError($"VRInventoryUI on '{gameObject.name}' is missing its dependency: 'inventoryManager'. Please assign it in the inspector. Disabling UI.", this);
                gameObject.SetActive(false); // Disable the whole UI object.
                return;
            }
            else
            {
                Debug.LogWarning($"VRInventoryUI on '{gameObject.name}' found its 'inventoryManager' dependency at runtime. For better performance and reliability, please assign it manually in the inspector.", this);
            }
        }
        
        inventoryData = inventoryManager.GetInventoryData();
        if (inventoryData == null)
        {
            Debug.LogError("VRInventoryUI could not find InventoryData via the assigned InventoryManager!", this);
            gameObject.SetActive(false);

            return;
        }
        
        CreateSlotUIs();
    }

    protected override void RegisterCustomBindings()
    {
        this.SubscribeToProperty<ReactiveCollection<InventorySlot>>("inventory.slots", OnInventoryChanged, fireOnSubscribe: true);
    }

    private void CreateSlotUIs()
    {
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

    private void OnInventoryChanged(ReactiveCollection<InventorySlot> newSlots)
    {
        if (slotUIs == null || newSlots == null) return;
        
        for (int i = 0; i < Mathf.Min(slotUIs.Length, newSlots.Count); i++)
        {
            slotUIs[i].UpdateSlot(newSlots[i]);
        }
    }



    public void OnSlotInteracted(int slotIndex)
    {
        // Use the cached reference, which is more reliable and performant.
        inventoryManager?.UseItem(slotIndex);
    }
}