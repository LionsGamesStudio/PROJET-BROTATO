using System.Collections;
using System.Collections.Generic;

public interface IAttacker : IEntity
{
    bool AttackEnable { get; set; }
    float Damage { get; }
    float AttackSpeed { get; }
    float Range { get; }
    TargetSelector TargetSelector { get; }
    IEnumerator Attack(IHealthTarget healthTarget);
    AttackerType AttackerType { get; }
}

public enum AttackerType
{
    None = 0,
    Player = 1 << 0,
    Monster = 1 << 1
}