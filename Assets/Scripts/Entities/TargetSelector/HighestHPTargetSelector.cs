using System.Collections.Generic;
using UnityEngine;
using FluxFramework.Core;

/// <summary>
/// A TargetSelector that selects the IHealthTarget with the most current health by querying the Flux global state.
/// </summary>
[CreateAssetMenu(fileName = "HighestHPTargetSelector", menuName = "Targeting/Highest HP Target Selector")]
public class HighestHPTargetSelector : TargetSelector
{
    public override IHealthTarget SelectTarget(List<IHealthTarget> possibleTargets, Vector3 origin)
    {
        if (possibleTargets == null || possibleTargets.Count == 0)
        {
            return null;
        }

        IHealthTarget highestHpTarget = null;
        float maxHealth = float.MinValue;

        var propertyManager = FluxFramework.Core.Flux.Manager.Properties;

        foreach (IHealthTarget target in possibleTargets)
        {
            if (target as Component == null || !(target as Component).gameObject.activeInHierarchy)
            {
                continue;
            }
            
            string key = target.HealthPropertyKey;
            if (string.IsNullOrEmpty(key)) continue;

            var healthProperty = propertyManager.GetProperty<float>(key);

            if (healthProperty != null)
            {
                float currentHealth = healthProperty.Value;
                if (currentHealth > maxHealth)
                {
                    maxHealth = currentHealth;
                    highestHpTarget = target;
                }
            }
        }

        return highestHpTarget;
    }
}