

using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class AutoTarget : MonoBehaviour
{

    [Header("Targeting System")]
    [SerializeField] private TargetingMode currentTargetingMode = TargetingMode.Closest;
    public enum TargetingMode
    {
        Closest,
        Farthest,
        HighestHP,
        LowestHP,
        Random
    }

    [Space(10)]
    [Header("To Instantiate in Hand")]
    public GameObject bulletPrefab;

    // Interfaces 
    private IWeapon weapon; // Marche pour n'importe qu'elle arme

    private ITargetingStrategy currentStrategy;


    public List<IEnemy> enemiesInRange = new List<IEnemy>();
    public IEnemy monsterLocked; // Marche pour n'importe quel monstre

    private Coroutine shootingCoroutine;



    void Start()
    {
        InitializeComponent();
        CreateStrategy();
    }

    void InitializeComponent()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        weapon = GetComponent<IWeapon>();
        sphereCollider.radius = weapon.Radius_Range;
    }

    private void CreateStrategy()
    {
        currentStrategy = currentTargetingMode switch
        {
            TargetingMode.Closest => new ClosestEnemyStrategy(weapon),
            TargetingMode.Farthest => new FarthestEnemyStrategy(weapon),
            TargetingMode.HighestHP => new HighestHPEnemyStrategy(),
            TargetingMode.LowestHP => new LowestHPEnemyStrategy(),
            TargetingMode.Random => new RandomEnemyStrategy(),
            _ => new ClosestEnemyStrategy(weapon),
        };
    }

    public void SetTargetingMode(TargetingMode newMode)
    {
        if (currentTargetingMode != newMode)
        {
            currentTargetingMode = newMode;
            CreateStrategy();
        }
    }

    [ContextMenu("Set Closest")]
    public void SetClosest() { SetTargetingMode(TargetingMode.Closest); }

    [ContextMenu("Set Farthest")]
    public void SetFarthest() { SetTargetingMode(TargetingMode.Farthest); }

    [ContextMenu("Set Highest HP")]
    public void SetHighestHP() { SetTargetingMode(TargetingMode.HighestHP); }

    [ContextMenu("Set Lowest HP")]
    public void SetLowestHP() { SetTargetingMode(TargetingMode.LowestHP); }

    [ContextMenu("Set Random")]
    public void SetRandom() { SetTargetingMode(TargetingMode.Random); }

    void OnTriggerEnter(Collider other)
    {
        IEnemy enemy = other.GetComponent<IEnemy>();

        if (enemy != null)
        {
            enemiesInRange.Add(enemy);
        }
    }

    void OnTriggerExit(Collider other)
    {
        IEnemy enemy = other.GetComponent<IEnemy>();

        if (enemy != null)
        {
            enemiesInRange.Remove(enemy);
        }
    }

    private void CleanEnemyList()
    {
        enemiesInRange.RemoveAll(enemy => !IsValidEnemy(enemy));
    }
    private bool IsValidEnemy(IEnemy enemy)
    {
        if (enemy == null) return false;

        MonoBehaviour enemyMB = enemy as MonoBehaviour;
        return enemyMB != null && enemyMB.gameObject != null && enemyMB.gameObject.activeInHierarchy;
    }

    void Update()
    {
        CleanEnemyList(); // Modifier avec l'event EnemyDieEvent !

        if (monsterLocked == null || !IsValidEnemy(monsterLocked)) // Pour éviter des reTarget à chaque fois qu'un monstre rentre dans la zone.
        {
            monsterLocked = currentStrategy.SelectTarget(enemiesInRange);
        }

        HandleShooting();
    }

    private void HandleShooting()
    {
        MonoBehaviour enemyMono = monsterLocked as MonoBehaviour;

        if (monsterLocked != null && enemyMono != null && enemyMono.gameObject.activeInHierarchy)
        {
            transform.LookAt(enemyMono.transform);

            //transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(enemyMono.transform.position - transform.position),180 * Time.deltaTime ); ChatGPT

            if (weapon.CanShoot && shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(Shoot(weapon.Shoot_Rate));
            }
        }
    }


    IEnumerator Shoot(float shoot_Rate)
    {
        weapon.CanShoot = false;

        Transform weaponTransform = (weapon as MonoBehaviour).transform;

        Projectile bullet = Instantiate(bulletPrefab, weaponTransform.position, weaponTransform.rotation).GetComponent<Projectile>();
        bullet.damage = weapon.Damage;

        yield return new WaitForSeconds(1 / shoot_Rate);

        weapon.CanShoot = true;
        shootingCoroutine = null; // libérer le flag

        yield break;
    }
    

}