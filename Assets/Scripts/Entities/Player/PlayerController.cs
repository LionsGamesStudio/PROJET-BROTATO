using System;
using System.Collections.Generic;
using FluxFramework.Attributes;
using FluxFramework.Core;
using FluxFramework.Extensions;
using UnityEngine;

[RequireComponent(typeof(Entity))]
public class PlayerController : FluxMonoBehaviour, IPlayer
{
    [Header("Player Stats")]
    [SerializeField]
    [ReactiveProperty("player.health")]
    [FluxRange(0f, 100f)]
    private float health = 100f; // This will likely become a read-only visual representation once HealthComponent manages it.

    [SerializeField]
    [ReactiveProperty("player.maxHealth")]
    private float maxHealth = 100f; // Managed by HealthComponent

    [SerializeField]
    [ReactiveProperty("player.movementSpeed")]
    private float movementSpeed = 5f;

    [SerializeField]
    [ReactiveProperty("player.armor")]
    private float armor = 0f;

    private HealthComponent _healthComponent;
    private BuffManager _buffManager;
    private PlayerWeaponAttackerComponent _playerAttackerComponent; // Reference to the new attacker component

    public string HealthPropertyKey => "player.health";
    public float MaxHealth => maxHealth; // Still provides max health for IHealthTarget
    public AttackerType DamagerEntities => AttackerType.Monster; // Player is damaged by monsters

    protected override void OnFluxAwake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _buffManager = GetComponent<BuffManager>();
        _playerAttackerComponent = GetComponent<PlayerWeaponAttackerComponent>(); // Get the new attacker component

        if (_playerAttackerComponent == null)
        {
            Debug.LogError("PlayerController: PlayerWeaponAttackerComponent not found on this GameObject!");
        }
    }

    public void OnHealthChanged(float oldValue, float newValue)
    {
        if (newValue < oldValue)
        {
            this.PublishEvent(new PlayerDamagedEvent(oldValue - newValue));
        }
    }

    public void OnDeath()
    {
        this.PublishEvent(new PlayerDeathEvent());
    }

    public void TakeDamage(float damage)
    {
        _healthComponent?.TakeDamage(damage);
    }

    public string GetStatPropertyKey(StatType statType)
    {
        switch (statType)
        {
            case StatType.Health: return "player.health";
            case StatType.MaxHealth: return "player.maxHealth";
            case StatType.MovementSpeed: return "player.movementSpeed";
            case StatType.Defense: return "player.armor";

            // These stats are now handled by PlayerWeaponAttackerComponent.
            // Return string.Empty to signal that this component does not handle them.
            case StatType.Damage:
            case StatType.AttackSpeed:
            case StatType.Range:
            default:
                return string.Empty;
        }
    }
}