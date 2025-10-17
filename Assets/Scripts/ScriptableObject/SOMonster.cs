using System.Collections.Generic;
using UnityEngine;
// Il n'est pas nécessaire d'importer le namespace du BehaviorTree ici,
// car nous ne faisons référence qu'aux classes de base AttackBehavior et TargetSelector.

[CreateAssetMenu(fileName = "NewMonster", menuName = "ScriptableObject/Monster")]
public class SOMonster : ScriptableObject
{
    [Header("Combat Stats")]
    [SerializeField]
    private int damage = 10;

    [SerializeField]
    private float attackSpeed = 1f;

    [SerializeField]
    private float health = 10f;

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

    [SerializeField]
    private float radiusRange = 1f;

    [Space(10)]
    [Header("Visuals")]
    [SerializeField]
    private GameObject monsterPrefab;

    [SerializeField]
    private LootTable lootTable;

    [Space(10)]
    [Header("AI & Combat Logic")]
    [Tooltip("Defines HOW this monster attacks (e.g., direct damage, projectile, AoE).")]
    [SerializeField]
    private AttackBehavior attackBehavior;

    [Tooltip("Defines WHICH TARGET this monster selects (e.g., closest, weakest).")]
    [SerializeField]
    private TargetSelector targetSelector;


    public int Damage => damage;
    public int MoneyValue => moneyValue;
    public float AttackSpeed => attackSpeed;
    public float MovementSpeed => movementSpeed;
    public int Shield => shield;
    public float RadiusRange => radiusRange;
    public float Health => health;
    public GameObject MonsterPrefab => monsterPrefab;
    public LootTable LootTable => lootTable;

    public AttackBehavior AttackBehavior => attackBehavior;
    public TargetSelector TargetSelector => targetSelector;
}