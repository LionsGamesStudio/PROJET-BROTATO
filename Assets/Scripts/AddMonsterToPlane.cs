using UnityEngine;

public class AddMonsterToPlane : MonoBehaviour
{
    public GameObject objectMonster; // Prefab
    private float x,z;
    private const float y = 0.125f; 

    public int nbrEnemy = 0;

    private int spawnRange = 15; // Longueur de la map 

    public void Start()
    {
        AddEnemy(objectMonster); // Pour test
    }

    public void AjouterEnemy(int nombre, GameObject objectMonster) // On rajouter
    {
        // Faire une boucle qui fait add AddEnemy du type souhaité

    }

    public void AddEnemy(GameObject objectMonster)
    {
        nbrEnemy+=1;
        x = Random.Range(-spawnRange,spawnRange);
        z = Random.Range(-spawnRange,spawnRange);
        GameObject newObject = Instantiate(objectMonster, new Vector3(x,y,z), Quaternion.identity);
        newObject.SetActive(true); // On active le monstre !

        // Orientation vers le joueur à faire 
        
        // Amélioration du code 
        
        Debug.Log("Ajout d'un ennemie sur la map");
    }

    // On peux faire des vagues précises pour chaque fonction par exemple
}
