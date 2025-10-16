using System.Collections.Generic;
using FluxFramework.Core;
using UnityEngine;

/// <summary>
/// Provides the physical attack capabilities for an entity.
/// Its main roles are detecting targets and executing a specified AttackBehavior.
/// It does NOT decide when or who to attack; that is the role of an AI Controller.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
[RequireComponent(typeof(Entity))]
public abstract class BaseAttackerComponent : FluxMonoBehaviour, IAttacker
{
    [Header("Attacker Configuration")]
    [SerializeField] protected AttackerType attackerType = AttackerType.None;
    [SerializeField] protected AttackBehavior attackBehavior;
    [SerializeField] protected TargetSelector targetSelector;

    protected Entity entity;
    protected SphereCollider rangeTrigger; 
    
    protected List<IHealthTarget> detectedTargets = new List<IHealthTarget>();
    /// <summary>
    /// A read-only list of potential targets currently within the trigger range.
    /// The AI Brain will read from this list.
    /// </summary>
    public IReadOnlyList<IHealthTarget> DetectedTargets => detectedTargets;

    // IAttacker Interface Implementation
    public bool AttackEnable { get; set; } = true;
    public abstract float Damage { get; }
    public abstract float AttackSpeed { get; }
    public abstract float Range { get; }
    public TargetSelector TargetSelector => targetSelector;
    public AttackerType AttackerType => attackerType;
    public AttackBehavior AttackBehavior => attackBehavior;

    private bool isInitialized = false;
    
    protected override void OnFluxStart()
    {
        base.OnFluxStart();
        entity = GetComponent<Entity>();
        if (entity == null)
        {
            Debug.LogError($"BaseAttackerComponent requires an Entity component on {gameObject.name}", this);
            enabled = false;
            return;
        }
        rangeTrigger = GetComponent<SphereCollider>();
        rangeTrigger.isTrigger = true;
        isInitialized = true;
    }

    /// <summary>
    /// A public command that the AI Brain can call to execute an attack.
    /// </summary>
    public virtual void PerformAttack(IHealthTarget healthTarget)
    {
        if (!AttackEnable || healthTarget == null || attackBehavior == null) return;
        attackBehavior.Execute(healthTarget, this, this.entity.ID); 
    }

    // OnTriggerEnter and OnTriggerExit are the core of this component's responsibility.
    void OnTriggerEnter(Collider other)
    {
        if (!isInitialized) return;
        var targetEntity = other.GetComponent<Entity>();
        if (targetEntity == null || targetEntity.ID == this.entity.ID) return;

        IHealthTarget newTarget = other.GetComponent<IHealthTarget>();
        if (newTarget != null)
        {
            bool canDamage = (AttackerType & newTarget.DamagerEntities) != 0;
            if (canDamage && !detectedTargets.Contains(newTarget))
            {
                detectedTargets.Add(newTarget);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (!isInitialized) return;
        IHealthTarget exitingTarget = other.GetComponent<IHealthTarget>();
        if (exitingTarget != null && detectedTargets.Contains(exitingTarget))
        {
            detectedTargets.Remove(exitingTarget);
        }
    }
}