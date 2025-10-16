using UnityEngine;
using UnityEngine.AI;
using Pathfinding.BehaviorTree;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "RobotLogic", menuName = "Behavior Tree Logic/Robot Logic")]
public class RobotLogic : BehaviorTreeLogic
{
    public override BehaviorTree CreateTree(SOMonster monsterData, NavMeshAgent agent, Transform aiTransform, BaseAttackerComponent attacker, Dictionary<string, object> blackboard)
    {        
        blackboard["currentTarget"] = null;
        blackboard["lastAttackTime"] = -999f;
        
        var animator = aiTransform.GetComponentInChildren<Animator>();

        var tree = new BehaviorTree("Robot");
        var root = new Selector("Root");
        tree.AddChild(root);

        var attackSeq = new Sequence("Attack Sequence");
        attackSeq.AddChild(new Leaf("Has Target", new Condition(() => blackboard["currentTarget"] != null)));
        attackSeq.AddChild(new Leaf("In Range", new Condition(() =>
        {
            var target = (IHealthTarget)blackboard["currentTarget"];
            var targetTransform = (target as Component).transform;
            return Vector3.Distance(aiTransform.position, targetTransform.position) < attacker.Range;
        })));
        attackSeq.AddChild(new Leaf("Not On Cooldown", new Condition(() =>
            Time.time >= (float)blackboard["lastAttackTime"] + 1 / attacker.AttackSpeed)));
        attackSeq.AddChild(new Leaf("Do Attack", new ActionStrategy(() =>
        {
            var target = (IHealthTarget)blackboard["currentTarget"];
            animator?.SetTrigger("Attack");
            attacker.PerformAttack(target);
            blackboard["lastAttackTime"] = Time.time;
        })));

        var findTargetSeq = new Sequence("Find Target Sequence");
        findTargetSeq.AddChild(new Leaf("No Target", new Condition(() => blackboard["currentTarget"] == null)));
        findTargetSeq.AddChild(new Leaf("Select Target", new ActionStrategy(() =>
        {
            var selectedTarget = attacker.TargetSelector.SelectTarget(new List<IHealthTarget>(attacker.DetectedTargets), aiTransform.position);
            if (selectedTarget != null)
            {
                blackboard["currentTarget"] = selectedTarget;
            }
        })));

        var chaseSeq = new Sequence("Chase Sequence");
        chaseSeq.AddChild(new Leaf("Has Target", new Condition(() => blackboard["currentTarget"] != null)));
        chaseSeq.AddChild(new Leaf("Chase", new ActionStrategy(() =>
        {
            if (agent.isOnNavMesh)
            {
                var target = (IHealthTarget)blackboard["currentTarget"];
                var targetTransform = (target as Component).transform;
                agent.SetDestination(targetTransform.position);
            }
        })));

        root.AddChild(attackSeq);
        root.AddChild(findTargetSeq);
        root.AddChild(chaseSeq);

        return tree;
    }
}