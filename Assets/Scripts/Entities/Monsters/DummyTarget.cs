using FluxFramework.Core;
using UnityEngine;

/// <summary>
/// A simple component that implements IHealthTarget for testing purposes.
/// </summary>
[RequireComponent(typeof(Entity))]
[RequireComponent(typeof(HealthComponent))]
public class DummyTarget : FluxMonoBehaviour, IHealthTarget
{

    /// <summary>
    /// Generates a UNIQUE property key for each instance of the dummy,
    /// by using the ID from its Entity component. This is the crucial fix.
    /// </summary>
    public string HealthPropertyKey
    {
        get
        {
            // Get the Entity component and use its unique ID to create a unique key.
            Entity entity = GetComponent<Entity>();
            if (entity != null)
            {
                return $"dummy_target_{entity.ID}.health";
            }
            
            // Fallback in case the Entity component is not found (should not happen due to [RequireComponent])
            Debug.LogError("DummyTarget is missing its required Entity component!", this);
            return "dummy_target_error.health";
        }
    }

    /// <summary>
    /// Specifies that only Players can damage this dummy.
    /// </summary>
    public AttackerType DamagerEntities => AttackerType.Player; 

    private HealthComponent _cacheHealthComponent;

    /// <summary>
    /// Gets the MaxHealth value directly from the attached HealthComponent.
    /// </summary>
    public float MaxHealth
    {
        get
        {
            if (_cacheHealthComponent == null)
            {
                _cacheHealthComponent = GetComponent<HealthComponent>();
                if (_cacheHealthComponent == null)
                {
                    Debug.LogError("DummyTarget requires a HealthComponent!", this);
                    return 0f;
                }
            }
            return _cacheHealthComponent.MaxHealth;
        }
    }

    /// <summary>
    /// Callback for when health changes. Provides visual feedback in the console.
    /// </summary>
    public void OnHealthChanged(float oldValue, float newValue)
    {
        Debug.Log($"<color=orange>DummyTarget '{name}': Ouch! My health changed from {oldValue} to {newValue}.</color>");
    }

    /// <summary>
    /// Callback for when the dummy is defeated. Provides visual feedback.
    /// </summary>
    public void OnDeath()
    {
        Debug.Log($"<color=red>DummyTarget '{name}': I have been defeated!</color>");
        
        // Try to get a renderer to change its color for clear visual feedback in the scene.
        var renderer = GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material.color = Color.red;
        }
    }
}