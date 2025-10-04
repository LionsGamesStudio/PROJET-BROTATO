using System.Collections.Generic;
using FluxFramework.Attributes;
using FluxFramework.Core;
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

    [FluxAction]
    public bool AddItem(ItemData item, int quantity = 1)
    {
        if (item == null || quantity <= 0) return false;

        // Look for an existing slot with the same item
        for (int i = 0; i < inventoryData.slots.Count; i++)
        {
            var slot = inventoryData.slots[i];
            if (slot.itemData?.ID == item.ID)
            {
                slot.quantity += quantity;
                UpdateInventoryProperty();
                PublishEvent(new ItemAddedEvent(item, quantity));
                return true;
            }
        }

        // Look for an empty slot
        for (int i = 0; i < inventoryData.slots.Count; i++)
        {
            var slot = inventoryData.slots[i];
            if (slot.IsEmpty)
            {
                slot.itemData = item;
                slot.quantity = quantity;
                UpdateInventoryProperty();
                PublishEvent(new ItemAddedEvent(item, quantity));
                return true;
            }
        }

        return false; // Inventory full
    }

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
                        slot.itemData = null;
                        slot.quantity = 0;
                    }
                    UpdateInventoryProperty();
                    PublishEvent(new ItemRemovedEvent(itemId, quantity));
                    return true;
                }
                break;
            }
        }
        return false;
    }

    [FluxAction]
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

    private void UseConsumable(ConsumableData consumable)
    {
        if (consumable == null) return;

        // Restore health
        if (consumable.HealthRestore > 0f && ownerHealth != null)
        {
            ownerHealth.Heal(consumable.HealthRestore);
        }

        // Apply buffs
        if (consumable.BuffsToApply != null && buffManager != null)
        {
            foreach (var buff in consumable.BuffsToApply)
            {
                buffManager.ApplyBuff(buff);
            }
        }

        PublishEvent(new ConsumableUsedEvent(consumable));
    }

    private void EquipWeapon(WeaponData weapon)
    {
        if (weapon == null) return;

        // Look for an empty weapon slot or replace the first one
        for (int i = 0; i < inventoryData.equippedWeapons.Count; i++)
        {
            if (inventoryData.equippedWeapons[i] == null)
            {
                inventoryData.equippedWeapons[i] = weapon;
                UpdateWeaponsProperty();
                PublishEvent(new WeaponEquippedEvent(weapon, i));
                return;
            }
        }

        // Replace the first weapon if all slots are full
        if (inventoryData.equippedWeapons.Count > 0)
        {
            var oldWeapon = inventoryData.equippedWeapons[0];
            inventoryData.equippedWeapons[0] = weapon;
            UpdateWeaponsProperty();
            
            // Return the old weapon to inventory
            if (oldWeapon != null)
            {
                AddItem(oldWeapon);
            }
            
            PublishEvent(new WeaponEquippedEvent(weapon, 0));
        }
    }

    private void UpdateInventoryProperty()
    {
        // Force update the reactive property
        foreach (var slot in inventoryData.slots)
        {
            AddToReactiveCollection<InventorySlot>("inventory.slots", slot);
        }
    }

    private void UpdateWeaponsProperty()
    {
        foreach (var weapon in inventoryData.equippedWeapons)
        {
            AddToReactiveCollection<WeaponData>("inventory.equippedWeapons", weapon);
        }
    }
}