using System.Collections.Generic;
using FluxFramework.Attributes;
using FluxFramework.Core;
using UnityEngine;

public class WeaponInvoker : FluxMonoBehaviour
{
    [Tooltip("List of spawn points where weapons will be instantiated or attached.")]
    public List<Transform> spawnPoints = new List<Transform>();

    [Tooltip("The component responsible for handling player weapon attacks.")]
    [SerializeField] private PlayerWeaponAttackerComponent playerAttackerComponent;

    private List<GameObject> activeWeapons = new List<GameObject>();
    private Dictionary<int, List<GameObject>> pooledWeapons = new Dictionary<int, List<GameObject>>();
    private Transform poolParent;

    protected override void OnFluxStart()
    {
        // --- Dependency Validation ---
        // Check if the required component has been assigned in the inspector.
        if (playerAttackerComponent == null)
        {
            Debug.LogError($"WeaponInvoker on '{gameObject.name}' is missing its dependency: 'playerAttackerComponent'. Please assign it in the inspector. Disabling component.", this);
            this.enabled = false; // Disable the component to prevent further errors.
            return;
        }

        poolParent = new GameObject("WeaponPool").transform;
        poolParent.SetParent(this.transform);

        for (int i = 0; i < spawnPoints.Count; i++)
        {
            activeWeapons.Add(null); 
        }

        FluxFramework.Core.Flux.Manager.EventBus.Subscribe<WeaponEquippedEvent>(OnWeaponEquipped);
    }

    private void OnWeaponEquipped(WeaponEquippedEvent evt)
    {
        if (evt.SlotIndex < 0 || evt.SlotIndex >= spawnPoints.Count)
        {
            Debug.LogError($"WeaponInvoker: Invalid SlotIndex {evt.SlotIndex}.");
            return;
        }

        if (activeWeapons[evt.SlotIndex] != null)
        {
            ReleaseWeapon(evt.SlotIndex);
        }

        GameObject newWeaponInstance = GetWeapon(evt.Weapon.Prefab, spawnPoints[evt.SlotIndex]);
        activeWeapons[evt.SlotIndex] = newWeaponInstance;

        if (playerAttackerComponent != null)
        {
            playerAttackerComponent.EquipWeapon(evt.Weapon);
        }
    }

    private GameObject GetWeapon(GameObject weaponPrefab, Transform spawnPoint)
    {
        int prefabId = weaponPrefab.GetInstanceID();
        
        if (pooledWeapons.TryGetValue(prefabId, out var weaponList) && weaponList.Count > 0)
        {
            GameObject weaponInstance = weaponList[weaponList.Count - 1];
            weaponList.RemoveAt(weaponList.Count - 1);

            weaponInstance.transform.SetParent(spawnPoint, false); // Attach to spawn point
            weaponInstance.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            weaponInstance.SetActive(true);

            Debug.Log($"WeaponInvoker: Reusing '{weaponPrefab.name}' from pool.");
            return weaponInstance;
        }

        Debug.Log($"WeaponInvoker: Instantiating new '{weaponPrefab.name}'.");
        GameObject newWeapon = Instantiate(weaponPrefab, spawnPoint.position, spawnPoint.rotation);
        newWeapon.transform.SetParent(spawnPoint, true); // Attach to spawn point

        // Add and configure the PoolableObject component on the new instance.
        var poolable = newWeapon.GetComponent<PoolableObject>();
        if (poolable == null)
        {
            Debug.LogWarning($"Weapon prefab '{weaponPrefab.name}' is missing the PoolableObject component. Adding it at runtime.");
            poolable = newWeapon.AddComponent<PoolableObject>();
        }
        poolable.SetPrefabId(prefabId);
        
        return newWeapon;
    }

    private void ReleaseWeapon(int slotIndex)
    {
        GameObject weaponToRelease = activeWeapons[slotIndex];
        if (weaponToRelease == null) return;

        var poolable = weaponToRelease.GetComponent<PoolableObject>();
        if (poolable == null)
        {
            // If the object isn't poolable, just destroy it.
            Debug.LogWarning($"Weapon '{weaponToRelease.name}' is not poolable. Destroying it instead.");
            Destroy(weaponToRelease);
            return;
        }

        // Deactivate and move the object to the pool parent.
        weaponToRelease.SetActive(false);
        weaponToRelease.transform.SetParent(poolParent);
        
        int prefabId = poolable.PrefabId;

        // Ensure a list exists for this prefab ID in the pool dictionary.
        if (!pooledWeapons.ContainsKey(prefabId))
        {
            pooledWeapons[prefabId] = new List<GameObject>();
        }

        // Add the object to the pool.
        pooledWeapons[prefabId].Add(weaponToRelease);
        activeWeapons[slotIndex] = null; // Clear the active slot.

        Debug.Log($"WeaponInvoker: Released '{weaponToRelease.name}' to pool.");
    }
}