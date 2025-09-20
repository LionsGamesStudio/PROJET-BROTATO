using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowestHPEnemyStrategy : ITargetingStrategy
{
    
    public IEnemy SelectTarget(List<IEnemy> enemiesInRange)
    {

        if (enemiesInRange == null || enemiesInRange.Count == 0)
            return null;

        IEnemy highestHpEnemy = null;
        float minHealth = float.MaxValue;

        foreach (IEnemy enemy in enemiesInRange)
        {

            if (enemy.Pv < minHealth)
            {
                minHealth = enemy.Pv;
                highestHpEnemy = enemy;
            }

        }

        return highestHpEnemy;
    }

    public string GetStrategyName() => "Lowest HP Enemy";
}
