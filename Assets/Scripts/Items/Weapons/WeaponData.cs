using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Items/Weapon")]
public class WeaponData : ItemData, IWeapon
{
    [Header("Weapon Stats")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float range = 2f;
    [SerializeField] private AttackBehavior weaponBehavior;
    [SerializeField] private TargetSelector targetSelector;

    // IWeapon Interface Properties
    public float Damage => damage;
    public float AttackSpeed => attackSpeed;
    public float Range => range;
    
    // These properties are now part of the IWeapon interface and are provided by WeaponData.
    // The AttackerComponent will use these values.
    public AttackBehavior WeaponBehavior => weaponBehavior;
    public TargetSelector TargetSelector => targetSelector;
    
    // AttackerType and AttackEnable are now managed by the IAttacker component (e.g., PlayerWeaponAttackerComponent).
    // They are not directly part of the WeaponData itself.
}