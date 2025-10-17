using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSpawnStrategy : ISpawnStrategy
{
    private float spawnRange = 15f;
    private float playerRange = 3f;
    private Vector3 playerPos;

    public RandomSpawnStrategy(Transform playerTransform)
    {
        this.playerPos = playerTransform.position;
    }

    public List<GameObject> SpawnXMonster(GameObject monsterPrefab, List<Vector3> positions)
    {
        List<GameObject> spawned = new List<GameObject>();

        // Instantiate and activate the prefab. The framework handles the rest.
        GameObject enemyInstance = Object.Instantiate(monsterPrefab, positions[0], Quaternion.identity);
        enemyInstance.SetActive(true);

        // Verify the prefab is valid by checking for the correct component.
        if (enemyInstance.GetComponent<MonsterAttackerComponent>() != null)
        {
            spawned.Add(enemyInstance);
        }
        else
        {
            Debug.LogError($"Spawn failed: The prefab '{monsterPrefab.name}' is missing a MonsterAttackerComponent.");
            Object.Destroy(enemyInstance);
        }

        return spawned;
    }

    public List<Vector3> GetValidPosition(int enemyToUse)
    {
        for (int attempt = 0; attempt < 50; attempt++)
        {
            float x = Random.Range(-spawnRange, spawnRange);
            float z = Random.Range(-spawnRange, spawnRange);
            List<Vector3> potentialPositions = new List<Vector3> { new Vector3(x, 0, z) };
            float distanceToPlayer = Vector3.Distance(potentialPositions[0], playerPos);
            if (distanceToPlayer >= playerRange)
            {
                return potentialPositions;
            }
        }
        return ForcedValidPosition();
    }

    private List<Vector3> ForcedValidPosition()
    {
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        float spawnDistance = playerRange + 1f;
        float x = playerPos.x + Mathf.Cos(randomAngle) * spawnDistance;
        float z = playerPos.z + Mathf.Sin(randomAngle) * spawnDistance;
        x = Mathf.Clamp(x, -spawnRange, spawnRange);
        z = Mathf.Clamp(z, -spawnRange, spawnRange);
        return new List<Vector3> { new Vector3(x, 0, z) };
    }

    public string GetStrategyName() => "Random Spawn";
}