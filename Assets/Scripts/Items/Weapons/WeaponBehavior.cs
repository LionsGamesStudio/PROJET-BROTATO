using FluxFramework.Core;
using UnityEngine;

/// <summary>
/// Abstract base class for all attack behaviors.
/// Concrete implementations of this class define what an attack actually does
/// (e.g., deals direct damage, fires a projectile, creates an area of effect).
/// </summary>
public abstract class WeaponBehavior : AttackBehavior
{
    /// <summary>
    /// Executes the specific logic of this attack behavior.
    /// </summary>
    /// <param name="healthTarget">The target that will be affected by this attack.</param>
    /// <param name="attacker">The IAttacker entity performing the attack, providing stats like damage.</param>
    public abstract override void Execute(IHealthTarget healthTarget, IAttacker attacker);
}