using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITargetingStrategy
{
    IEnemy SelectTarget(List<IEnemy> enemiesInRange);

}

