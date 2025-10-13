using FluxFramework.Core;
using UnityEngine;

public abstract class AttackBehavior : ScriptableObject
{
    public abstract void Execute(IHealthTarget healthTarget, IAttacker attacker);
}