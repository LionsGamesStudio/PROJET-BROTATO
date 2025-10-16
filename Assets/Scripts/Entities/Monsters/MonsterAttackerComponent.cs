using UnityEngine;

public class MonsterAttackerComponent : BaseAttackerComponent
{
    [Header("Monster Data")]
    [SerializeField] private SOMonster monsterData;

    public override float Damage => monsterData.Damage;
    public override float AttackSpeed => monsterData.AttackSpeed;
    public override float Range => monsterData.RadiusRange;

    protected override void OnFluxAwake()
    {
        if (monsterData == null)
        {
            Debug.LogError("MonsterAttackerComponent is missing a SOMonster data asset!", this);
            enabled = false;
            return;
        }

        // Set the attacker type.
        attackerType = AttackerType.Monster;
        targetSelector = monsterData.TargetSelector;
        attackBehavior = monsterData.AttackBehavior;
        
        // Call the base for common initialization.
        base.OnFluxAwake(); 
    }
}