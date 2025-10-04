using System.Collections;
using System.Threading;
using UnityEngine;

using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.ProjectWindowCallback;
using System.Linq;
using System.Data;
public class AddMonsterToPlane : MonoBehaviour
{
    public List<GameObject> prefabMonster = new List<GameObject>();  // All the prefab of the slimes
    private float x,z;
    private const float y = 0.125f; 
    private float spawnRange = 15f; // Radius of the map
    private float playerRange = 2f; // For the range --> à revoir
    
    [SerializeField]
    private GameObject player;
    private Class_Perso classPerso; // A changer
    public int vague;

    // public InventorySyncronisationWeapon inventorySyncronisationWeapon; // C'est pour faire comme si on achete une arme chaque round
    // private List<Class_Weapon> actualWeapon;

    // Radius of the player to dont add to plane for security of gameplay 
    // --> don't work just work for the start because we don't take the position of the player

    public void Start()
    {
        classPerso = player.GetComponent<Class_Perso>();
        vague = classPerso.vague;

        Vague1(); // TEST
        
    }

    public void ResetAllEnemy() // A modifier
    {
        // On récupère tous les MonoBehaviour actifs dans la scène
        MonoBehaviour[] behaviours = FindObjectsOfType<MonoBehaviour>();

        foreach (var b in behaviours)
        {
            if (b is IEnemy enemy)
            {
                Destroy((enemy as MonoBehaviour).gameObject); // si ton interface expose un GameObject
            }
        }
    }


