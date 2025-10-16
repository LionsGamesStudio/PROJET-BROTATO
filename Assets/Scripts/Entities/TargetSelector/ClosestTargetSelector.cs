using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A concrete implementation of TargetSelector that selects the closest IHealthTarget
/// from a list of possible targets.
/// </summary>
[CreateAssetMenu(fileName = "ClosestTargetSelector", menuName = "Targeting/Closest Target Selector")]
public class ClosestTargetSelector : TargetSelector
{
    /// <summary>
    /// Selects the closest IHealthTarget from the provided list of possible targets
    /// relative to a given origin point (which would be the attacker's position).
    /// </summary>
    /// <param name="possibleTargets">A list of potential targets in range.</param>
    /// <param name="origin">The position from which to measure distance (e.g., the attacker's position).</param>
    /// <returns>The closest IHealthTarget, or null if no valid targets are found.</returns>
    public override IHealthTarget SelectTarget(List<IHealthTarget> possibleTargets, Vector3 origin)
    {
        if (possibleTargets == null || possibleTargets.Count == 0)
        {
            return null;
        }

        IHealthTarget closestTarget = null;
        float minDistance = float.MaxValue;

        foreach (IHealthTarget target in possibleTargets)
        {
            // Ensure the target is a valid Unity GameObject component before proceeding.
            // This handles cases where IHealthTarget might be on a destroyed GameObject.
            Component targetComponent = target as Component;
            if (targetComponent == null || !targetComponent.gameObject.activeInHierarchy)
            {
                continue; // Skip invalid or inactive targets.
            }

            float distance = Vector3.Distance(origin, targetComponent.transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestTarget = target;
            }
        }

        return closestTarget;
    }
}