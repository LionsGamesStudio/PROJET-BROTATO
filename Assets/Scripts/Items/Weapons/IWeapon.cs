
public interface IWeapon : IItem, IAttacker
{
    WeaponBehavior WeaponBehavior { get; }
}