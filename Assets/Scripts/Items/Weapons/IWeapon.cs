
public interface IWeapon : IItem
{
    float Damage { get; }
    float AttackSpeed { get; }
    float Range { get; }
    AttackBehavior WeaponBehavior { get; }
    TargetSelector TargetSelector { get; }
}