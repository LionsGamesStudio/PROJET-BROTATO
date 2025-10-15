using System.Collections;
using Pathfinding.BehaviorTree;
using UnityEngine;
using UnityEngine.AI;
using Events;


public class Monster : MonoBehaviour ,IEnemy
{

    [SerializeField]
    private SOMonster sOMonster;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Class_Perso player; // Temporary just for a test, or maybe not...

    private GameObject visualInstance;
    private BehaviorTree behaviorTree;

    public float RadiusRange
    { get => sOMonster ? sOMonster.RadiusRange : 0f; set { } }

    public bool currentAttackEnable = false; // Will change
    public bool AttackEnable { get => currentAttackEnable; set => currentAttackEnable = value; }

    public float currentPv; // Pv will change
    public float Pv { get => currentPv; set { currentPv = value; } }

    private bool currentIsAttacking = false; // Will change
    public bool IsAttacking { get => currentIsAttacking; set => currentIsAttacking = value; }

    // Le restant des données du SOMonster
    public int Damage => sOMonster ? sOMonster.Damage : 0;
    public int MoneyValue => sOMonster ? sOMonster.MoneyValue : 0;
    public float AttackSpeed => sOMonster ? sOMonster.AttackSpeed : 1f;
    public float MovementSpeed => sOMonster ? sOMonster.MovementSpeed : 1f;
    public int Shield => sOMonster ? sOMonster.Shield : 0;
    public GameObject MonsterPrefab => sOMonster.MonsterPrefab;


    public void Initialize()
    {
        InitializeGameObject();
        InitializeNMA();
        InitializeBH();

        // Debug
    }

    private void InitializeGameObject()
    {
        if (sOMonster.MonsterPrefab != null)
        {
            visualInstance = Instantiate(sOMonster.MonsterPrefab, transform);
            visualInstance.transform.localPosition = Vector3.zero; // center in the GameObject
        }

        else
        {
            Debug.Log("Le SO ne comporte pas de monstre !");
        }
    }

    private void InitializeBH()
    {
        behaviorTree = GetComponent<IBehaviorTree>().CreateTree(sOMonster, agent, player);

        if (sOMonster == null)
        {
            Debug.Log("Le BH n'a pas été initialisé");
        }

    }

    private void InitializeNMA()
    {
        agent.speed = sOMonster.MovementSpeed;
        agent.stoppingDistance = sOMonster.RadiusRange; 
    }

    void Update() // BH Update
    {
        behaviorTree?.Process();
    }

    public void Die()
    {
        Debug.Log("L'ennemi est mort");
        FluxFramework.Core.Flux.Manager.EventBus.Publish(new EnemyDieEvent(gameObject));

        player.money += MoneyValue; // Remake this later
        player.enemyKilled++;

        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        Pv -= damage;
        if (Pv <= 0)
        {
            Die();
        }
    }

    public void SetPlayer(GameObject newValue)
    {
        player = newValue.GetComponent<Class_Perso>();
    }


}
