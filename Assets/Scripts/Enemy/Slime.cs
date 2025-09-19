using System.Collections;
using UnityEngine;
using Events;

public class Slime : MonoBehaviour, IEnemy
{
    public int damage;
    public int moneyValue;
    public float attackSpeed;
    public float movementSpeed;
    public int shield;
    public string typeMonster;

    // Pour le IEnemy
    public float radiusRange;

    public float RadiusRange
    {
        get { return radiusRange; }
        set { radiusRange = value; }
    }

    public bool attackEnable = false; 

    public bool AttackEnable
    {
        get { return attackEnable; }
        set { attackEnable = value; }
    }

    public float pv;

    public float Pv
    {
        get { return pv; }
        set { pv = value; }
    }
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

        MonoBehaviour mb = character as MonoBehaviour;
        if (mb != null)
        {
            FluxFramework.Core.Flux.Manager.EventBus.Publish(new GetDamageEvent(damage, transform.position));
        }

        character.TakeDamage(damage);
        Debug.Log("L'ennemi attaque !");

        yield return new WaitForSeconds(1f / attackSpeed);

        Debug.Log("L'ennemi peut de nouveau attaquer !");
        isAttacking = false;
    }
}