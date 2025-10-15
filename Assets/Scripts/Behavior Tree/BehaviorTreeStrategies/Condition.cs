using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;


namespace Pathfinding.BehaviorTree
{
    public class Condition : IBehaviorStrategy
    {
        readonly Func<bool> predicate; // --> Prend une fonction qui return un bool sans ent

        public Condition(Func<bool> predicate)
        {
            this.predicate = predicate;
        }
        public Node.Status Process() => predicate() ? Node.Status.Success : Node.Status.Failure;
    }
}