    private void NextWave() // Ce code est complétement à revoir, il est uniquement à but de faire un truc potable...
    {
        classPerso.NextVague();
        vague = classPerso.vague;

        if (vague == 1)
        {
            Debug.Log($"Lancement de la vague {vague}");
            Vague1();
        }

        if (vague == 2)
        {
            Vague2();
            Debug.Log($"Lancement de la vague {vague}");
            var set2 = GameObject.Find("Set (2)"); // Faire un truc du style ${nbr_set}
            var weapon2 = set2.transform.Find("Pistol - Common");
            weapon2.gameObject.SetActive(true);

            // inventorySyncronisationWeapon.GetWeaponAndSyncronise();
            // inventorySyncronisationWeapon.Syncronisation();

        }

        if (vague == 3)
        {
            Vague3();
            Debug.Log($"Lancement de la vague {vague}");
            var set3 = GameObject.Find("Set (3)");
            var weapon3 = set3.transform.Find("Pistol - Common");
            weapon3.gameObject.SetActive(true);
            // inventorySyncronisationWeapon.GetWeaponAndSyncronise();
            // inventorySyncronisationWeapon.Syncronisation();
        }

        if (vague == 4)
        {
            Vague4();
            Debug.Log($"Lancement de la vague {vague}");
            var set4 = GameObject.Find("Set (4)");
            var weapon4 = set4.transform.Find("Pistol - Common");
            weapon4.gameObject.SetActive(true);
            // inventorySyncronisationWeapon.GetWeaponAndSyncronise();
            // inventorySyncronisationWeapon.Syncronisation();
        }

        if (vague == 5)
        {
            Vague5();
            Debug.Log($"Lancement de la vague {vague}");
            var set5 = GameObject.Find("Set (5)");
            var weapon5 = set5.transform.Find("Pistol - Common");
            weapon5.gameObject.SetActive(true);
            // inventorySyncronisationWeapon.GetWeaponAndSyncronise();
            // inventorySyncronisationWeapon.Syncronisation();
        }

        if (vague == 6)
        {
            Vague6();
            Debug.Log($"Lancement de la vague {vague}");
            var set6 = GameObject.Find("Set (6)");
            var weapon6 = set6.transform.Find("Pistol - Common");
            weapon6.gameObject.SetActive(true);
            // inventorySyncronisationWeapon.GetWeaponAndSyncronise();
            // inventorySyncronisationWeapon.Syncronisation();

        }

        if (vague == 7)
        {
            Vague7();
            Debug.Log($"Lancement de la vague {vague}");
        }

        if (vague == 8)
        {
            Vague8();
            Debug.Log($"Lancement de la vague {vague}");
        }

        if (vague == 9)
        {
            Vague9();
            Debug.Log($"Lancement de la vague {vague}");
        }

        if (vague == 10)
        {
            Vague10();
            Debug.Log($"Lancement de la vague {vague}");
        }

        if (vague >= 10)
        {
            // Mettre une vague qui deviens de plus en plus compliqué 
            Vague10();
            Debug.Log($"Lancement de la vague {vague}");
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

        Debug.Log("Lancement de l'attente de 10 sec + reset de tous les ennemies");
        ResetAllEnemy();
        yield return new WaitForSeconds(10f); // TEST
        Debug.Log("Fin des 10 secondes d'attentes");


        Debug.Log("Lancement de la vague suivante !");
        NextWave(); // Je peux faire spawn en boucle mes vagues --> Fonction qui repertorie toutes les vagues et return celle que je veux par rapport à la vague actuelle par exemple
        yield break;

    }

    public void AddEnemy(GameObject objectMonster)
    {

        // Gestion de la position du joueur
        Vector3 positionPlayer = classPerso.transform.position; // On l'utisera probablement pas mais je n'ai pas envie qu'il spawn sur le joueur

        //int spawnRangex = spawnRange - positionPlayer;
        //int spawnRangez2 = spawnRange - positionPlayer

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

        
        GameObject newMonster = Instantiate(objectMonster, new Vector3(x,y,z), Quaternion.identity);
        newMonster.SetActive(true); // We set true the apparition to the monster !
        IEnemy scriptMonster = newMonster.GetComponent<IEnemy>();
        scriptMonster.SetPlayer(player);

        Debug.Log($"Ajout d'un ennemie sur la map avec la position ({x}) ({y}) ({z})");
    }

    // On peux faire des vagues précises pour chaque fonction par exemple --> Scriptable objet ? 


    void Vague1()
    {
        StartCoroutine(AjouterEnemy(26, prefabMonster[0], 1f));
        StartCoroutine(AjouterEnemy(26, prefabMonster[1], 1f));
        StartCoroutine(AjouterEnemy(26, prefabMonster[2], 1f));


        // On reset toutes les entités qui ont le tag enemy 

        // On passe à la phase de shop 

        // Puis quand c'est fini on lance la vague d'après 


    }

    void Vague2() // Ajout d'une arme commune
    {
        StartCoroutine(AjouterEnemy(50, prefabMonster[0],0.5f));
    }
        
    
    void Vague3()
    {
        StartCoroutine(AjouterEnemy(60, prefabMonster[0],0.4f));
        StartCoroutine(AjouterEnemy(5, prefabMonster[1],5f));

    }

    void Vague4()
    {
        StartCoroutine(AjouterEnemy(80, prefabMonster[0],0.4f));
        StartCoroutine(AjouterEnemy(10, prefabMonster[1],3f));
    }

    void Vague5()
    {
        StartCoroutine(AjouterEnemy(80, prefabMonster[0],0.4f));
        StartCoroutine(AjouterEnemy(15, prefabMonster[1],2.5f));
        StartCoroutine(AjouterEnemy(5, prefabMonster[2],5f));
    }

    void Vague6() // Changement de l'arme commune avec l'arme uncommun 
    {
        StartCoroutine(AjouterEnemy(80, prefabMonster[0],0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }

    void Vague7()
    {
        StartCoroutine(AjouterEnemy(80, prefabMonster[0],0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }

    void Vague8()
    {
        StartCoroutine(AjouterEnemy(80, prefabMonster[0],0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }

    void Vague9()
    {
        StartCoroutine(AjouterEnemy(80, prefabMonster[0],0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }

    void Vague10()
    {
        StartCoroutine(AjouterEnemy(80, prefabMonster[0],0.4f));
        StartCoroutine(AjouterEnemy(20, prefabMonster[1],2.5f));
    }


}
