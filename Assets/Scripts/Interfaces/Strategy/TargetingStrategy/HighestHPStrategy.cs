using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighestHPEnemyStrategy : ITargetingStrategy
{
    
    public IEnemy SelectTarget(List<IEnemy> enemiesInRange)
    {

        if (enemiesInRange == null || enemiesInRange.Count == 0)
            return null;

        IEnemy highestHpEnemy = null;
        float maxHealth = float.MinValue;

        foreach (IEnemy enemy in enemiesInRange)
        {

            if (enemy.Pv > maxHealth)
            {
                maxHealth = enemy.Pv;
                highestHpEnemy = enemy;
            }

        }

        return highestHpEnemy;
    }

    public string GetStrategyName() => "Highest HP Enemy";
}
