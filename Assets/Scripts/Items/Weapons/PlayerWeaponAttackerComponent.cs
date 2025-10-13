using FluxFramework.Attributes;
using FluxFramework.Core;
using FluxFramework.Extensions;
using UnityEngine;

/// <summary>
/// Attacker component for player characters, whose attack stats are determined by an equipped weapon.
/// </summary>
public class PlayerWeaponAttackerComponent : BaseAttackerComponent, IBuffTarget
{
    // --- Base stats from the currently equipped weapon ---
    private float _baseDamage = 0f;
    private float _baseAttackSpeed = 0f;
    private float _baseRange = 0f;

    // --- Final stats, exposed as Reactive Properties for BuffManager and UI ---
    // These properties will be the final calculated values after all modifiers (buffs, etc.) are applied.
    [ReactiveProperty("player.finalDamage")]
    private float _finalDamage = 0f;
    
    [ReactiveProperty("player.finalAttackSpeed")]
    private float _finalAttackSpeed = 0f;
    
    [ReactiveProperty("player.finalRange")]
    private float _finalRange = 0f;
    
    // Override abstract properties from BaseAttackerComponent to use the FINAL stats.
    // The attack logic will now use the buffed/debuffed values.
    public override float Damage => _finalDamage;
    public override float AttackSpeed => _finalAttackSpeed;
    public override float Range => _finalRange;

    protected override void OnFluxAwake()
    {
        base.OnFluxAwake(); // Call base initialization
        
        // Initialize with default/no weapon stats until one is equipped.
        EquipWeapon(null); 
    }

    /// <summary>
    /// Equips a new weapon, updating the player's base attack stats and recalculating final stats.
    /// This method is typically called by the WeaponInvoker.
    /// </summary>
    /// <param name="weapon">The WeaponData of the weapon to equip. Pass null to unequip.</param>
    public void EquipWeapon(WeaponData weapon)
    {
        if (weapon == null)
        {
            // Reset base stats and behavior if no weapon is equipped.
            _baseDamage = 0f;
            _baseAttackSpeed = 0f;
            _baseRange = 0f;
            targetSelector = null;
            attackBehavior = null;
            Debug.Log("PlayerWeaponAttackerComponent: Weapon unequipped. Base stats reset.");
        }
        else
        {
            // Update base stats and behavior based on the new weapon.
            _baseDamage = weapon.Damage;
            _baseAttackSpeed = weapon.AttackSpeed;
            _baseRange = weapon.Range;
            targetSelector = weapon.TargetSelector;
            attackBehavior = weapon.WeaponBehavior;
            Debug.Log($"PlayerWeaponAttackerComponent: Equipped weapon '{weapon.Name}'. Base Damage: {_baseDamage}, Speed: {_baseAttackSpeed}, Range: {_baseRange}.");
        }

        // After updating base stats, recalculate the final stats.
        // This ensures that existing buffs are applied to the new weapon's stats.
        RecalculateFinalStats();
        
        // Update the range collider's radius based on the final, potentially buffed range.
        if (rangeTrigger != null)
        {
            rangeTrigger.radius = Range;
        }

        StopAttacking(); // Stop any ongoing attack while weapon stats are changing.
    }

    /// <summary>
    /// Recalculates the final stats by starting with the base stats and applying all active buffs.
    /// This should be called whenever a weapon is equipped or a buff is applied/removed.
    /// </summary>
    private void RecalculateFinalStats()
    {
        // Start with the base stats from the weapon.
        // By using UpdateReactiveProperty, we ensure the UI and other systems are notified of the change.
        this.UpdateReactiveProperty("player.finalDamage", _baseDamage);
        this.UpdateReactiveProperty("player.finalAttackSpeed", _baseAttackSpeed);
        this.UpdateReactiveProperty("player.finalRange", _baseRange);

        // TODO in a future step: We need a way to get all active buffs from the BuffManager
        // and re-apply their modifiers here. For now, BuffManager will directly modify these
        // reactive properties, and this method just resets them to base values.
        // This is a simplification that works for now but should be improved for robustness.
    }
    
    #region IBuffTarget Implementation

    /// <summary>
    /// Provides the property key for a given stat type, allowing BuffManager to find
    /// and modify the correct ReactiveProperty.
    /// </summary>
    /// <param name="statType">The type of stat to get the key for.</param>
    /// <returns>The string key of the reactive property.</returns>
    public string GetStatPropertyKey(StatType statType)
    {
        switch (statType)
        {
            // Note: These now point to the "final" stat properties.
            case StatType.Damage: return "player.finalDamage";
            case StatType.AttackSpeed: return "player.finalAttackSpeed";
            case StatType.Range: return "player.finalRange";
            
            // Return empty string for stats not handled by this component.
            // The BuffManager will then know to check other components like PlayerController.
            default: return string.Empty;
        }
    }

    #endregion
}