using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ISpawnStrategy
{
    /// <summary>
    /// Spawns a specified number of monsters at the given position
    /// </summary>
    /// <param name="objectMonster">The monster prefab to spawn</param>
    /// <param name="validPosition">The world position where monsters will spawn</param>
    /// <param name="numberOfEnemies">Number of monsters in the current SpawningPattern</param>
    /// <returns>The actual number of monsters spawned</returns>

    List<GameObject> SpawnXMonster(GameObject objectMonster, List<Vector3> validPosition); 

    /// <summary>
    /// Prepare the fonction of SpawnXMonster with geting valid position in function of the strategy
    /// </summary>
    /// <returns>A list a position that enemy or enemies are going to spawn </returns>
    List<Vector3> GetValidPosition(int count); 

}

