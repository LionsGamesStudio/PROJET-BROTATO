using System;
using System.Collections.Generic;
using FluxFramework.Attributes;
using FluxFramework.Core;
using UnityEngine;

public class PlayerController : FluxMonoBehaviour, IPlayer
{
    [Header("Player Stats")]
    [SerializeField]
    [ReactiveProperty("player.health")]
    [FluxRange(0f, 100f)]
    private float health = 100f;

    [SerializeField]
    [ReactiveProperty("player.maxHealth")]
    private float maxHealth = 100f;

    [SerializeField]
    [ReactiveProperty("player.damage")]
    private float damage = 10f;

    [SerializeField]
    [ReactiveProperty("player.attackSpeed")]
    private float attackSpeed = 1f;

    [SerializeField]
    [ReactiveProperty("player.movementSpeed")]
    private float movementSpeed = 5f;

    [SerializeField]
    [ReactiveProperty("player.armor")]
    private float armor = 0f;

    private HealthComponent _healthComponent;
    private BuffManager _buffManager;

    public string HealthPropertyKey => "player.health";
    public float MaxHealth => maxHealth;
    public AttackerType DamagerEntities => AttackerType.Monster;
    
    private int _id;
    
    public int ID
    {
        get
        {
            if (_id == 0)
            {
                _id = Guid.NewGuid().GetHashCode();
            }
            return _id;
        }
    }

    protected override void OnFluxAwake()
    {
        _healthComponent = GetComponent<HealthComponent>();
        _buffManager = GetComponent<BuffManager>();
    }

    public void OnHealthChanged(float oldValue, float newValue)
    {
        if (newValue < oldValue)
        {
            PublishEvent(new PlayerDamagedEvent(oldValue - newValue));
        }
    }

    public void OnDeath()
    {
        PublishEvent(new PlayerDeathEvent());
    }

    public void TakeDamage(float damage)
    {
        _healthComponent?.TakeDamage(damage);
    }

    public string GetStatPropertyKey(StatType statType)
    {
        return statType switch
        {
            StatType.Health => "player.health",
            StatType.MaxHealth => "player.maxHealth",
            StatType.Damage => "player.damage",
            StatType.AttackSpeed => "player.attackSpeed",
            StatType.MovementSpeed => "player.movementSpeed",
            StatType.Defense => "player.armor",
            _ => throw new ArgumentOutOfRangeException(nameof(statType), statType, null)
        };
    }
}