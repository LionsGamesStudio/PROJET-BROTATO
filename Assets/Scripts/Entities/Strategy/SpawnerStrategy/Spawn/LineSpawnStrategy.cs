using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class LineSpawnStrategy : ISpawnStrategy
{
    private Vector3 linePosition;
    private bool isHorizontalLine;
    private int enemyInLine = 20;

    List<Vector3> sideSpawnPoint = new List<Vector3>
    {
        new Vector3(0, 0, 13f), // North
        new Vector3(0, 0, -13f), // South
        new Vector3(13f, 0, 0), // East
        new Vector3(-13f, 0, 0), // West
    };
    
    private Vector3 playerPos;

    public LineSpawnStrategy(Transform playerTransform)
    {
        this.playerPos = playerTransform.position;
        this.linePosition = SetLinePosition(playerPos);
        this.isHorizontalLine = DetermineLineOrientation(linePosition);
    }
    
    private bool DetermineLineOrientation(Vector3 position)
    {
        float absX = Mathf.Abs(position.x);
        float absZ = Mathf.Abs(position.z);
        return absZ > absX;
    }

    public List<GameObject> SpawnXMonster(GameObject monsterPrefab, List<Vector3> positions)
    {
        List<GameObject> spawned = new List<GameObject>();

        foreach(var pos in positions)
        {
            // We check for a valid NavMesh position before spawning.
            if (NavMesh.SamplePosition(pos, out NavMeshHit hit, 2.0f, NavMesh.AllAreas))
            {
                // Instantiate and activate. The framework handles the rest.
                GameObject enemyInstance = Object.Instantiate(monsterPrefab, hit.position, Quaternion.identity);
                enemyInstance.SetActive(true);

                // Verify the prefab is valid.
                if (enemyInstance.GetComponent<MonsterAttackerComponent>() != null)
                {
                    spawned.Add(enemyInstance);
                }
                else
                {
                    Debug.LogError($"Spawn failed: The prefab '{monsterPrefab.name}' is missing a MonsterAttackerComponent.");
                    Object.Destroy(enemyInstance);
                }
            }
            else
            {
                Debug.LogWarning($"Could not spawn: position {pos} is not on a valid NavMesh.", monsterPrefab);
            }
        }

        return spawned;
    }

    public List<Vector3> GetValidPosition(int enemyToUse)
    {
        enemyToUse = Mathf.Min(enemyToUse, enemyInLine);
        List<Vector3> enemyPositions = new List<Vector3>();
        float totalLineWidth = 30f;
        float spacing = (enemyToUse > 1) ? totalLineWidth / (enemyToUse - 1) : 0;
        float startOffset = -totalLineWidth / 2f;

        for (int i = 0; i < enemyToUse; i++)
        {
            Vector3 enemyPosition;
            float offset = startOffset + spacing * i;

            if (isHorizontalLine)
            {
                enemyPosition = new Vector3(linePosition.x + offset, linePosition.y, linePosition.z);
            }
            else
            {
                enemyPosition = new Vector3(linePosition.x, linePosition.y, linePosition.z + offset);
            }
            enemyPositions.Add(enemyPosition);
        }
        return enemyPositions;
    }

    public Vector3 SetLinePosition(Vector3 playerPos)
    {
        float maxDistance = float.MinValue;
        Vector3 farthestSide = Vector3.zero;

        foreach (Vector3 side in sideSpawnPoint)
        {
            float distance = Vector3.Distance(playerPos, side);
            if (distance > maxDistance)
            {
                maxDistance = distance;
                farthestSide = side;
            }
        }
        return farthestSide;
    }

    public string GetStrategyName() => "Line Spawn";
}