using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class Item : MonoBehaviour
{
    private ItemData itemData;

    public ItemData ItemData => itemData;

    public void Initialize(ItemData data)
    {
        itemData = data;
        if (data == null)
        {
            Debug.LogError("Item initialized with null ItemData!");
            return;
        }

        SphereCollider collider = GetComponent<SphereCollider>();
        if (collider == null)
        {
            Debug.LogError("Item requires a SphereCollider component.");
            return;
        }
        collider.isTrigger = true;
        collider.enabled = true;
        collider.radius = data.PickupRadius;
    }
    
    private void OnTriggerEnter(Collider other)
    {
        InventoryManager inventoryManager = other.GetComponent<InventoryManager>();
        Debug.Log($"Item: Detected collision with {other.gameObject.name}");
        if (inventoryManager != null && itemData != null)
        {
            if (inventoryManager.AddItem(itemData, 1))
            {
                Destroy(gameObject);
            }
        }
    }
}