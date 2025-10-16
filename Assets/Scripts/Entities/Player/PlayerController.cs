using FluxFramework.Core;
using FluxFramework.Attributes;
using FluxFramework.Extensions;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
[RequireComponent(typeof(BuffManager))]
[RequireComponent(typeof(Entity))]
public class PlayerController : FluxMonoBehaviour, IHealthTarget, IBuffTarget
{
    [Header("Player Base Stats")]
    [SerializeField]
    [ReactiveProperty("player.maxHealth")]
    private float maxHealth = 100f;

    [Tooltip("Player's innate base damage bonus, applied to all weapons.")]
    [ReactiveProperty("player.baseDamage")]
    public float baseDamage = 0f;

    [Tooltip("Player's innate attack speed bonus, applied to all weapons.")]
    [ReactiveProperty("player.baseAttackSpeed")]
    public float baseAttackSpeed = 0f;

    [SerializeField]
    [ReactiveProperty("player.movementSpeed")]
    private float movementSpeed = 5f;

    [SerializeField]
    [ReactiveProperty("player.armor")]
    private float armor = 0f;

    private HealthComponent _healthComponent;
    private Entity _entity;

    #region IHealthTarget Implementation

    public string HealthPropertyKey => $"player.health";
    public float MaxHealth => maxHealth;
    public AttackerType DamagerEntities => AttackerType.Monster; // The player is damaged by monsters.

    public void OnHealthChanged(float oldValue, float newValue)
    {
        // Publish an event if the player takes damage.
        if (newValue < oldValue)
        {
            this.PublishEvent(new PlayerDamagedEvent(oldValue - newValue));
        }
    }

    public void OnDeath()
    {
        // Publish an event upon the player's death.
        Debug.Log("Player has died.");
        this.PublishEvent(new PlayerDeathEvent());

        // TODO: Add death logic (defeat screen, etc.)
        GameStateManager.Instance.ReturnToMainMenu();
    }

    #endregion

    #region IBuffTarget Implementation

    /// <summary>
    /// Provides the BuffManager with the correct reactive property key for a global player stat.
    /// This is where buffs that affect the entire player should be directed.
    /// </summary>
    public string GetStatPropertyKey(StatType statType)
    {
        switch (statType)
        {
            // Global player stats
            case StatType.Health: return "player.health";
            case StatType.MaxHealth: return "player.maxHealth";
            case StatType.MovementSpeed: return "player.movementSpeed";
            case StatType.Defense: return "player.armor";

            // Combat stats that affect the player directly (base bonuses)
            case StatType.Damage: return "player.baseDamage";
            case StatType.AttackSpeed: return "player.baseAttackSpeed";
            
            // Note: Range is intentionally left out here, as it's a weapon-specific stat.
            // If the BuffManager requests a stat that this component doesn't handle, we return an empty string.
            // The BuffManager will then know to check other IBuffTarget components (like the weapons).
            case StatType.Range:
            default:
                return string.Empty;
        }
    }

    #endregion

    protected override void OnFluxAwake()
    {
        base.OnFluxAwake();
        _healthComponent = GetComponent<HealthComponent>();
        _entity = GetComponent<Entity>();

        if (_healthComponent == null)
        {
            Debug.LogError("PlayerController requires a HealthComponent.", this);
        }
        
        // Initialize the player's health property in the manager
        FluxFramework.Core.Flux.Manager.Properties.GetOrCreateProperty<float>(HealthPropertyKey, maxHealth);

        // Register the player's transform so AIs can find it
        FluxFramework.Core.Flux.Manager.Properties.GetOrCreateProperty<Transform>("player.transform", this.transform);
    }

    protected override void OnFluxDestroy()
    {
        // Clean up the player's transform reference if this object is destroyed
        var playerTransformProp = FluxFramework.Core.Flux.Manager.Properties.GetProperty<Transform>("player.transform");
        if (playerTransformProp != null && playerTransformProp.Value == this.transform)
        {
            playerTransformProp.Value = null;
        }
        base.OnFluxDestroy();
    }

    /// <summary>
    /// Public method to deal damage, delegating the call to the HealthComponent.
    /// </summary>
    public void TakeDamage(float damage)
    {
        _healthComponent?.TakeDamage(damage);
    }
}