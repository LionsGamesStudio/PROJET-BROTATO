using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager.Requests;
using UnityEngine;


public class Slime : MonoBehaviour, IEntity, IDamageable, IDie, IAttack, IEnemy
{

    public float pv;
    public int damage;
    public int moneyValue;
    public float attackSpeed;
    public float movementSpeed; // float meter per second
    public int shield;
    public string typeMonster;

    // Test du IEnemy

    public float radiusRange;
    public bool attackEnable = false;


    public bool isTargeted = false; // Pour que les 6 armes focus différents enemies --> J'ai des problèmes donc nique sa mère

    private bool isAttacking = false;



    private Transform target; // Player target

    [SerializeField]
    private GameObject player; // The player himself
    private Class_Perso character; // Stat of the player

    //private List<Class_Monster> enemiesinRange;


    void Start()
    {
        if (!player) // Test 
        {
            Debug.Log("Le joueur n'est pas bien initialisé");
        }
        if (!character)
        {
            Debug.Log("Les stats du joueur ne sont pas bien initialisé");
        }

        target = player.transform;
    }

    public void SetPlayer(GameObject newValue)
    {
        player = newValue;
        character = player.GetComponent<Class_Perso>();
    }


    void GoPlayer()
    {

        Vector3 direction = target.position - transform.position; // Calcul de la direction
        direction.y = 0; // Ignore l'axe Y pour éviter une inclinaison

        transform.rotation = Quaternion.LookRotation(direction);

        // Pour avancer vers la cible sans changer de hauteur
        transform.Translate(Vector3.forward * movementSpeed * Time.deltaTime);
    }

    void Update() // S'oriente vers le joueur + avance --> à traduire
    {
        if (!isAttacking || (isAttacking && !attackEnable)) // Il avance quand il n'attaque pas ou il avance quand il peut attaquer mais est en cooldown 
        {
            GoPlayer();
        }


        if (attackEnable && !isAttacking)
        {
            StartCoroutine(Attack(character)); // Lancement de la coroutine de l'attaque !
        }


    }

    public void Die() // Sera lancé en tant que test quand l'ennemi se fera toucher.
    {
        Destroy(gameObject);
        character.money += moneyValue;
        character.enemyKilled += 1;
    }

    public void TakeDamage(int damage)
    {
        pv -= damage;
        if (pv <= 0)
        {
            Die();
        }
    }
    
    public IEnumerator Attack(IDamageable character)
    {
        isAttacking = true;

        character.TakeDamage(damage);
        Debug.Log("L'ennemi attaque !");

        yield return new WaitForSeconds(1f / attackSpeed);

        Debug.Log("L'ennemi peut de nouveau attaquer !");
        isAttacking = false;
    }
}
