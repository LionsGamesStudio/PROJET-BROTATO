using System.Collections;
using System.Threading;
using UnityEngine;

public class AddMonsterToPlane : MonoBehaviour
{
    public GameObject objectMonster; // Prefab to make
    private float x,z;
    private const float y = 0.125f; 
    public int nbrEnemy = 0;

    private float spawnRange = 15f; // Radius of the map

    private float range = 2f; 
    private float playerRange = 2f; // For the range --> à revoir

    private ClassPerso player;

    private bool nextVague = false;

    // Radius of the player to dont add to plane for security of gameplay 
    // --> don't work just work for the start because we don't take the position of the player

    public void Start()
    {
        player = GameObject.Find("-- XR Origin").GetComponent<ClassPerso>(); // Faire une fonction pour avoir le player :)
        nextVague = false;
        Vague1();
        
    }

    IEnumerator AjouterEnemy(int nombre, GameObject objectMonster, float temp) // We add an enemy to the plane
    {
        // Faire une boucle qui fait add AddEnemy du type souhaité
        for (int i = 0; i < nombre; i++)
        {
            AddEnemy(objectMonster);
            yield return new WaitForSeconds(temp);
        }
        
        yield break;

    }

    public void AddEnemy(GameObject objectMonster)
    {
        nbrEnemy+=1;

        Debug.Log("Il y a un total de" + nbrEnemy + " sur la map !");

        x = Random.Range(-spawnRange,spawnRange);
        while (x < playerRange && x > -playerRange) // Compris entre 2 et -2 mais pas mis à jour par rapport à la position du joueur
        {
            x = Random.Range(-spawnRange,spawnRange);
        }

        z = Random.Range(-spawnRange,spawnRange);

        while (z < playerRange && z > -playerRange)
        {
            z = Random.Range(-spawnRange,spawnRange);
        }

        
        GameObject newObject = Instantiate(objectMonster, new Vector3(x,y,z), Quaternion.identity);
        newObject.SetActive(true); // We set true the apparition to the monster !

        Debug.Log("Ajout d'un ennemie sur la map avec la position ("+x+") ("+y+") ("+z+")");
    }

    // On peux faire des vagues précises pour chaque fonction par exemple --> Scriptable objet ? 

    private bool timer = true;
    void Vague1()
    {
        
        StartCoroutine(AjouterEnemy(80, objectMonster,0.5f));

        // On reset toutes les entités qui ont le tag enemy 

        // On passe à la phase de shop 

        // Puis quand c'est fini on lance la vague d'après 
    }
        

}
