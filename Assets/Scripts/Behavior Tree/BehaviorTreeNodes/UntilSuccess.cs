using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Pathfinding.BehaviorTree
{
    public class UntilSuccess : Node
    {
        public UntilSuccess(string name) : base(name) { }

        public override Status Process()
        {
            if (children[0].Process() == Status.Success)
            {
                Reset();
                return Status.Success;
            }

            return Status.Running;
        }
    }
}