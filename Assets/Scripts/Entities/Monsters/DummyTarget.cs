using UnityEngine;

/// <summary>
/// A simple component that implements IHealthTarget for testing purposes.
/// It allows an object to be recognized as a valid target by attacker components.
/// </summary>
public class DummyTarget : MonoBehaviour, IHealthTarget
{
    // We can use the GameObject's instance ID for a simple, unique runtime ID.
    public int ID => gameObject.GetInstanceID();

    // A dummy property key for the HealthComponent.
    public string HealthPropertyKey => "test_target.health";

    // Defines which types of attackers can damage this target.
    public AttackerType DamagerEntities => AttackerType.Player; 

    // This can be linked to the HealthComponent's maxHealth if needed, or just be a fixed value.
    public float MaxHealth => 100f; 

    public void OnHealthChanged(float oldValue, float newValue)
    {
        Debug.Log($"<color=orange>TestTarget '{name}': Ouch! My health changed from {oldValue} to {newValue}.</color>");
    }

    public void OnDeath()
    {
        Debug.Log($"<color=red>TestTarget '{name}': I have been defeated!</color>");
        // Optional: Make the dummy disappear or change color on death.
        // GetComponent<Renderer>().material.color = Color.red;
    }
}