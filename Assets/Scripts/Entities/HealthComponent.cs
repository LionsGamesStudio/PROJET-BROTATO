using System;
using FluxFramework.Core;
using UnityEngine;

public class HealthComponent : FluxMonoBehaviour
{
    [Header("Health Settings")]
    [SerializeField] private float maxHealth = 100f;
    [SerializeField] private string healthPropertyKey;
    
    private IHealthTarget _target;
    private IDisposable _healthSubscription;

    public float MaxHealth => maxHealth;
    public string HealthPropertyKey => healthPropertyKey;
    
    public int ID
    {
        get
        {
            if (ID == 0)
            {
                ID = Guid.NewGuid().GetHashCode();
            }
            return ID;
        }
        private set { ID = value; }
    }

    protected override void OnFluxAwake()
    {
        _target = GetComponent<IHealthTarget>();
        if (_target == null)
        {
            Debug.LogError($"HealthComponent requires a component implementing IHealthTarget on {gameObject.name}");
            return;
        }

        // Initialize health property
        FluxFramework.Core.Flux.Manager.Properties.GetOrCreateProperty(healthPropertyKey, maxHealth);

        // Subscribe to health changes
        _healthSubscription = SubscribeToProperty<float>(healthPropertyKey, OnHealthChanged, fireOnSubscribe: true);
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

        UpdateReactiveProperty<float>(healthPropertyKey, currentHealth =>
            Mathf.Max(0f, currentHealth - damage));
    }

    /// <summary>
    /// Heal the entity.
    /// </summary>
    /// <param name="amount"></param>
    public void Heal(float amount)
    {
        if (amount <= 0f) return;

        UpdateReactiveProperty<float>(healthPropertyKey, currentHealth =>
            Mathf.Min(maxHealth, currentHealth + amount));
    }

    /// <summary>
    /// Get the current health value.
    /// </summary>
    /// <returns></returns>
    public float GetCurrentHealth()
    {
        return GetReactivePropertyValue<float>(healthPropertyKey);
    }
}
