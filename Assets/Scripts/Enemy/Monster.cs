using System;
using System.Collections;
using System.Collections.Generic;
using Pathfinding.BehaviorTree;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

/// <summary>
/// This function is to setup the monster
/// </summary>
public class Monster : MonoBehaviour
{

    [SerializeField]
    private SOMonster sOMonster;

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private GameObject player; // Temporary just for a test, or maybe not...

    private GameObject visualInstance;
    private BehaviorTree behaviorTree;

    // public float RadiusRange
    // { get => sOMonster ? sOMonster.RadiusRange : 0f; set { } }

    // public bool currentAttackEnable = false; // Will change
    // public bool AttackEnable { get => currentAttackEnable; set => currentAttackEnable = value; }

    // public float currentPv; // Pv will change
    // public float Pv { get => currentPv; set { currentPv = value; } }

    // private bool currentIsAttacking = false; // Will change
    // public bool IsAttacking { get => currentIsAttacking; set => currentIsAttacking = value; }

    // // Le restant des données du SOMonster
    // public int Damage => sOMonster ? sOMonster.Damage : 0;
    // public int MoneyValue => sOMonster ? sOMonster.MoneyValue : 0;
    // public float AttackSpeed => sOMonster ? sOMonster.AttackSpeed : 1f;
    // public float MovementSpeed => sOMonster ? sOMonster.MovementSpeed : 1f;
    // public int Shield => sOMonster ? sOMonster.Shield : 0;
    // public GameObject MonsterPrefab => sOMonster.MonsterPrefab;

    void Initialize(SOMonster so, BehaviorTree behaviorTree) // A utiliser dans mon spawner !
    {
        InitializeGameObject(so);
        InitializeBH(behaviorTree);
    }

    private void InitializeGameObject(SOMonster so)
    {
        sOMonster = so;

        if (sOMonster.MonsterPrefab != null)
        {
            visualInstance = Instantiate(sOMonster.MonsterPrefab, transform);
            visualInstance.transform.localPosition = Vector3.zero; // centre dans le GameObject
        }

        else {
            Debug.Log("Le SO ne comporte pas de monstre !");
        }
    }

    private void InitializeBH(BehaviorTree behaviorTree)
    {
        this.behaviorTree = behaviorTree;

        if (sOMonster == null)
        {
            Debug.Log("Le BH n'a pas été initialisé");
        }

    }

    void Update() // BH Update
    {
        behaviorTree.Process();
    }

}
