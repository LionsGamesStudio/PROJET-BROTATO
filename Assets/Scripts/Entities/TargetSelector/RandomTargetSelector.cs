using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// A TargetSelector that selects a random IHealthTarget from the list.
/// </summary>
[CreateAssetMenu(fileName = "RandomTargetSelector", menuName = "Targeting/Random Target Selector")]
public class RandomTargetSelector : TargetSelector
{
    public override IHealthTarget SelectTarget(List<IHealthTarget> possibleTargets, Vector3 origin)
    {
        if (possibleTargets == null || possibleTargets.Count == 0)
        {
            return null;
        }

        // It's robust to first filter the list for valid targets before picking one.
        var validTargets = new List<IHealthTarget>();
        foreach (var target in possibleTargets)
        {
            if (target as Component != null && (target as Component).gameObject.activeInHierarchy)
            {
                validTargets.Add(target);
            }
        }

        if (validTargets.Count == 0)
        {
            return null;
        }

        // Return a random target from the list of valid ones.
        return validTargets[Random.Range(0, validTargets.Count)];
    }
}