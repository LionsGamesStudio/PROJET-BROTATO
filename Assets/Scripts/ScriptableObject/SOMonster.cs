using System.Collections;
using System.Collections.Generic;
using Pathfinding.BehaviorTree;
using UnityEditor.XR.OpenXR.Features;
using UnityEngine;
using UnityEngine.AI;

[CreateAssetMenu(fileName = "NewMonster", menuName = "ScriptableObject/Monster")]
public class SOMonster : ScriptableObject
{
    // Start is called before the first frame update
    [Header("Combat Stats")]
    [SerializeField]
    private int damage = 10;

    [SerializeField]
    private float attackSpeed = 1f;

    [SerializeField]
    private float pv = 10f;

    [SerializeField]
    private int shield = 0;

    [Space(10)]
    [Header("Rewards")]
    [SerializeField]
    private int moneyValue = 1;

    [Space(10)]
    [Header("Movement")]
    [SerializeField]
    private float movementSpeed = 1f;

    // Pour le IEnemy
    [SerializeField]
    private float radiusRange = 1f;

    [Space(10)]
    [Header("Initial State")]


    [SerializeField]
    private GameObject monsterPrefab;

    public int Damage => damage;
    public int MoneyValue => moneyValue;
    public float AttackSpeed => attackSpeed;
    public float MovementSpeed => movementSpeed;
    public int Shield => shield;
    public float RadiusRange => radiusRange;
    public float Pv => pv;
    public GameObject MonsterPrefab => monsterPrefab;
}
