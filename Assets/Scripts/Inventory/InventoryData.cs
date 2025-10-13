using System.Collections.Generic;
using FluxFramework.Attributes;
using FluxFramework.Core;
using FluxFramework.Extensions;
using UnityEngine;


[CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory/Inventory Data")]
public class InventoryData : FluxDataContainer
{
    [Header("Inventory Settings")]
    [SerializeField] private int maxSlots = 20;
    [SerializeField] private int maxWeapons = 4;
    
    [ReactiveProperty("inventory.slots")]
    public ReactiveCollection<InventorySlot> slots;

    [ReactiveProperty("inventory.equippedWeapons")]
    public ReactiveCollection<WeaponData> equippedWeapons;

    [FluxButton("Reset Inventory")]
    protected override void OnDataContainerInitialized()
    {
        Debug.Log("InventoryData: Initializing or validating data container...");
        
        // --- Initialize the ReactiveCollection for inventory slots ---
        slots = new ReactiveCollection<InventorySlot>();

        // Ensure the slots collection has the correct size.
        if (slots.Count != maxSlots)
        {
            Debug.Log($"InventoryData: Re-initializing inventory slots to size {maxSlots}.");
            slots.Clear(); // Clear any existing data before re-initializing.
            for (int i = 0; i < maxSlots; i++)
            {
                // Add empty slots to the collection.
                slots.Add(new InventorySlot(null, 0, i));
            }
        }

        // --- Initialize the ReactiveCollection for equipped weapons ---
        equippedWeapons = new ReactiveCollection<WeaponData>();

        // This is the crucial part: ensure the equipped weapons collection has
        // exactly 'maxWeapons' items, using 'null' for empty slots.
        // This prevents out-of-range exceptions when trying to access an index directly.
        if (equippedWeapons.Count != maxWeapons)
        {
            Debug.Log($"InventoryData: Re-initializing equipped weapons slots to size {maxWeapons}.");
            equippedWeapons.Clear();
            for (int i = 0; i < maxWeapons; i++)
            {
                // Add null placeholders for each weapon slot.
                equippedWeapons.Add(null);
            }
        }
    }
}