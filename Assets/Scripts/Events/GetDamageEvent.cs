using FluxFramework.Core;
using UnityEngine;

namespace Events
{
    public class GetDamageEvent : FluxEventBase
    {
        public int damage;
        public Vector3 fromDirection;

        public GetDamageEvent(int damage, Vector3 fromDirection)
        {
            this.damage = damage;
            this.fromDirection = fromDirection;
        }
    }
}