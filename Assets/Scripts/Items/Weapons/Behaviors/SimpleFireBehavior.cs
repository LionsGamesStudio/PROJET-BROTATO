
public class SimpleFireBehavior : WeaponBehavior
{
    public override void Execute(IHealthTarget healthTarget, IWeapon weapon)
    {
        if (healthTarget == null || weapon == null) return;

        var damage = weapon.Damage;
    }
}