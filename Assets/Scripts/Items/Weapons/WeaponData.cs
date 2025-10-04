using System.Collections;
using UnityEngine;

[CreateAssetMenu(fileName = "WeaponData", menuName = "Items/Weapon")]
public class WeaponData : ItemData, IWeapon
{
    [Header("Weapon Stats")]
    [SerializeField] private float damage = 10f;
    [SerializeField] private float attackSpeed = 1f;
    [SerializeField] private float range = 2f;
    [SerializeField] private WeaponBehavior weaponBehavior;
    [SerializeField] private TargetSelector targetSelector;

    public float Damage => damage;
    public float AttackSpeed => attackSpeed;
    public float Range => range;
    public bool AttackEnable { get; set; } = true;
    public AttackerType AttackerType => AttackerType.Player;
    public WeaponBehavior WeaponBehavior => weaponBehavior;
    public TargetSelector TargetSelector => targetSelector;

    public IEnumerator Attack(IHealthTarget healthTarget)
    {
        if (AttackEnable)
        {
            AttackEnable = false;
            weaponBehavior.Execute(healthTarget, this);
            yield return new WaitForSeconds(1f / attackSpeed);
            AttackEnable = true;
        }
    }
}