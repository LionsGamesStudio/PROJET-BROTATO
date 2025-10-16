/// <summary>
/// Defines the contract for any entity capable of performing an attack.
/// </summary>
public interface IAttacker
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
    /// Gets the AttackerType of this entity, used for friendly/hostile detection.
    /// </summary>
    AttackerType AttackerType { get; }

    /// <summary>
    /// Gets the current attack behavior of this attacker.
    /// </summary>
    AttackBehavior AttackBehavior { get; }

    /// <summary>
    /// NEW: A direct command to perform an attack on a specific target.
    /// This replaces the old coroutine-based method.
    /// </summary>
    /// <param name="healthTarget">The target to attack.</param>
    void PerformAttack(IHealthTarget healthTarget);
}

/// <summary>
/// A bitmask enum to define entity types for targeting and friendly-fire logic.
/// The [Flags] attribute is a best practice for bitmask enums.
/// </summary>
[System.Flags]
public enum AttackerType
{
    None = 0,
    Player = 1 << 0,  // Binary 0001
    Monster = 1 << 1  // Binary 0010
}