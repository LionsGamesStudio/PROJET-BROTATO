using System.Collections;
using System.Threading;
using UnityEngine;

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
public class AddMonsterToPlane : MonoBehaviour
{
    public GameObject objectMonster; // Prefab to make

    public List<GameObject> prefabMonster = new List<GameObject>();  // All the prefab of the slimes

    private float x,z;
    private const float y = 0.125f; 
    public int nbrEnemy = 0;

    private float spawnRange = 15f; // Radius of the map

    private float range = 2f; 
    private float playerRange = 2f; // For the range --> à revoir

    private ClassPerso player;
    /// <summary>
    /// Clélie le meilleure et la plus belle du monde hiHIHIHIHI SKIN CL2LIE, Io sono ziziman
    /// </summary>

    private bool nextVague = false;

    public int vague;

    // Radius of the player to dont add to plane for security of gameplay 
    // --> don't work just work for the start because we don't take the position of the player

    public void Start()
    {
        player = GameObject.Find("-- XR Origin").GetComponent<ClassPerso>(); // Faire une fonction pour avoir le player :)
        vague = player.vague;

        Vague1(); // TEST
        
    }

    private void NextWave() // Ce code est complétement à revoir, il est uniquement à but de faire un truc potable...
    {
        player.NextVague();
        vague = player.vague;
        if (vague == 1) 
        {
            Debug.Log("Lancement de la vague 1");
            Vague1();
        }

        if (vague == 2) 
        {
            Vague2();
            Debug.Log("Lancement de la vague 2");
            var set2 = GameObject.Find("Set (2)");
            var weapon2 = set2.transform.Find("Pistol - Common");
            weapon2.gameObject.SetActive(true);
            
        }

        if (vague == 3) 
        {
            Vague3();
            Debug.Log("Lancement de la vague 3");
            var set3 = GameObject.Find("Set (2)");
            var weapon3 = set3.transform.Find("Pistol - Common");
            weapon3.gameObject.SetActive(true);
        }

        if (vague == 4) 
        {
            Vague4();
            Debug.Log("Lancement de la vague 4");
            var set4 = GameObject.Find("Set (2)");
            var weapon4 = set4.transform.Find("Pistol - Common");
            weapon4.gameObject.SetActive(true);
        }

        if (vague == 5) 
        {
            Vague5();
            Debug.Log("Lancement de la vague 5");
            var set5 = GameObject.Find("Set (2)");
            var weapon5 = set5.transform.Find("Pistol - Common");
            weapon5.gameObject.SetActive(true);
        }

        if (vague == 6) 
        {
            Vague6();
            Debug.Log("Lancement de la vague 6");
            var set6 = GameObject.Find("Set (2)");
            var weapon6 = set6.transform.Find("Pistol - Common");
            weapon6.gameObject.SetActive(true);
        }

        if (vague == 7) 
        {
            Vague7();
            Debug.Log("Lancement de la vague 7");
        }

        if (vague == 8) 
        {
            Vague8();
            Debug.Log("Lancement de la vague 8");
        }

        if (vague == 9) 
        {
            Vague9();
            Debug.Log("Lancement de la vague 9");
        }

        if (vague == 10) 
        {
            Vague10();
            Debug.Log("Lancement de la vague 10");
        }

        if (vague >= 10) 
        {
            // Mettre une vague qui deviens de plus en plus compliqué 
            Vague10();
            Debug.Log("Lancement de la vague "+vague);
        }

    }

    IEnumerator AjouterEnemy(int nombre, GameObject objectMonster, float temp) // We add an enemy to the plane
    {
        // Faire une boucle qui fait add AddEnemy du type souhaité
        for (int i = 0; i < nombre; i++)
        {
            AddEnemy(objectMonster);
            yield return new WaitForSeconds(temp);
        }
        
        

        Debug.Log("Lancement de l'attente de 10 sec");
        yield return new WaitForSeconds(10f); // TEST
        Debug.Log("Fin des 10 secondes d'attentes");


        Debug.Log("Lancement de la vague suivante !");
        NextWave(); // Je peux faire spawn en boucle mes vagues --> Fonction qui repertorie toutes les vagues et return celle que je veux par rapport à la vague actuelle par exemple
        yield break;

    }

    void AjouterEnemyList(List<int> nombre,List<GameObject> prefabMOnster) // One of this for each vague later
    {   
        for (int i = 0; i < prefabMOnster.Count - 1 ; i++)
        {   
            AjouterEnemy(nombre[i],prefabMOnster[i],0.5f);
        }
        
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

        Debug.Log($"Ajout d'un ennemie sur la map avec la position ({x}) ({y}) ({z})");
    }

    // On peux faire des vagues précises pour chaque fonction par exemple --> Scriptable objet ? 


    void Vague1()
    {   
        StartCoroutine(AjouterEnemy(25, objectMonster,1f));


        // On reset toutes les entités qui ont le tag enemy 

        // On passe à la phase de shop 

        // Puis quand c'est fini on lance la vague d'après 

        
    }

    void Vague2() // Ajout d'une arme commune
    {
        StartCoroutine(AjouterEnemy(50, objectMonster,0.5f));
    }
        
    
    void Vague3()
    {
        StartCoroutine(AjouterEnemy(60, objectMonster,0.4f));
        StartCoroutine(AjouterEnemy(5, prefabMonster[1],5f));

    }

    void Vague4()
    {
        StartCoroutine(AjouterEnemy(80, objectMonster,0.4f));
        StartCoroutine(AjouterEnemy(10, prefabMonster[1],3f));
    }

    void Vague5()
    {
        StartCoroutine(AjouterEnemy(80, objectMonster,0.4f));
        StartCoroutine(AjouterEnemy(15, prefabMonster[1],2.5f));
        StartCoroutine(AjouterEnemy(5, prefabMonster[2],5f));
    }

    void Vague6() // Changement de l'arme commune avec l'arme uncommun 
    {
        StartCoroutine(AjouterEnemy(80, objectMonster,0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }

    void Vague7()
    {
        StartCoroutine(AjouterEnemy(80, objectMonster,0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }

    void Vague8()
    {
        StartCoroutine(AjouterEnemy(80, objectMonster,0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }

    void Vague9()
    {
        StartCoroutine(AjouterEnemy(80, objectMonster,0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }

    void Vague10()
    {
        StartCoroutine(AjouterEnemy(80, objectMonster,0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }

    void Vague11()
    {
        StartCoroutine(AjouterEnemy(80, objectMonster,0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }

    void Vague12()
    {
        StartCoroutine(AjouterEnemy(80, objectMonster,0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }

    void Vague13()
    {
        StartCoroutine(AjouterEnemy(80, objectMonster,0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }

    void Vague14()
    {
        StartCoroutine(AjouterEnemy(80, objectMonster,0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }

    void Vague15()
    {
        StartCoroutine(AjouterEnemy(120, objectMonster,0.4f));
        StartCoroutine(AjouterEnemy(30, prefabMonster[1],2.5f));
    }
}
