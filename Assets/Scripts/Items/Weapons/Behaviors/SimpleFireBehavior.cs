using UnityEngine;

/// <summary>
/// A concrete attack behavior that deals direct damage to a single target.
/// </summary>
[CreateAssetMenu(fileName = "SimpleFireBehavior", menuName = "Attack Behaviors/Simple Fire Behavior")]
public class SimpleFireBehavior : WeaponBehavior
{
    /// <summary>
    /// Executes the simple fire behavior: finds the target's HealthComponent and applies damage.
    /// </summary>
    /// <param name="healthTarget">The target to damage.</param>
    /// <param name="attacker">The attacker providing the damage amount.</param>
    public override void Execute(IHealthTarget healthTarget, IAttacker attacker)
    {
        // Ensure both the target and attacker are valid before proceeding.
        if (healthTarget == null || attacker == null)
        {
            Debug.LogWarning("SimpleFireBehavior: Execute called with a null target or attacker.");
            return;
        }

        // The IHealthTarget is an interface, so it must be on a Component.
        // We cast it to a Component to access its GameObject and other components.
        var targetComponent = healthTarget as Component;
        if (targetComponent == null)
        {
            Debug.LogError("SimpleFireBehavior: The provided IHealthTarget could not be cast to a Component.");
            return;
        }

        // Try to get the HealthComponent from the target.
        var targetHealthComponent = targetComponent.GetComponent<HealthComponent>();
        if (targetHealthComponent != null)
        {
            // Apply damage to the target using the attacker's final damage value.
            float damageToDeal = attacker.Damage;
            targetHealthComponent.TakeDamage(damageToDeal);

            // Debug log to confirm the attack happened. This is very useful for testing.
            Debug.Log($"{attacker.ID} attacked {targetHealthComponent.name} for {damageToDeal} damage.");
        }
        else
        {
            Debug.LogWarning($"SimpleFireBehavior: Target {targetComponent.name} has an IHealthTarget interface but no HealthComponent to take damage from.");
        }
    }
}