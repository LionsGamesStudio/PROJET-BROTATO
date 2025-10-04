using System.Collections.Generic;
using FluxFramework.Attributes;
using FluxFramework.Core;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryData", menuName = "Inventory/Inventory Data")]
public class InventoryData : FluxDataContainer
{
    [Header("Inventory Settings")]
    [SerializeField] private int maxSlots = 20;
    [SerializeField] private int maxWeapons = 4;
    
    [ReactiveProperty("inventory.slots")]
    public List<InventorySlot> slots = new List<InventorySlot>();
    
    [ReactiveProperty("inventory.equippedWeapons")]
    public List<WeaponData> equippedWeapons = new List<WeaponData>();

    protected override void OnDataContainerInitialized()
    {
        if (slots == null || slots.Count != maxSlots)
        {
            slots = new List<InventorySlot>(maxSlots);
            for (int i = 0; i < maxSlots; i++)
            {
                slots.Add(new InventorySlot(null, 0, i));
            }
        }

        if (equippedWeapons == null || equippedWeapons.Count != maxWeapons)
        {
            equippedWeapons = new List<WeaponData>(maxWeapons);
        }
    }
}