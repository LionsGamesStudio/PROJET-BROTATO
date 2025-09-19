using UnityEngine;

public interface IEnemy : IDie, IDamageable, IEntity, IAttack
{
    public float RadiusRange { get; set; }

    public bool AttackEnable { get; set; }

    public float Pv { get; set; }

    public void SetPlayer(GameObject newValue);
}