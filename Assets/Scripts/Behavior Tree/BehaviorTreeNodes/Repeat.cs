using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.InputSystem.Interactions;

namespace Pathfinding.BehaviorTree
{
    // Repeat N times, don't care of true or false
    public class Repeat : Node
    {
        private int repeatCount;
        private int currentCount;
        public Repeat(string name, int count) : base(name)
        {
            repeatCount = count;
        }

        public override Status Process()
        {
            if (children[0].Process() != Status.Running)
            {
                children[0].Reset();
                currentCount++;

                if (currentCount < repeatCount)
                {
                    Reset();
                    return Status.Success;
                }

            }

            return Status.Running;
        }
    }

}
