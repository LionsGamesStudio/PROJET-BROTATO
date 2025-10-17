using System;
using FluxFramework.Core;
using FluxFramework.Extensions;
using UnityEngine;

public class HealthComponent : FluxMonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    
    private IHealthTarget _target;
    private IDisposable _healthSubscription;

    public float MaxHealth => maxHealth;
    public string HealthPropertyKey => _target.HealthPropertyKey;

    protected override void OnFluxAwake()
    {
        _target = GetComponent<IHealthTarget>();
        if (_target == null)
        {
            Debug.LogError($"HealthComponent requires a component implementing IHealthTarget on {gameObject.name}");
            return;
        }

        // Initialize the health property in the Flux property manager if it doesn't exist.
        FluxFramework.Core.Flux.Manager.Properties.GetOrCreateProperty<float>(HealthPropertyKey, maxHealth);

        // Subscribe to health changes
        _healthSubscription = this.SubscribeToProperty<float>(HealthPropertyKey, OnHealthChanged, fireOnSubscribe: true);
    }

    protected override void OnFluxDestroy()
    {
        _healthSubscription?.Dispose();
    }

    /// <summary>
    /// Called when the health property changes.
    /// </summary>
    /// <param name="oldValue"></param>
    /// <param name="newValue"></param>
    private void OnHealthChanged(float oldValue, float newValue)
    {
        if (oldValue == newValue) return;
        
        _target?.OnHealthChanged(oldValue, newValue);

        if (newValue <= 0f && oldValue > 0f)
        {
            _target?.OnDeath();
        }
    }

    /// <summary>
    /// Apply damage to the entity.
    /// </summary>
    /// <param name="damage"></param>
    public void TakeDamage(float damage)
    {
        if (damage <= 0f) return;

        this.UpdateReactiveProperty<float>(HealthPropertyKey, currentHealth =>
            Mathf.Max(0f, currentHealth - damage));
    }

    /// <summary>
    /// Heal the entity.
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(float amount)
    {
        if (amount <= 0f) return;

        this.UpdateReactiveProperty<float>(HealthPropertyKey, currentHealth =>
            Mathf.Min(maxHealth, currentHealth + amount));
    }

    /// <summary>
    /// Get the current health value.
    /// </summary>
    /// <returns></returns>
    public float GetCurrentHealth()
    {
        return this.GetReactivePropertyValue<float>(HealthPropertyKey);
    }
}
