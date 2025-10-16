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

        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        if (sphereCollider != null)
        {
            sphereCollider.radius = Range;
        }
        else
        {
            Debug.LogWarning("MonsterAttackerComponent: No SphereCollider found on the GameObject. Please add one for proper range detection.");
        }
        
        // Call the base for common initialization.
        base.OnFluxAwake(); 
    }
}