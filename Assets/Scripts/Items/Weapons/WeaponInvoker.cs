using System.Collections.Generic;
using FluxFramework.Attributes;
using FluxFramework.Core;
using UnityEngine;

public class WeaponInvoker : FluxMonoBehaviour
{
    public List<Transform> spawnPoints = new List<Transform>();

    private List<GameObject> spawnedWeapons = new List<GameObject>();

    protected override void OnFluxStart()
    {
        FluxFramework.Core.Flux.Manager.EventBus.Subscribe<WeaponEquippedEvent>(SpawnWeapon);
    }


    public void SpawnWeapon(WeaponEquippedEvent evt)
    {
        // TODO : optimize by using object pooling
        if (spawnPoints.Count == 0)
        {
            Debug.LogWarning("No spawn points assigned for weapon spawning.");
            return;
        }

        Debug.Log($"Spawning weapon: {evt.Weapon.Name}");

        GameObject weaponPrefab = evt.Weapon.Prefab;

        int randomIndex = Random.Range(0, spawnPoints.Count);

        if (spawnedWeapons[randomIndex] != null)
        {
            GameObject weaponToRemove = spawnedWeapons[randomIndex];
            spawnedWeapons.RemoveAt(randomIndex);
            Destroy(weaponToRemove);

            // Add effect or sound here for weapon removal if needed
        }

        Transform spawnPoint = spawnPoints[randomIndex];
        GameObject newWeapon = Instantiate(weaponPrefab, spawnPoint.position, Quaternion.identity);
        spawnedWeapons.Add(newWeapon);

        AttackAreaDetection attackArea = newWeapon.GetComponent<AttackAreaDetection>();
        if (attackArea != null)
        {
            newWeapon.AddComponent<AttackAreaDetection>();
        }

        // Add effect or sound here for weapon spawn if needed

    }
}