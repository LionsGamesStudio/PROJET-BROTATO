using FluxFramework.Core;
using UnityEngine;

public abstract class AttackBehavior : ScriptableObject
{
    /// <summary>
    /// Executes the specific logic of this attack behavior.
    /// </summary>
    /// <param name="healthTarget">The target that will be affected by this attack.</param>
    /// <param name="attacker">The IAttacker entity performing the attack.</param>
    /// <param name="attackerId">The unique ID of the attacker entity.</param>
    public abstract void Execute(IHealthTarget healthTarget, IAttacker attacker, int attackerId);
}