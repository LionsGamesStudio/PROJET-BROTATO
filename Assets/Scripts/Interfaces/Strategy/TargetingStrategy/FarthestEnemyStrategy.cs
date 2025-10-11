using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FarthestEnemyStrategy : ITargetingStrategy
{
    private IWeapon weapon;

    public FarthestEnemyStrategy(IWeapon newWeapon)
    {
        weapon = newWeapon;
    }

    public IEnemy SelectTarget(List<IEnemy> enemiesInRange)
    {

        if (enemiesInRange == null || enemiesInRange.Count == 0) 
            return null;

        IEnemy farthestEnemy = null;
        float maxDistance = float.MinValue;

        Vector3 weaponPosition = (weapon as MonoBehaviour).transform.position;

        foreach (IEnemy enemy in enemiesInRange)
        {
            Vector3 enemyPosition = (enemy as MonoBehaviour).transform.position;
            float distance = Vector3.Distance(enemyPosition, weaponPosition);

            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestEnemy = enemy;
            }

        }

        return farthestEnemy;
    }

    public string GetStrategyName() => "Farthest Enemy";
}
