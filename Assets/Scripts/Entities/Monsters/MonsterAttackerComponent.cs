using UnityEngine;

/// <summary>
/// Concrete attacker component for monsters, defining their inherent attack stats.
/// </summary>
public class MonsterAttackerComponent : BaseAttackerComponent
{
    [Header("Monster Attack Stats")]
    [SerializeField] private float monsterDamage = 10f;
    [SerializeField] private float monsterAttackSpeed = 1f;
    [SerializeField] private float monsterRange = 2f;
    [SerializeField] private AttackerType monsterAttackerType = AttackerType.Monster;

    // Implement abstract properties from BaseAttackerComponent
    public override float Damage => monsterDamage;
    public override float AttackSpeed => monsterAttackSpeed;
    public override float Range => monsterRange;

    protected override void OnFluxAwake()
    {
        // Set specific attacker type for monsters.
        attackerType = monsterAttackerType;
        base.OnFluxAwake(); // Call base initialization
    }
    
    // No need to override Attack method unless monster has very unique attack logic
    // Default BaseAttackerComponent.Attack will work using the assigned AttackBehavior.
}