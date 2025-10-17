using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract base class for all target selection strategies.
/// Concrete implementations will define specific selection logic (e.g., closest, lowest health).
/// As a ScriptableObject, instances can be created as assets.
/// </summary>
public abstract class TargetSelector : ScriptableObject
{
    /// <summary>
    /// Selects a target from a list of possible health targets based on specific criteria.
    /// </summary>
    /// <param name="possibleTargets">A list of potential targets in range.</param>
    /// <param name="origin">The position from which targeting calculations should be made.</param>
    /// <returns>The selected IHealthTarget, or null if no suitable target is found.</returns>
    public abstract IHealthTarget SelectTarget(List<IHealthTarget> possibleTargets, Vector3 origin);
}