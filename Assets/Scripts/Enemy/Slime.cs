using System.Collections;
using UnityEngine;
using Events;

public class Slime : MonoBehaviour, IEnemy
{
    [Header("Data to give")]
    [SerializeField]
    private SOMonster sOMonster; // Le seul truc à mettre au final

    //[SerializeField]
    // private WaveManagement manager;
    // Pour le IEnemy

    public float RadiusRange
    { get => sOMonster ? sOMonster.RadiusRange : 0f; set { } }

    public bool currentAttackEnable = false; // Will change
    public bool AttackEnable { get => currentAttackEnable; set => currentAttackEnable = value;  }
    
    public float currentPv; // Pv will change
    public float Pv { get => currentPv; set { currentPv = value; } }

    private bool currentIsAttacking = false; // Will change
    public bool IsAttacking { get => currentIsAttacking; set => currentIsAttacking = value; }

    // Le restant des données du SOMonster
    public int Damage => sOMonster ? sOMonster.Damage : 0;
    public int MoneyValue => sOMonster ? sOMonster.MoneyValue : 0;
    public float AttackSpeed => sOMonster ? sOMonster.AttackSpeed : 1f;
    public float MovementSpeed => sOMonster ? sOMonster.MovementSpeed : 1f;
    public int Shield => sOMonster ? sOMonster.Shield : 0;


    [SerializeField]
    private GameObject player;
    private Transform target;
    private Class_Perso character; // A modifier avec mon IEnemy

    void Start()
    {
        if (!player)
        {
            Debug.Log("The player isn't existing");
        }
        if (!character)
        {
            Debug.Log("The player stats aren't existing");
        }

        target = player.transform;

        if (sOMonster != null)
        {
            currentPv = sOMonster.Pv;
        }
        else
        {
            Debug.Log("Error from SOMonster for enemy of type Slime");
        }
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
        transform.Translate(MovementSpeed * Time.deltaTime * Vector3.forward);
    }

    void Update()
    {
        if (!IsAttacking || (IsAttacking && !AttackEnable))
        {
            GoPlayer();
        }

        if (AttackEnable && !IsAttacking)
        {
            StartCoroutine(Attack(character));
        }
    }

    public void Die()
    {
        // Faire un event qu'on est obligé de rajouter pour chaque enemy !

        character.money += MoneyValue;
        character.enemyKilled += 1;

        Destroy(gameObject);
    }

    public void TakeDamage(int damage)
    {
        Pv -= damage;
        if (Pv <= 0)
        {
            Die();
        }
    }
    
    public IEnumerator Attack(IDamageable character)
    {
        IsAttacking = true;

        MonoBehaviour mb = character as MonoBehaviour;
        if (mb != null)
        {
            FluxFramework.Core.Flux.Manager.EventBus.Publish(new GetDamageEvent(Damage, transform.position));
        }

        character.TakeDamage(Damage);
        Debug.Log("L'ennemi attaque !");

        yield return new WaitForSeconds(1f / AttackSpeed);

        Debug.Log("L'ennemi peut de nouveau attaquer !");
        IsAttacking = false;
    }
}