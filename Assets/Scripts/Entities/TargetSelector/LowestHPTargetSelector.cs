using System.Collections.Generic;
using UnityEngine;
using FluxFramework.Core;

/// <summary>
/// A TargetSelector that selects the IHealthTarget with the least current health by querying the Flux global state.
/// </summary>
[CreateAssetMenu(fileName = "LowestHPTargetSelector", menuName = "Targeting/Lowest HP Target Selector")]
public class LowestHPTargetSelector : TargetSelector
{
    public override IHealthTarget SelectTarget(List<IHealthTarget> possibleTargets, Vector3 origin)
    {
        if (possibleTargets == null || possibleTargets.Count == 0)
        {
            return null;
        }

        IHealthTarget lowestHpTarget = null;
        float minHealth = float.MaxValue;

        var propertyManager = FluxFramework.Core.Flux.Manager.Properties;

        foreach (IHealthTarget target in possibleTargets)
        {
            if (target as Component == null || !(target as Component).gameObject.activeInHierarchy)
            {
                continue;
            }

            // 1. Get the unique health property key for this target.
            string key = target.HealthPropertyKey;
            if (string.IsNullOrEmpty(key)) continue;

            // 2. Use the key to get the reactive property from the manager.
            var healthProperty = propertyManager.GetProperty<float>(key);

            // 3. Read the value and compare.
            if (healthProperty != null)
            {
                float currentHealth = healthProperty.Value;
                if (currentHealth < minHealth)
                {
                    minHealth = currentHealth;
                    lowestHpTarget = target;
                }
            }
        }

        return lowestHpTarget;
    }
}