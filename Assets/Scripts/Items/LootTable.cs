using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LootTable
{
    [SerializeField] private List<Loot> lootItems = new List<Loot>();

    public Loot GetRandomLoot()
    {
        if (lootItems == null || lootItems.Count == 0)
            return null;

        float totalChance = 0f;
        foreach (var loot in lootItems)
        {
            totalChance += loot.DropChance;
        }

        float randomValue = UnityEngine.Random.Range(0, totalChance);
        float cumulativeChance = 0f;
        foreach (var loot in lootItems)
        {
            cumulativeChance += loot.DropChance;
            if (randomValue <= cumulativeChance)
            {
                return loot;
            }
        }

        return null; // Fallback, should not reach here if chances are set correctly.
    }
}

[Serializable]
public class Loot
{
    public ItemData Item;
    public int Quantity;

    public float DropChance; // Value between 0 and 1

    public Loot(ItemData item, int quantity, float dropChance)
    {
        Item = item;
        Quantity = quantity;
        DropChance = dropChance;
    }
}