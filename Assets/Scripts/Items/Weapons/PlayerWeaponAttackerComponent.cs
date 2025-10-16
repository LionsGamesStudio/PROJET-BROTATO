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

    // Reference to the player's central attack controller.
    private PlayerAttackController _attackController;

    // --- Core Property Overrides ---
    public override float Damage => GetReactiveValue(_damagePropertyKey);
    public override float AttackSpeed => GetReactiveValue(_attackSpeedPropertyKey);
    public override float Range => GetReactiveValue(_rangePropertyKey);


    public void Initialize(int instanceId)
    {
        _attackController = GetComponentInParent<PlayerAttackController>();
        _instanceId = instanceId;
        _damagePropertyKey = $"weapon.{_instanceId}.damage";
        _attackSpeedPropertyKey = $"weapon.{_instanceId}.attackSpeed";
        _rangePropertyKey = $"weapon.{_instanceId}.range";

        var damageSub = this.SubscribeToProperty<float>("player.baseDamage", _ => RecalculateFinalStats(), fireOnSubscribe: false);
        var speedSub = this.SubscribeToProperty<float>("player.baseAttackSpeed", _ => RecalculateFinalStats(), fireOnSubscribe: false);

        _subscriptions.Add(damageSub);
        _subscriptions.Add(speedSub);
    }

    public override void PerformAttack(IHealthTarget healthTarget)
    {
        base.PerformAttack(healthTarget);

        // Rotate the weapon to face the target
        if (healthTarget is Component targetComponent)
        {
            Vector3 directionToTarget = (targetComponent.transform.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(directionToTarget);
            transform.rotation = Quaternion.Euler(0, lookRotation.eulerAngles.y, 0);
        }
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
            _weaponBaseDamage = weapon.Damage;
            _weaponBaseAttackSpeed = weapon.AttackSpeed;
            _weaponBaseRange = weapon.Range;
            targetSelector = weapon.TargetSelector;
            attackBehavior = weapon.WeaponBehavior;
        }
        else
        {
            _weaponBaseDamage = 0f;
            _weaponBaseAttackSpeed = 0f;
            _weaponBaseRange = 0f;
        }

        RecalculateFinalStats();

        if (rangeTrigger != null)
        {
            rangeTrigger.radius = Range;
        }
        
        _attackController?.RegisterWeapon(this);
    }
    
    private void RecalculateFinalStats()
    {
        float playerDamageBonus = this.GetReactivePropertyValue<float>("player.baseDamage");
        float playerAttackSpeedBonus = this.GetReactivePropertyValue<float>("player.baseAttackSpeed");

        float finalDamage = _weaponBaseDamage + playerDamageBonus;
        float finalAttackSpeed = _weaponBaseAttackSpeed + playerAttackSpeedBonus;
        float finalRange = _weaponBaseRange; 

        FluxFramework.Core.Flux.Manager.Properties.GetOrCreateProperty(_damagePropertyKey, finalDamage);
        FluxFramework.Core.Flux.Manager.Properties.GetOrCreateProperty(_attackSpeedPropertyKey, finalAttackSpeed);
        FluxFramework.Core.Flux.Manager.Properties.GetOrCreateProperty(_rangePropertyKey, finalRange);
    }

    public void Decommission()
    {
        if (_instanceId < 0) return;
        
        _attackController?.UnregisterWeapon(this);

        foreach (var sub in _subscriptions)
        {
            sub?.Dispose();
        }
        _subscriptions.Clear();

        FluxFramework.Core.Flux.Manager.Properties.UnregisterProperty(_damagePropertyKey);
        FluxFramework.Core.Flux.Manager.Properties.UnregisterProperty(_attackSpeedPropertyKey);
        FluxFramework.Core.Flux.Manager.Properties.UnregisterProperty(_rangePropertyKey);

        Debug.Log($"Decommissioned weapon [{_instanceId}].");
        _instanceId = -1;
    }

    private float GetReactiveValue(string key)
    {
        if (string.IsNullOrEmpty(key)) return 0f;
        var property = FluxFramework.Core.Flux.Manager.Properties.GetProperty<float>(key);
        return property != null ? property.Value : 0f;
    }

    public string GetStatPropertyKey(StatType statType)
    {
        switch (statType)
        {
            case StatType.Damage: return _damagePropertyKey;
            case StatType.AttackSpeed: return _attackSpeedPropertyKey;
            case StatType.Range: return _rangePropertyKey;
            default: return string.Empty;
        }
    }
}