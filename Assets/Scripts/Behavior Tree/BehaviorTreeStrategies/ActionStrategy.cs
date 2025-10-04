using System;
using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;


namespace Pathfinding.BehaviorTree
{
    public class ActionStrategy : IBehaviorStrategy
    {
        readonly Action doSomething; // --> Prend une fonction sans argument qui ne return rien

        public ActionStrategy(Action doSomething)
        {
            this.doSomething = doSomething;
        }

        public Node.Status Process()
        {
            doSomething();
            return Node.Status.Success;
        }
    }
}