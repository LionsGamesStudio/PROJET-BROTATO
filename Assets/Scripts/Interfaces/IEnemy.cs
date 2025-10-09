using UnityEngine;

public interface IEnemy : IDie, IDamageable, IEntity//, IAttack // Revoir ça de plus près pour faire un truc carré !
{
    public float RadiusRange { get; set; }

    public bool AttackEnable { get; set; }

    public float Pv { get; set; }

    public bool IsAttacking { get; set;}

    public void SetPlayer(GameObject newValue);
}