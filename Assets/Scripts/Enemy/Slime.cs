using System.Collections;
using UnityEngine;

public class Slime : MonoBehaviour, IEntity, IDamageable, IDie, IAttack, IEnemy
{
    public float pv;
    public int damage;
    public int moneyValue;
    public float attackSpeed;
    public float movementSpeed;
    public int shield;
    public string typeMonster;

    // Test du IEnemy
    public float radiusRange;
    public bool attackEnable = false;

    // ✅ Propriétés IEnemy sécurisées
    public Transform Transform => this ? transform : null; // Je suis obligé d'avoir cela pour AutoTarget1
    public GameObject GameObject => this ? gameObject : null;
    public bool IsDestroyed => this == null; 

    private bool isAttacking = false;

    [SerializeField]
    private GameObject player;
    private Transform target;
    private Class_Perso character;

    void Start()
    {
        if (!player)
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
        Vector3 direction = target.position - transform.position;
        direction.y = 0;

        transform.rotation = Quaternion.LookRotation(direction);
        transform.Translate(movementSpeed * Time.deltaTime * Vector3.forward);
    }

    void Update()
    {
        if (!isAttacking || (isAttacking && !attackEnable))
        {
            GoPlayer();
        }

        if (attackEnable && !isAttacking)
        {
            StartCoroutine(Attack(character));
        }
    }

    public void Die()
    {
        character.money += moneyValue;
        character.enemyKilled += 1;
        Destroy(gameObject);
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