using FluxFramework.Core;
using UnityEngine;

public class PlayerDamagedEvent : FluxEventBase
{
    public float Damage { get; }
    public PlayerDamagedEvent(float damage)
    {
        Damage = damage;
    }
}