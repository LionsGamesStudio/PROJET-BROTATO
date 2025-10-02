using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.BehaviorTree
{ 
    public class Leaf : Node
    {
        readonly IBehaviorStrategy strategy;

        public Leaf(string name, IBehaviorStrategy strategy, int priority = 0) : base(name, priority)
        {
            this.strategy = strategy;
        }

        public override Status Process() => strategy.Process();

        public override void Reset() => strategy.Reset();
    }
}