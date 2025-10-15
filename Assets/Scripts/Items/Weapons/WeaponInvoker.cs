using System.Collections.Generic;
using FluxFramework.Core;
using UnityEngine;

public class WeaponInvoker : FluxMonoBehaviour
{
    [Tooltip("List of spawn points where weapons will be instantiated or attached.")]
    public List<Transform> spawnPoints = new List<Transform>();

    // Internal tracking of active weapon GameObjects.
    private List<GameObject> activeWeapons = new List<GameObject>();
    
    // A pool of inactive weapon instances to reduce instantiation overhead.
    private Dictionary<int, List<GameObject>> pooledWeapons = new Dictionary<int, List<GameObject>>();
    private Transform poolParent;

    protected override void OnFluxStart()
    {
        // Create a parent object to hold pooled weapons neatly in the hierarchy.
        poolParent = new GameObject("WeaponPool").transform;
        poolParent.SetParent(this.transform);

        // Initialize the active weapons list to match the number of spawn points.
        for (int i = 0; i < spawnPoints.Count; i++)
        {
            activeWeapons.Add(null); 
        }

        // Subscribe to the event that triggers weapon equipping.
        FluxFramework.Core.Flux.Manager.EventBus.Subscribe<WeaponEquippedEvent>(OnWeaponEquipped);
    }

    private void OnWeaponEquipped(WeaponEquippedEvent evt)
    {
        // Validate the slot index from the event.
        if (evt.SlotIndex < 0 || evt.SlotIndex >= spawnPoints.Count)
        {
            Debug.LogError($"WeaponInvoker: Invalid SlotIndex {evt.SlotIndex}.");
            return;
        }

        // If a weapon already exists in this slot, release it first.
        if (activeWeapons[evt.SlotIndex] != null)
        {
            ReleaseWeapon(evt.SlotIndex);
        }

        // Get a new weapon instance, either from the pool or by instantiating it.
        GameObject newWeaponInstance = GetWeapon(evt.Weapon.Prefab, spawnPoints[evt.SlotIndex]);
        activeWeapons[evt.SlotIndex] = newWeaponInstance;

        var attackerComponent = newWeaponInstance.GetComponent<PlayerWeaponAttackerComponent>();
        if (attackerComponent != null)
        {
            // 1. Initialize the component with its unique ID (the slot index).
            //    This allows it to generate its unique reactive property keys.
            attackerComponent.Initialize(evt.SlotIndex);

            // 2. Tell the component to equip the weapon. This will cause it to create
            //    its reactive properties in the Flux manager.
            attackerComponent.EquipWeapon(evt.Weapon);
        }
        else
        {
            Debug.LogError($"Weapon prefab '{evt.Weapon.Prefab.name}' is missing the required PlayerWeaponAttackerComponent.", evt.Weapon.Prefab);
        }
    }

    /// <summary>
    /// Decommissions and pools the weapon in the specified slot.
    /// </summary>
    /// <param name="slotIndex">The index of the weapon to release.</param>
    private void ReleaseWeapon(int slotIndex)
    {
        GameObject weaponToRelease = activeWeapons[slotIndex];
        if (weaponToRelease == null) return;

        // Before pooling, we must tell the weapon's component to clean up its reactive properties.
        var attackerComponent = weaponToRelease.GetComponent<PlayerWeaponAttackerComponent>();
        if (attackerComponent != null)
        {
            attackerComponent.Decommission();
        }

        // Standard pooling logic begins here.
        var poolable = weaponToRelease.GetComponent<PoolableObject>();
        if (poolable == null)
        {
            Debug.LogWarning($"Weapon '{weaponToRelease.name}' is not poolable. Destroying it instead.");
            Destroy(weaponToRelease);
            activeWeapons[slotIndex] = null;
            return;
        }
        
        weaponToRelease.SetActive(false);
        weaponToRelease.transform.SetParent(poolParent);
        
        int prefabId = poolable.PrefabId;
        if (!pooledWeapons.ContainsKey(prefabId))
        {
            pooledWeapons[prefabId] = new List<GameObject>();
        }

        pooledWeapons[prefabId].Add(weaponToRelease);
        activeWeapons[slotIndex] = null; // Clear the active slot.

        Debug.Log($"WeaponInvoker: Released '{weaponToRelease.name}' to pool.");
    }

    /// <summary>
    /// Retrieves a weapon instance, reusing from a pool if possible or instantiating a new one.
    /// </summary>
    private GameObject GetWeapon(GameObject weaponPrefab, Transform spawnPoint)
    {
        int prefabId = weaponPrefab.GetInstanceID();
        
        if (pooledWeapons.TryGetValue(prefabId, out var weaponList) && weaponList.Count > 0)
        {
            GameObject weaponInstance = weaponList[weaponList.Count - 1];
            weaponList.RemoveAt(weaponList.Count - 1);

            weaponInstance.transform.SetParent(spawnPoint, false);
            weaponInstance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            weaponInstance.SetActive(true);
            
            return weaponInstance;
        }

        GameObject newWeapon = Instantiate(weaponPrefab, spawnPoint, false);
        newWeapon.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);

        var poolable = newWeapon.GetComponent<PoolableObject>() ?? newWeapon.AddComponent<PoolableObject>();
        poolable.SetPrefabId(prefabId);
        
        return newWeapon;
    }
}