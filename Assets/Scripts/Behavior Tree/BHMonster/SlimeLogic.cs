using UnityEngine;
using UnityEngine.AI;
using Pathfinding.BehaviorTree;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SlimeLogic", menuName = "Behavior Tree Logic/Slime Logic")]
public class SlimeLogic : BehaviorTreeLogic
{
    public override BehaviorTree CreateTree(SOMonster monsterData, NavMeshAgent agent, Transform aiTransform, BaseAttackerComponent attacker, Dictionary<string, object> blackboard)
    {
        // Blackboard initialization
        blackboard["currentTarget"] = null;
        blackboard["lastAttackTime"] = -999f;
        
        var animator = aiTransform.GetComponentInChildren<Animator>();

        var tree = new BehaviorTree("Slime");
        var root = new Selector("Root");
        tree.AddChild(root);

        // --- Attack Sequence ---
        var attackSeq = new Sequence("Attack Sequence");

        // Condition: Have a valid target
        attackSeq.AddChild(new Leaf("Has Target", new Condition(() => blackboard["currentTarget"] != null)));
        
        // Condition: Be in range of the target
        attackSeq.AddChild(new Leaf("In Range", new Condition(() =>
        {
            var target = (IHealthTarget)blackboard["currentTarget"];
            var targetTransform = (target as Component).transform;
            return Vector3.Distance(aiTransform.position, targetTransform.position) < attacker.Range;
        })));
        
        // Condition: Not be on cooldown
        attackSeq.AddChild(new Leaf("Not On Cooldown", new Condition(() =>
            Time.time >= (float)blackboard["lastAttackTime"] + 1 / attacker.AttackSpeed)));
            
        // Action: Attack
        attackSeq.AddChild(new Leaf("Do Attack", new ActionStrategy(() =>
        {
            var target = (IHealthTarget)blackboard["currentTarget"];
            animator?.SetTrigger("Attack");
            
            attacker.PerformAttack(target);
            
            blackboard["lastAttackTime"] = Time.time;
        })));

        // --- Find Target Sequence ---
        var findTargetSeq = new Sequence("Find Target Sequence");
        
        // Condition: DO NOT have a target
        findTargetSeq.AddChild(new Leaf("No Target", new Condition(() => blackboard["currentTarget"] == null)));
        
        // Action: Select a new target
        findTargetSeq.AddChild(new Leaf("Select Target", new ActionStrategy(() =>
        {
            // The AI uses its TargetSelector to choose from detected targets.
            var selectedTarget = attacker.TargetSelector.SelectTarget(new List<IHealthTarget>(attacker.DetectedTargets), aiTransform.position);
            if (selectedTarget != null)
            {
                blackboard["currentTarget"] = selectedTarget;
            }
        })));

        // --- Chase Behavior ---
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