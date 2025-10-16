using UnityEngine;
using UnityEngine.AI;
using System.Collections.Generic;

using BT = Pathfinding.BehaviorTree;

public abstract class BehaviorTreeLogic : ScriptableObject
{

    public abstract BT.BehaviorTree CreateTree(
        SOMonster monsterData,
        NavMeshAgent agent,
        Transform aiTransform,
        BaseAttackerComponent attacker,
        Dictionary<string, object> blackboard);
}