using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomEnemyStrategy : ITargetingStrategy
{
    public IEnemy SelectTarget(List<IEnemy> enemiesInRange)
    {
        if (enemiesInRange.Count == 0)
            return null;

        return enemiesInRange[Random.Range(0, enemiesInRange.Count)];
    }

    public string GetStrategyName() => "Random Enemy";
}
