using FluxFramework.Core;
using UnityEngine;

public abstract class WeaponBehavior : FluxScriptableObject
{
    public abstract void Execute(IHealthTarget healthTarget, IWeapon weapon);
}