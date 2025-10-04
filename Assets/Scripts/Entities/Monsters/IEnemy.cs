using UnityEngine;

public interface IEnemy : IEntity
{
    public float RadiusRange { get; set; }

    public bool AttackEnable { get; set; }

    public float Pv { get; set; }

    public void SetPlayer(GameObject newValue);
}