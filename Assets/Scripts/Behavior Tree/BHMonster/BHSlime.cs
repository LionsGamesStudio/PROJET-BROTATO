using System.Collections;
using Pathfinding.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using Events;

public class BHSlime : MonoBehaviour
{
    BehaviorTree tree;
    private float lastAttackTime = -999f; 

    public BehaviorTree CreateTree(SOMonster so, NavMeshAgent agent, Class_Perso player)
    {
        
    tree = new BehaviorTree("Slime");

    Selector root = new Selector("Root");
    tree.AddChild(root);

    // Attaque si possible
    Sequence attackSeq = new Sequence("Attack");
    attackSeq.AddChild(new Leaf("In Range", new Condition(() => 
        Vector3.Distance(transform.position, player.transform.position) < 0.5f)));
    attackSeq.AddChild(new Leaf("Not On Cooldown", new Condition(() => 
        Time.time >= lastAttackTime + 1 / so.AttackSpeed)));
    attackSeq.AddChild(new Leaf("Do Attack", new ActionStrategy(() => 
    {
        player.TakeDamage(so.Damage);
        lastAttackTime = Time.time;
    })));

    // Sinon, poursuit le joueur
    Leaf chase = new Leaf("Chase", new ActionStrategy(() => 
    {
        agent.SetDestination(player.transform.position);
    }));

    root.AddChild(attackSeq);
    root.AddChild(chase);

    return tree;
}
}