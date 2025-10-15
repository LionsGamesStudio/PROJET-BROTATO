using FluxFramework.Core;
using FluxFramework.Extensions;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerWeaponAttackerComponent : BaseAttackerComponent, IBuffTarget
{
    // --- Instance-specific data ---
    private int _instanceId = -1;
    private string _damagePropertyKey;
    private string _attackSpeedPropertyKey;
    private string _rangePropertyKey;

    // --- Local storage for this weapon's own base stats ---
    private float _weaponBaseDamage = 0f;
    private float _weaponBaseAttackSpeed = 0f;
    private float _weaponBaseRange = 0f;

    // --- Subscriptions to the PLAYER'S global stats ---
    private List<IDisposable> _subscriptions = new List<IDisposable>();

    // --- Core Property Overrides ---
    public override float Damage => GetReactiveValue(_damagePropertyKey);
    public override float AttackSpeed => GetReactiveValue(_attackSpeedPropertyKey);
    public override float Range => GetReactiveValue(_rangePropertyKey);

    public void Initialize(int instanceId)
    {
        _instanceId = instanceId;
        _damagePropertyKey = $"weapon.{_instanceId}.damage";
        _attackSpeedPropertyKey = $"weapon.{_instanceId}.attackSpeed";
        _rangePropertyKey = $"weapon.{_instanceId}.range";

        // --- Subscribe to player's global stat changes ---
        // We use 'this' as the owner to leverage the extension methods.
        // The 'RecalculateFinalStats' method will now be called automatically whenever the player's base damage changes.
        var damageSub = this.SubscribeToProperty<float>("player.baseDamage", _ => RecalculateFinalStats(), fireOnSubscribe: false);
        var speedSub = this.SubscribeToProperty<float>("player.baseAttackSpeed", _ => RecalculateFinalStats(), fireOnSubscribe: false);
        
        _subscriptions.Add(damageSub);
        _subscriptions.Add(speedSub);
    }

    public void EquipWeapon(WeaponData weapon)
    {
        if (_instanceId < 0)
        {
            Debug.LogError($"Component not initialized on '{gameObject.name}'.", this);
            return;
        }

        if (weapon != null)
        {
            // Store this weapon's own base stats locally
            _weaponBaseDamage = weapon.Damage;
            _weaponBaseAttackSpeed = weapon.AttackSpeed;
            _weaponBaseRange = weapon.Range;
            targetSelector = weapon.TargetSelector;
            attackBehavior = weapon.WeaponBehavior;
        }
        else
        {
            // Reset if unequipping
            _weaponBaseDamage = 0f;
            _weaponBaseAttackSpeed = 0f;
            _weaponBaseRange = 0f;
        }

        // Trigger the first calculation
        RecalculateFinalStats();

        if (rangeTrigger != null)
        {
            rangeTrigger.radius = Range;
        }
        StopAttacking();
    }
    
    /// <summary>
    /// This is the core calculation logic. It combines the weapon's base stats
    /// with the player's global base stats to produce the final output.
    /// </summary>
    private void RecalculateFinalStats()
    {
        // 1. Get the player's current global stats
        float playerDamageBonus = this.GetReactivePropertyValue<float>("player.baseDamage");
        float playerAttackSpeedBonus = this.GetReactivePropertyValue<float>("player.baseAttackSpeed");
        // Add other bonuses for range, etc., if needed.

        // 2. Apply the formula (here, a simple addition)
        float finalDamage = _weaponBaseDamage + playerDamageBonus;
        float finalAttackSpeed = _weaponBaseAttackSpeed + playerAttackSpeedBonus;
        float finalRange = _weaponBaseRange; // Assuming range isn't buffed by player stats for now

        // 3. Create or update this weapon's unique reactive properties with the calculated final values.
        FluxFramework.Core.Flux.Manager.Properties.GetOrCreateProperty(_damagePropertyKey, finalDamage);
        FluxFramework.Core.Flux.Manager.Properties.GetOrCreateProperty(_attackSpeedPropertyKey, finalAttackSpeed);
        FluxFramework.Core.Flux.Manager.Properties.GetOrCreateProperty(_rangePropertyKey, finalRange);

        Debug.Log($"Recalculated stats for weapon [{_instanceId}]: Final Damage = {finalDamage} (Weapon: {_weaponBaseDamage} + Player: {playerDamageBonus})");
    }

    public void Decommission()
    {
        if (_instanceId < 0) return;

        // --- CRITICAL: Unsubscribe from all player stat events to prevent memory leaks ---
        foreach (var sub in _subscriptions)
        {
            sub?.Dispose();
        }
        _subscriptions.Clear();

        // Remove this weapon's properties from the manager
        FluxFramework.Core.Flux.Manager.Properties.UnregisterProperty(_damagePropertyKey);
        FluxFramework.Core.Flux.Manager.Properties.UnregisterProperty(_attackSpeedPropertyKey);
        FluxFramework.Core.Flux.Manager.Properties.UnregisterProperty(_rangePropertyKey);

        Debug.Log($"Decommissioned weapon [{_instanceId}].");
        _instanceId = -1;
    }

    /// <summary>
    /// Safely retrieves the current value of a reactive property from the manager.
    /// </summary>
    /// <param name="key">The unique key of the property to retrieve.</param>
    /// <returns>The property's value, or 0f if it doesn't exist.</returns>
    private float GetReactiveValue(string key)
    {
        if (string.IsNullOrEmpty(key)) return 0f;
        var property = FluxFramework.Core.Flux.Manager.Properties.GetProperty<float>(key);
        return property != null ? property.Value : 0f;
    }

    #region IBuffTarget Implementation

    /// <summary>
    /// Allows the BuffManager to find the correct reactive property key for this specific weapon instance.
    /// </summary>
    public string GetStatPropertyKey(StatType statType)
    {
        switch (statType)
        {
            case StatType.Damage: return _damagePropertyKey;
            case StatType.AttackSpeed: return _attackSpeedPropertyKey;
            case StatType.Range: return _rangePropertyKey;
            default: return string.Empty; // This component doesn't handle other stat types.
        }
    }

    #endregion
}