using System.Collections;
using System.Collections.Generic;
using Pathfinding.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;

public class BHSlime : MonoBehaviour
{
    BehaviorTree tree;
    private float lastAttackTime = -999f; // A voir si je peux gérer d'une autre manière

    public BehaviorTree CreateTree(SOMonster so, NavMeshAgent agent, GameObject player)
    {
        tree = new BehaviorTree("Slime");



        Selector startingSelector = new Selector("Start");

        tree.AddChild(startingSelector);

        Leaf moveToPlayer = new Leaf("Move to player", new ActionStrategy(() => agent.SetDestination(player.transform.position)));
        Sequence attackSequence = new Sequence("Attack");

        startingSelector.AddChild(moveToPlayer);
        startingSelector.AddChild(attackSequence);


        Leaf playerInRange = new Leaf("Player in range ?", new Condition(() => Vector3.Distance(gameObject.transform.position, player.transform.position) < 0.5f)); 

        Leaf notOnCooldown = new Leaf("Not on Cooldown ?", new Condition(() => Time.time >= lastAttackTime + 1/so.AttackSpeed));

        Leaf attack = new Leaf("Attack", new ActionStrategy(() =>
        {
            // Logique d'attaque à implémenter
            lastAttackTime = Time.time;

        }));

        attackSequence.AddChild(playerInRange);
        attackSequence.AddChild(notOnCooldown);
        attackSequence.AddChild(attack);

        return tree;
    }
}
