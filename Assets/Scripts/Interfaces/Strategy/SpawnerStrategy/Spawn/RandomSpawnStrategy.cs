
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class RandomSpawnStrategy : ISpawnStrategy
{
    // Start is called before the first frame update

    private float spawnRange = 15f; // Radius of the map will change
    private float playerRange = 3f; // Radius wanted will change

    private GameObject player;
    Vector3 playerPos;

    public RandomSpawnStrategy(GameObject newPlayer)
    {
        player = newPlayer;
        playerPos = player.transform.position;
    }


    public List<GameObject> SpawnXMonster(GameObject objectMonster, List<Vector3> positions)
    {

        List<GameObject> spawned = new List<GameObject>();

        GameObject enemy = Object.Instantiate(objectMonster, positions[0], Quaternion.identity);

        enemy.gameObject.SetActive(true); // ✅ Le monstre "s'éveille"
        
        Monster monster = enemy.GetComponent<Monster>();
        monster.SetPlayer(player);
        monster.Initialize();

        if (monster != null)
        {
            spawned.Add(enemy);
        }

        else
        {
            Debug.Log("Spawn échoué : prefab sans IEnemy strategy pattern Random");

        }
        return spawned;
    }


    public List<Vector3> GetValidPosition(int enemyToUse)
    {
        
        for (int attempt = 0; attempt < 50; attempt++)
        {
            // Générer une position aléatoire dans la zone -15/+15
            float x = Random.Range(-spawnRange, spawnRange);
            float z = Random.Range(-spawnRange, spawnRange);

            List<Vector3> potentialPositions = new List<Vector3> { new Vector3(x, 0, z) }; // On initialise le/les points de spawn 

            // Vérifier si on est assez loin du joueur
            float distanceToPlayer = Vector3.Distance(potentialPositions[0], playerPos);

            if (distanceToPlayer >= playerRange)
            {
                return potentialPositions;
            }
        }
        return ForcedValidPosition(); // Near impossible to have that
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
