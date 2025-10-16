using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A TargetSelector that selects the farthest IHealthTarget from a list.
/// </summary>
[CreateAssetMenu(fileName = "FarthestTargetSelector", menuName = "Targeting/Farthest Target Selector")]
public class FarthestTargetSelector : TargetSelector
{
    public override IHealthTarget SelectTarget(List<IHealthTarget> possibleTargets, Vector3 origin)
    {
        if (possibleTargets == null || possibleTargets.Count == 0)
        {
            return null;
        }

        IHealthTarget farthestTarget = null;
        float maxDistance = float.MinValue;

        foreach (IHealthTarget target in possibleTargets)
        {
            Component targetComponent = target as Component;
            if (targetComponent == null || !targetComponent.gameObject.activeInHierarchy)
            {
                continue; // Skip invalid or inactive targets.
            }

            float distance = Vector3.Distance(origin, targetComponent.transform.position);

            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestTarget = target;
            }
        }

        return farthestTarget;
    }
}