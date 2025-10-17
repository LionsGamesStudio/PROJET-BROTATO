using FluxFramework.Attributes;
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
    [Tooltip("This should be the 'Content' object of your ScrollView.")]
    [SerializeField] private Transform slotParent;
    [SerializeField] private VRInventorySlot slotPrefab;
    
    private List<VRInventorySlot> _activeSlotUIs = new List<VRInventorySlot>();
    private InventoryData inventoryData;

    protected override void OnFluxStart()
    {
        base.OnFluxStart();
        
        if (inventoryManager == null)
        {
            inventoryManager = FindObjectOfType<InventoryManager>();
            if (inventoryManager == null)
            {
                Debug.LogError($"VRInventoryUI on '{gameObject.name}' is missing its 'inventoryManager' dependency. Disabling UI.", this);
                gameObject.SetActive(false);
                return;
            }
        }
        
        inventoryData = inventoryManager.GetInventoryData();
        if (inventoryData == null)
        {
            Debug.LogError("VRInventoryUI could not find InventoryData via the assigned InventoryManager!", this);
            gameObject.SetActive(false);
            return;
        }
        
        // Build the initial state of the UI on startup.
        RebuildUI();
    }

    /// <summary>
    /// Listens for the ItemAddedEvent and triggers a full UI rebuild.
    /// </summary>
    [FluxEventHandler]
    private void OnItemAdded(ItemAddedEvent evt)
    {
        RebuildUI();
    }

    /// <summary>
    /// Listens for the ItemRemovedEvent and triggers a full UI rebuild.
    /// </summary>
    [FluxEventHandler]
    private void OnItemRemoved(ItemRemovedEvent evt)
    {
        RebuildUI();
    }

    /// <summary>
    /// Rebuilds the entire inventory UI from scratch based on the current state
    /// of the inventoryData.
    /// </summary>
    private void RebuildUI()
    {
        // Ensure data is available before proceeding.
        if (inventoryData == null) return;

        // 1. Destroy all previously instantiated UI slots.
        foreach (VRInventorySlot slotUI in _activeSlotUIs)
        {
            Destroy(slotUI.gameObject);
        }
        _activeSlotUIs.Clear();

        // 2. Iterate through the authoritative data source (inventoryData.slots).
        for(int i = 0; i < inventoryData.slots.Count; i++)
        {
            InventorySlot slotData = inventoryData.slots[i];
            
            // Only create a visual slot for items that actually exist.
            if (!slotData.IsEmpty)
            {
                // 3. Instantiate a new UI slot prefab for each valid item.
                VRInventorySlot newSlotUI = Instantiate(slotPrefab, slotParent);
                
                // Initialize the slot with its ORIGINAL index from the data source.
                newSlotUI.Initialize(slotData.slotIndex);
                
                // Update the slot's visuals.
                newSlotUI.UpdateSlot(slotData);
                
                _activeSlotUIs.Add(newSlotUI);
            }
        }
    }
}