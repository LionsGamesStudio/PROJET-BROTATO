using FluxFramework.Core;
using UnityEngine;

public class EnemyAttackEvent : FluxEventBase
{
    public Vector3 FromWorldPosition { get; }
    public EnemyAttackEvent(Vector3 fromWorldPosition)
    {
        FromWorldPosition = fromWorldPosition;
    }
}