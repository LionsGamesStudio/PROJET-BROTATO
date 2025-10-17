using UnityEngine;

/// <summary>
/// A concrete attack behavior that deals direct damage to a single target.
/// </summary>
[CreateAssetMenu(fileName = "SimpleFireBehavior", menuName = "Attack Behaviors/Simple Fire Behavior")]
public class SimpleFireBehavior : AttackBehavior
{
    /// <summary>
    /// Executes the simple fire behavior: finds the target's HealthComponent and applies damage.
    /// </summary>
    /// <param name="healthTarget">The target to damage.</param>
    /// <param name="attacker">The attacker providing the damage amount.</param>
    /// <param name="attackerId">The unique ID of the attacker entity.</param>
    public override void Execute(IHealthTarget healthTarget, IAttacker attacker, int attackerId)
    {
        if (healthTarget == null || attacker == null)
        {
            Debug.LogWarning("SimpleFireBehavior: Execute called with a null target or attacker.");
            return;
        }

        var targetComponent = healthTarget as Component;
        if (targetComponent == null)
        {
            Debug.LogError("SimpleFireBehavior: The provided IHealthTarget could not be cast to a Component.");
            return;
        }

        var targetHealthComponent = targetComponent.GetComponent<HealthComponent>();
        if (targetHealthComponent != null)
        {
            float currentHealth = FluxFramework.Core.Flux.Manager.Properties.GetOrCreateProperty<float>(targetHealthComponent.HealthPropertyKey);

            if(currentHealth <= 0)
            {
                return; // Target is already dead, no need to apply damage
            }

            float damageToDeal = attacker.Damage;

            if (currentHealth - damageToDeal < 0)
            {
                damageToDeal = currentHealth; // Prevent overhealing
            }

            
            targetHealthComponent?.TakeDamage(damageToDeal);

            // The log now uses the attackerId passed as a parameter.
            Debug.Log($"Attacker ID:{attackerId} attacked {targetComponent.name} for {damageToDeal} damage.");
        }
        else
        {
            Debug.LogWarning($"SimpleFireBehavior: Target {targetComponent.name} has an IHealthTarget interface but no HealthComponent to take damage from.");
        }
    }
}