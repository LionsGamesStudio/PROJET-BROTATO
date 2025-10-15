using UnityEngine.AI;

namespace Pathfinding.BehaviorTree
{
    public interface IBehaviorTree
    {
        BehaviorTree CreateTree(SOMonster so, NavMeshAgent agent, Class_Perso player);
    }

}

