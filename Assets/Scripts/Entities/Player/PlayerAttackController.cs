using System.Collections.Generic;
using FluxFramework.Core;
using UnityEngine;

/// <summary>
/// The "brain" that controls the player's active weapons. It doesn't know about
/// the inventory, only about the weapon attackers that are currently active in the scene.
/// It commands registered weapons to find targets and attack.
/// </summary>
public class PlayerAttackController : FluxMonoBehaviour
{
    // A list of all currently active and ready-to-fire weapon attacker components.
    private readonly List<PlayerWeaponAttackerComponent> _activeWeapons = new List<PlayerWeaponAttackerComponent>();

    // A dictionary to store the last attack time for each weapon instance.
    private readonly Dictionary<int, float> _lastAttackTimes = new Dictionary<int, float>();

    /// <summary>
    /// Called by PlayerWeaponAttackerComponent instances when they are enabled.
    /// </summary>
    public void RegisterWeapon(PlayerWeaponAttackerComponent weapon)
    {
        if (!_activeWeapons.Contains(weapon))
        {
            _activeWeapons.Add(weapon);
            Debug.Log($"PlayerAttackController: Registered weapon '{weapon.name}'. Total active weapons: {_activeWeapons.Count}");
        }
    }

    /// <summary>
    /// Called by PlayerWeaponAttackerComponent instances when they are disabled.
    /// </summary>
    public void UnregisterWeapon(PlayerWeaponAttackerComponent weapon)
    {
        if (_activeWeapons.Contains(weapon))
        {
            _activeWeapons.Remove(weapon);
            Debug.Log($"PlayerAttackController: Unregistered weapon '{weapon.name}'. Total active weapons: {_activeWeapons.Count}");
        }
    }

    private void Update()
    {
        // Iterate over a copy in case the list is modified during the loop (less likely with this pattern but safer).
        foreach (var weapon in _activeWeapons)
        {
            // Basic checks to ensure the weapon is ready.
            if (weapon == null || !weapon.AttackEnable || weapon.DetectedTargets.Count == 0)
            {
                continue;
            }

            // Ensure the weapon has an entry in our cooldown dictionary.
            int weaponId = weapon.GetInstanceID();
            _lastAttackTimes.TryAdd(weaponId, -999f);

            // Check if the weapon is off cooldown.
            if (Time.time >= _lastAttackTimes[weaponId] + (1 / weapon.AttackSpeed))
            {
                // 1. Use the weapon's own TargetSelector to find the best target from its detected list.
                IHealthTarget target = weapon.TargetSelector.SelectTarget(
                    new List<IHealthTarget>(weapon.DetectedTargets), 
                    weapon.transform.position
                );

                // 2. If a valid target was selected, command the weapon to attack it.
                if (target != null)
                {
                    weapon.PerformAttack(target);
                    _lastAttackTimes[weaponId] = Time.time; // Reset the cooldown timer for this weapon.
                }
            }
        }
    }
}