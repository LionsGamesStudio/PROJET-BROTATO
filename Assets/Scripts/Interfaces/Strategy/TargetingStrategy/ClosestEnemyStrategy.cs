using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClosestEnemyStrategy : ITargetingStrategy
{
    private IWeapon weapon;

    public ClosestEnemyStrategy(IWeapon newWeapon)
    {
        weapon = newWeapon;
    }

    public IEnemy SelectTarget(List<IEnemy> enemiesInRange )
    {
        if (enemiesInRange == null || enemiesInRange.Count == 0) 
            return null;

        IEnemy closestEnemy = null;
        float minDistance = float.MaxValue;
        Vector3 weaponPosition = (weapon as MonoBehaviour).transform.position;

        foreach (IEnemy enemy in enemiesInRange)
        {
            Vector3 enemyPosition = (enemy as MonoBehaviour).transform.position;
            float distance = Vector3.Distance(enemyPosition, weaponPosition);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }

        }

        return closestEnemy;
    }
    public string GetStrategyName() => "Closest Enemy";
}
