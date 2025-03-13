using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

public class Class_Monster : MonoBehaviour
{

    public int pv;
    public int damage;
    public int moneyValue;
    public float attackSpeed;
    public float movementSpeed; // float meter per second
    public int shield;
    public string typeMonster;
    public float radiusRange;

    public bool AttackEnable = false;
    public bool isTargeted = false; // Pour que les 6 armes focus différents enemies --> J'ai des problèmes donc nique sa mère

    private bool isAttacking = false;

    

    private Transform target; // Player target

    private GameObject player;

    private List<Class_Monster> enemiesinRange ;


    void Start()
    {   
        player = GameObject.Find("-- XR Origin");
        target = player.transform;
    }


    void GoPlayer() 
    {          
        Vector3 direction = target.position - transform.position; // Calcul de la direction
        direction.y = 0; // Ignore l'axe Y pour éviter une inclinaison

        transform.rotation = Quaternion.LookRotation(direction);

        // Pour avancer vers la cible sans changer de hauteur
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime); 
    }

    
    IEnumerator Attack() // METTRE EN COROUTINE
    {   
        isAttacking = true;

        while (isAttacking) 
        {

            ClassPerso player = GameObject.Find("-- XR Origin").GetComponent<ClassPerso>(); // On récupère les données du joueur si elle on été màj

            player.SubirDegats_SupprimerSante(damage);

            Debug.Log("L'ennemie attaque !");

            yield return new WaitForSeconds(1/attackSpeed); // Ca arrête la possibilité d'attaquer pendant x seconde 

            Debug.Log("L'ennemie peut de nouveau attaquer !");

            isAttacking = false;

            yield break;
        }
        

    }

    
    void Update() // S'oriente vers le joueur + avance --> à traduire
    {   
        if (!isAttacking|| (isAttacking && !AttackEnable) ) // Il avance quand il n'attaque pas ou il avance quand il peut attaquer mais est en cooldown 
        {
            GoPlayer();
        }
        

        if (AttackEnable && !isAttacking) 
        {
            StartCoroutine(Attack()); // Lancement de la coroutine de l'attaque !
        }
        

    }

    public void TestDie() // Sera lancé en tant que test quand l'ennemi se fera toucher.
    {
        if (pv <= 0)
        {
            Destroy(gameObject); 

            ClassPerso character = player.GetComponent<ClassPerso>(); 
            character.money += moneyValue;

        }
    }
}
