using System.Collections.Generic;
using System.Data;
using UnityEngine;
using UnityEngine.AI;

namespace Pathfinding.BehaviorTree
{
    public interface IBehaviorStrategy
    {
        Node.Status Process();
        void Reset() {
            // Noop
        }
    }
}

