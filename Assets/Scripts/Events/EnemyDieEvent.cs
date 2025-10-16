using FluxFramework.Core;
using UnityEngine;


namespace Events
{
    public class EnemyDieEvent : FluxEventBase
    {
        public GameObject enemy;
        public EnemyDieEvent(GameObject enemy)
        {
            this.enemy = enemy;
        }
    }
}