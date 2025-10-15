using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.BehaviorTree
{
    public class Parallel : Node
    { 
        public Parallel(string name, int priority = 0) : base(name, priority) { }

        public override Status Process()
        {
            bool anyRunning = false;

            for (int i = 0; i < children.Count; i++)
            {
                if (children[i].Process() == Status.Running)
                {
                    anyRunning = true; // bool for turn all nodes and after put running.
                }
            }

            if (anyRunning)
                return Status.Running;

            Reset();
            return Status.Success;
        }
    }
    
}
