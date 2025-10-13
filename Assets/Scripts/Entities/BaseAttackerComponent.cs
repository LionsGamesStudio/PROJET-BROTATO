using System.Collections;
using System.Collections.Generic;
using FluxFramework.Core;
using UnityEngine;

/// <summary>
/// Base class for all attacking entities.
/// It handles common logic like target detection, selection, and the attack cycle.
/// Concrete attacker types (e.g., Monster, Player) will inherit from this class.
/// </summary>
[RequireComponent(typeof(SphereCollider))]
public abstract class BaseAttackerComponent : FluxMonoBehaviour, IAttacker
{
    [Header("Attacker Configuration (Base)")]
    [SerializeField] protected AttackerType attackerType = AttackerType.None; // Default to None, overridden by concrete classes
    [SerializeField] protected TargetSelector targetSelector; // The strategy to select targets.
    [SerializeField] protected AttackBehavior attackBehavior; // The behavior to execute when attacking.

    [Header("Targeting Settings")]
    [SerializeField] protected LayerMask targetLayers;

    // This SphereCollider will be used to detect potential targets within range.
    protected SphereCollider rangeTrigger; 
    
    // List of IHealthTarget components currently within the attack range trigger.
    protected List<IHealthTarget> detectedTargets = new List<IHealthTarget>();

    // Coroutine reference to stop attacking if target leaves range or dies.
    protected Coroutine attackCoroutine;

    // Timer for attack cooldown.
    protected float nextAttackTime;

    // IAttacker Interface Properties - concrete classes will define Damage, AttackSpeed, Range
    public bool AttackEnable { get; set; } = true;
    public abstract float Damage { get; }
    public abstract float AttackSpeed { get; }
    public abstract float Range { get; }
    public TargetSelector TargetSelector => targetSelector;
    public AttackerType AttackerType => attackerType;
    public AttackBehavior AttackBehavior => attackBehavior;

    // ID
    public int ID => GetInstanceID();

    protected override void OnFluxAwake()
    {
        // Get or add the SphereCollider for range detection.
        rangeTrigger = GetComponent<SphereCollider>();
        rangeTrigger.isTrigger = true; // Ensure it's a trigger for detection.
        // Set the collider's radius.
        rangeTrigger.radius = Range; 

        // TODO: Configure the collider's layer to optimize detection.
    }

    protected void Update()
    {
        // Do not attack if disabled, no selector, or no attack behavior.
        if (!AttackEnable || TargetSelector == null || AttackBehavior == null)
        {
            return;
        }

        // Clean up detected targets: remove null references or inactive GameObjects.
        detectedTargets.RemoveAll(target => target == null || (target as Component) == null || !(target as Component).gameObject.activeInHierarchy);

        // If there are no targets, stop attacking.
        if (detectedTargets.Count == 0)
        {
            StopAttacking();
            return;
        }

        // Select the current target using the assigned TargetSelector and attacker's position.
        IHealthTarget currentTarget = TargetSelector.SelectTarget(detectedTargets, transform.position);

        // If a target is selected, and attack cooldown has passed, initiate an attack.
        if (currentTarget != null && Time.time >= nextAttackTime)
        {
            // Start the attack coroutine if not already running for this attack cycle.
            // Note: If you want to allow concurrent attacks (e.g., multiple projectiles),
            // you might need to adjust this logic to allow multiple coroutines.
            if (attackCoroutine == null)
            {
                attackCoroutine = StartCoroutine(Attack(currentTarget)); // Call the abstract Attack method
            }
            
            // Set next attack time, regardless of whether a new coroutine was started,
            // as this dictates the attack rate.
            nextAttackTime = Time.time + (1f / AttackSpeed); 
        }
    }

    /// <summary>
    /// Concrete implementations of IAttacker will provide their specific attack logic here.
    /// This method will be called by the update loop when an attack is due.
    /// </summary>
    /// <param name="healthTarget">The target of the attack.</param>
    public virtual IEnumerator Attack(IHealthTarget healthTarget)
    {
        // Execute the weapon/attacker's specific attack behavior.
        // The AttackBehavior will handle the actual damage application etc.
        AttackBehavior.Execute(healthTarget, this); 
        
        // Yield for a short period. This allows the Unity main thread to continue.
        yield return null; 
        
        // Reset attack coroutine reference after execution, allowing the next attack cycle to start.
        attackCoroutine = null;
    }

    /// <summary>
    /// Stops the current attack coroutine and resets attack state.
    /// </summary>
    protected void StopAttacking()
    {
        if (attackCoroutine != null)
        {
            StopCoroutine(attackCoroutine);
            attackCoroutine = null;
        }
    }

    /// <summary>
    /// Called when another collider enters the trigger zone.
    /// </summary>
    /// <param name="other">The collider that entered the trigger.</param>
    void OnTriggerEnter(Collider other)
    {
        IHealthTarget newTarget = other.GetComponent<IHealthTarget>();

        // Check if it's a valid health target and not the attacker itself.
        if (newTarget != null && newTarget.ID != this.ID)
        {
            // Check if the attacker is meant to damage this type of entity.
            bool canDamage = (AttackerType & newTarget.DamagerEntities) != 0;

            if (canDamage && !detectedTargets.Contains(newTarget))
            {
                detectedTargets.Add(newTarget);
                // Debug.Log($"BaseAttackerComponent: Detected potential target: {other.name} for {gameObject.name}");
            }
        }
    }

    /// <summary>
    /// Called when another collider exits the trigger zone.
    /// </summary>
    /// <param name="other">The collider that exited the trigger.</param>
    void OnTriggerExit(Collider other)
    {
        IHealthTarget exitingTarget = other.GetComponent<IHealthTarget>();

        // If the exiting collider was a detected target, remove it from the list.
        if (exitingTarget != null && detectedTargets.Contains(exitingTarget))
        {
            detectedTargets.Remove(exitingTarget);
            // Debug.Log($"BaseAttackerComponent: Target exited range: {other.name} from {gameObject.name}");
        }
    }
}