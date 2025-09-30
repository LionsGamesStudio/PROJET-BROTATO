

using System.Collections.Generic;
using UnityEngine;

public class LineSpawnStrategy : ISpawnStrategy
{
    // Start is called before the first frame update
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

    private GameObject player;
    Vector3 playerPos;

    public LineSpawnStrategy(GameObject newPlayer)
    {
        player = newPlayer;
        playerPos = player.transform.position;
        linePosition = SetLinePosition(playerPos);

        isHorizontalLine = DetermineLineOrientation(linePosition);
    }
    
    private bool DetermineLineOrientation(Vector3 position)
    {
        // Si la position est sur l'axe X (Est ou Ouest), la ligne est VERTICALE (s'étend sur Z)
        // Si la position est sur l'axe Z (Nord ou Sud), la ligne est HORIZONTALE (s'étend sur X)
        
        float absX = Mathf.Abs(position.x);
        float absZ = Mathf.Abs(position.z);
        
        // Si X est dominant, on est à l'Est ou Ouest → ligne verticale
        // Si Z est dominant, on est au Nord ou Sud → ligne horizontale
        return absZ > absX;
    }


    public List<GameObject> SpawnXMonster(GameObject objectMonster, List<Vector3> positions)
    {

        List<GameObject> spawned = new List<GameObject>();
        for (int i = 0; i < positions.Count; i++)
        {
            GameObject enemy = Object.Instantiate(objectMonster, positions[i], Quaternion.identity);
            IEnemy enemyComponent = enemy.GetComponent<IEnemy>();
            if (enemyComponent != null)
            {
                enemyComponent.SetPlayer(player);
                spawned.Add(enemy);
            }
        }

        return spawned;
    }


    public List<Vector3> GetValidPosition(int enemyToUse) // Set all the position
    {
        enemyToUse = Mathf.Min(enemyToUse, enemyInLine);  // Nombre max d'ennemie par ligne = 20 de base

        List<Vector3> enemyPositions = new List<Vector3>();

        float totalLineWidth = 30f;

        float spacing = totalLineWidth / enemyToUse;

        // Calculer le point de départ (à gauche ou en bas de la ligne --> Dans le négatif)
        float startOffset = -totalLineWidth / 2f;

        for (int i = 0; i < enemyToUse; i++)
        {
            Vector3 enemyPosition = Vector3.zero;

            float offset = startOffset + spacing * (i + 1); // On va aller de gauche à droite en espaçant d'un certain nombre à chaque fois

            if (isHorizontalLine)
            {
                enemyPosition = new Vector3(
                    linePosition.x + offset,
                    linePosition.y + 0.125f,
                    linePosition.z
                );
            }
            else if (!isHorizontalLine)
            {
                enemyPosition = new Vector3(
                    linePosition.x,
                    linePosition.y + 0.125f,
                    linePosition.z + offset
                );
            }

            enemyPositions.Add(enemyPosition);
        }

        return enemyPositions;
    
    
    }

    public Vector3 SetLinePosition(Vector3 playerPos) // SetTheLine position at the start of the wave
    {
        // On regarde le point le plus loin du joueur et on set la position de la ligne à celle la plus loin 
        float maxDistance = float.MinValue;
        Vector3 currentSide = Vector3.zero;

        foreach (Vector3 side in sideSpawnPoint)
        {
            float sidePlayerDistance = Vector3.Distance(playerPos, side);
            // Calcul of distance

            if (maxDistance < sidePlayerDistance)
            {
                maxDistance = sidePlayerDistance;
                currentSide = side;
            }
        }

        return currentSide;
    }
    public string GetStrategyName() => "Line Spawn";
}
