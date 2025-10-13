using System.Collections;
using System.Collections.Generic; // Added for IReadOnlyList (potential future use for detected targets)
using UnityEngine; // Added for Vector3

public interface IAttacker : IEntity
{
    /// <summary>
    /// Gets or sets whether this attacker is currently enabled to attack.
    /// </summary>
    bool AttackEnable { get; set; }
    
    /// <summary>
    /// Gets the current damage output of the attacker.
    /// </summary>
    float Damage { get; }
    
    /// <summary>
    /// Gets the current attack speed (attacks per second) of the attacker.
    /// </summary>
    float AttackSpeed { get; }
    
    /// <summary>
    /// Gets the current attack range of the attacker.
    /// </summary>
    float Range { get; }
    
    /// <summary>
    /// Gets the TargetSelector strategy used by this attacker.
    /// </summary>
    TargetSelector TargetSelector { get; }
    
    /// <summary>
    /// Gets the AttackerType of this entity, used for friendly/hostile detection.
    /// </summary>
    AttackerType AttackerType { get; }

    /// <summary>
    /// Gets the current attack behavior of this attacker.
    /// </summary>
    AttackBehavior AttackBehavior { get; }

    /// <summary>
    /// Initiates an attack towards a specific health target.
    /// </summary>
    /// <param name="healthTarget">The target to attack.</param>
    IEnumerator Attack(IHealthTarget healthTarget);
}

public enum AttackerType
{
    None = 0,
    Player = 1 << 0,
    Monster = 1 << 1
}