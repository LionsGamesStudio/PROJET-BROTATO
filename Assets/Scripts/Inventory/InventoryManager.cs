using System.Collections.Generic;
using FluxFramework.Attributes;
using FluxFramework.Core;
using FluxFramework.Extensions;
using UnityEngine;

public class InventoryManager : FluxMonoBehaviour
{
    [Header("Dependencies")]
    [SerializeField] private InventoryData inventoryData;
    [SerializeField] private HealthComponent ownerHealth;
    [SerializeField] private BuffManager buffManager;

    public InventoryData GetInventoryData() => inventoryData;
    
    protected override void OnFluxAwake()
    {
        if (inventoryData == null)
        {
            Debug.LogError("InventoryManager needs an InventoryData asset assigned!");
            return;
        }
    }

    [FluxAction("Add Item to Inventory", ButtonText = "Add Item")]
    public bool AddItem(ItemData item, int quantity = 1)
    {
        if (item == null || quantity <= 0) return false;

        for (int i = 0; i < inventoryData.slots.Count; i++)
        {
            var slot = inventoryData.slots[i];
            if (slot.itemData?.ID == item.ID)
            {
                slot.quantity += quantity;
                this.PublishEvent(new ItemAddedEvent(item, quantity));
                return true;
            }
        }

        // If not stackable or item not found, look for an empty slot.
        for (int i = 0; i < inventoryData.slots.Count; i++)
        {
            var slot = inventoryData.slots[i];
            if (slot.IsEmpty)
            {
                slot.itemData = item;
                slot.quantity = quantity;
                this.PublishEvent(new ItemAddedEvent(item, quantity));
                return true;
            }
        }

        Debug.Log("Inventory is full. Cannot add item.");
        return false; // Inventory is full.
    }

    [FluxAction("Remove Item from Inventory", ButtonText = "Remove Item")]
    public bool RemoveItem(int itemId, int quantity = 1)
    {
        for (int i = 0; i < inventoryData.slots.Count; i++)
        {
            var slot = inventoryData.slots[i];
            if (slot.itemData?.ID == itemId)
            {
                if (slot.quantity >= quantity)
                {
                    slot.quantity -= quantity;
                    if (slot.quantity <= 0)
                    {
                        // Clear the slot if quantity drops to zero or below.
                        slot.itemData = null;
                        slot.quantity = 0;
                    }
                    this.PublishEvent(new ItemRemovedEvent(itemId, quantity));
                    return true;
                }
                break;
            }
        }
        return false;
    }

    [FluxAction("Use Item", ButtonText = "Use Item")]
    public void UseItem(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= inventoryData.slots.Count) return;

        var slot = inventoryData.slots[slotIndex];
        if (slot.IsEmpty) return;

        switch (slot.itemData.Type)
        {
            case ItemType.Consumable:
                UseConsumable(slot.itemData as ConsumableData);
                RemoveItem(slot.itemData.ID, 1);
                break;

            case ItemType.Weapon:
                EquipWeapon(slot.itemData as WeaponData);
                break;
        }
    }
    
    /// <summary>
    /// Checks if an item can be added to the inventory without actually adding it.
    /// </summary>
    /// <param name="item">The item to check.</param>
    /// <param name="quantity">The quantity of the item.</param>
    /// <returns>True if there is space, false otherwise.</returns>
    public bool CanAddItem(ItemData item, int quantity = 1)
    {
        if (item == null || quantity <= 0) return false;

        // First, check if the item can be stacked on an existing slot.
        // This doesn't require a new slot, so it's always possible.
        for (int i = 0; i < inventoryData.slots.Count; i++)
        {
            var slot = inventoryData.slots[i];
            if (slot.itemData?.ID == item.ID)
            {
                return true; // Stacking is possible.
            }
        }

        // If stacking is not possible, check for at least one empty slot.
        for (int i = 0; i < inventoryData.slots.Count; i++)
        {
            var slot = inventoryData.slots[i];
            if (slot.IsEmpty)
            {
                return true; // An empty slot is available.
            }
        }

        // If no stacking is possible and no empty slots are found, the item cannot be added.
        return false;
    }
    
    private void UseConsumable(ConsumableData consumable)
    {
        if (consumable == null) return;

        if (consumable.HealthRestore > 0f && ownerHealth != null)
        {
            ownerHealth.Heal(consumable.HealthRestore);
        }

        if (consumable.BuffsToApply != null && buffManager != null)
        {
            foreach (var buff in consumable.BuffsToApply)
            {
                buffManager.ApplyBuff(buff);
            }
        }

        this.PublishEvent(new ConsumableUsedEvent(consumable));
    }

    private void EquipWeapon(WeaponData weapon)
    {
        if (weapon == null) return;

        // Case 1: Look for an empty weapon slot. This is always allowed.
        for (int i = 0; i < inventoryData.equippedWeapons.Count; i++)
        {
            if (inventoryData.equippedWeapons[i] == null)
            {
                inventoryData.equippedWeapons[i] = weapon;
                this.PublishEvent(new WeaponEquippedEvent(weapon, i));
                return;
            }
        }

        // Case 2: All slots are full, so we attempt to replace the first weapon.
        if (inventoryData.equippedWeapons.Count > 0)
        {
            var oldWeapon = inventoryData.equippedWeapons[0];
            
            // --- CRITICAL CHECK ---
            // Before replacing the weapon, check if the old weapon can be returned to the inventory.
            if (oldWeapon != null && !CanAddItem(oldWeapon))
            {
                Debug.Log($"Cannot equip {weapon.Name}: Inventory is full and cannot accommodate the currently equipped weapon ({oldWeapon.Name}).");
                // Optional: Publish an event to notify the UI to show a "Inventory Full" message.
                // this.PublishEvent(new InventoryFullEvent());
                return; // Abort the equip operation.
            }

            // If the check passes, proceed with the replacement.
            inventoryData.equippedWeapons[0] = weapon;
            
            if (oldWeapon != null)
            {
                // This will now succeed because we checked it beforehand.
                AddItem(oldWeapon);
            }
            
            this.PublishEvent(new WeaponEquippedEvent(weapon, 0));
        }
    }
}