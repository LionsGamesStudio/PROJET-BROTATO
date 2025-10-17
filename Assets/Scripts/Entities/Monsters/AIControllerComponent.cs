using UnityEngine;
using UnityEngine.AI;
using Pathfinding.BehaviorTree;
using FluxFramework.Core;
using System.Collections.Generic;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(MonsterAttackerComponent))]
public class AIControllerComponent : FluxMonoBehaviour
{
    [Header("AI Configuration")]
    [SerializeField] private SOMonster monsterData;
    [SerializeField] private BehaviorTreeLogic aiLogic;

    private NavMeshAgent agent;
    private BehaviorTree behaviorTreeInstance;
    private MonsterAttackerComponent attackerComponent; // NEW

    private Dictionary<string, object> blackboard = new Dictionary<string, object>();

    protected override void OnFluxAwake()
    {
        base.OnFluxAwake();
        agent = GetComponent<NavMeshAgent>();
        attackerComponent = GetComponent<MonsterAttackerComponent>(); // NEW

        if (monsterData == null || aiLogic == null)
        {
            Debug.LogError($"AIControllerComponent on {gameObject.name} is missing monsterData or aiLogic asset!", this);
            enabled = false;
            return;
        }

        agent.speed = monsterData.MovementSpeed;
    }

    protected override void OnFluxStart()
    {
        base.OnFluxStart();
        if (aiLogic != null)
        {
            behaviorTreeInstance = aiLogic.CreateTree(monsterData, agent, this.transform, attackerComponent, blackboard);
        }
    }

    private void Update()
    {
        behaviorTreeInstance?.Process();
    }
}