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

    [Header("Player Base Offensive Stats")]
    [Tooltip("Player's innate base damage bonus, applied to all weapons.")]
    [ReactiveProperty("player.baseDamage")]
    public float baseDamage = 0f;

    [Tooltip("Player's innate attack speed bonus (can be additive or multiplicative).")]
    [ReactiveProperty("player.baseAttackSpeed")]
    public float baseAttackSpeed = 0f;

    [SerializeField]
    [ReactiveProperty("player.movementSpeed")]
    private float movementSpeed = 5f;

    [SerializeField]
    [ReactiveProperty("player.armor")]
    private float armor = 0f;

    private HealthComponent _healthComponent;
    private BuffManager _buffManager;

    public string HealthPropertyKey => "player.health";
    public float MaxHealth => maxHealth; // Still provides max health for IHealthTarget
    public AttackerType DamagerEntities => AttackerType.Monster; // Player is damaged by monsters

    protected override void OnFluxAwake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _buffManager = GetComponent<BuffManager>();
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

    /// <summary>
    /// Provides the BuffManager with the correct property key for a given global player stat.
    /// This is where buffs that affect the entire player should be directed.
    /// </summary>
    public string GetStatPropertyKey(StatType statType)
    {
        switch (statType)
        {
            // Player-specific, non-combat stats
            case StatType.Health: return "player.health";
            case StatType.MaxHealth: return "player.maxHealth";
            case StatType.MovementSpeed: return "player.movementSpeed";
            case StatType.Defense: return "player.armor";

            // Combat-related stats that affect the player directly
            case StatType.Damage: return "player.baseDamage";
            case StatType.AttackSpeed: return "player.baseAttackSpeed";
            
            // Note: Range is intentionally left out here, as we decided it's a weapon-specific stat.
            // If you wanted a global player range buff, you would add "player.baseRange" here.
            case StatType.Range:
            default:
                // Signal that this component does not handle other stat types.
                // The BuffManager would then know to check other IBuffTarget components (like the weapons themselves).
                return string.Empty;
        }
    }
}