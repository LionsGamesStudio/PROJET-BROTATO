using System.Collections.Generic;
using UnityEngine;

public interface ISpawnStrategy
{
    /// <summary>
    /// Spawns monsters using the provided prefab at the given positions.
    /// </summary>
    /// <param name="monsterPrefab">The monster prefab to spawn.</param>
    /// <param name="positions">The world positions where monsters will spawn.</param>
    /// <returns>A list of the successfully spawned monster GameObjects.</returns>
    List<GameObject> SpawnXMonster(GameObject monsterPrefab, List<Vector3> positions); 

    /// <summary>
    /// Calculates a list of valid spawn positions based on the strategy's rules.
    /// </summary>
    /// <param name="count">The number of positions to generate.</param>
    /// <returns>A list of valid spawn positions for the monsters.</returns>
    List<Vector3> GetValidPosition(int count); 
}