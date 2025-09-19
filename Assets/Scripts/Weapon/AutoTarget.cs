

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoTarget : MonoBehaviour
{  
    public GameObject bulletPrefab;
    private IWeapon weapon; // Marche pour n'importe qu'elle arme


    public List<IEnemy> enemiesInRange = new List<IEnemy>(); 
    public IEnemy monsterLocked; // Marche pour n'importe quel monstre

    private Coroutine shootingCoroutine;

    void Start()
    {
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        weapon = GetComponent<IWeapon>();
        sphereCollider.radius = weapon.Radius_Range;
    }

    void OnTriggerEnter(Collider other)
    {
        MonoBehaviour enemyComponent = other.GetComponent<MonoBehaviour>();

        if (enemyComponent is IEnemy enemy)
        {
            enemiesInRange.Add(enemy);
            UpdateFocus();
        }
    }

    void OnTriggerExit(Collider other)
    {
        MonoBehaviour enemyComponent = other.GetComponent<MonoBehaviour>();

        if (enemyComponent is IEnemy enemy)
        {
            enemiesInRange.Remove(enemy);

            if (enemy == monsterLocked)
            {
                UpdateFocus();
            }
        }
    }

    void UpdateFocus()
    {
        monsterLocked = FindClosestEnemy();
    }

    IEnemy FindClosestEnemy()
    {
        IEnemy closestEnemy = null;
        float minDistance = float.MinValue;

        CleanEnemyList();

        foreach (IEnemy enemy in enemiesInRange)
        {
            Transform enemyTransform = (enemy as MonoBehaviour).transform;
            float distance = Vector3.Distance(enemyTransform.position, (weapon as MonoBehaviour).transform.position);

            if (distance < minDistance)
            {
                minDistance = distance;
                closestEnemy = enemy;
            }

        }

        return closestEnemy;
    }

    IEnemy FindFarthestEnemy()
    {
        IEnemy farthestEnemy = null;
        float maxDistance = float.MaxValue;

        CleanEnemyList();

        foreach (IEnemy enemy in enemiesInRange)
        {
            Transform enemyTransform = (enemy as MonoBehaviour).transform;
            float distance = Vector3.Distance(enemyTransform.position, (weapon as MonoBehaviour).transform.position);

            if (distance < maxDistance)
            {
                maxDistance = distance;
                farthestEnemy = enemy;
            }

        }

        return farthestEnemy;
    }

    IEnemy FindLeastHPEnemy()
    {
        IEnemy leastHpEnemy = null;
        float minHealth = float.MinValue;

        CleanEnemyList();

        foreach (IEnemy enemy in enemiesInRange)
        {

            if (enemy.Pv < minHealth)
            {
                minHealth = enemy.Pv;
                leastHpEnemy = enemy;
            }

        }

        return leastHpEnemy;
    }
    IEnemy FindHighestHPEnemy()
    {
        IEnemy highestHpEnemy = null;
        float maxHealth = float.MinValue;

        CleanEnemyList();

        foreach (IEnemy enemy in enemiesInRange)
        {

            if (enemy.Pv > maxHealth)
            {
                maxHealth = enemy.Pv;
                highestHpEnemy = enemy;
            }

        }

        return highestHpEnemy;
    }

    IEnemy FindRandomEnemy()
    {

        CleanEnemyList();
        if (enemiesInRange.Count == 0) // ✓ Vérifier si la liste n'est pas vide
        return null;

        int randomIndex = Random.Range(0, enemiesInRange.Count);
        return enemiesInRange[randomIndex];
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
        // Vérification si l'ennemi verrouillé existe toujours
        MonoBehaviour enemyMono = monsterLocked as MonoBehaviour; // Ca je vais voir

        if (monsterLocked == null || enemyMono == null || !enemyMono.gameObject.activeInHierarchy)
        {
            UpdateFocus();
        }


        else
        {
            transform.LookAt(enemyMono.transform);

            //transform.rotation = Quaternion.RotateTowards(transform.rotation,Quaternion.LookRotation(enemyMono.transform.position - transform.position),180 * Time.deltaTime ); ChatGPT


            if (weapon.CanShoot && shootingCoroutine == null)
            {
                shootingCoroutine = StartCoroutine(CreateBullet(weapon.Shoot_Rate));
            }
        }
    }

    IEnumerator CreateBullet(float shoot_Rate)
    {
        weapon.CanShoot = false;

        Projectile bullet = Instantiate(bulletPrefab, (weapon as MonoBehaviour).transform.position, (weapon as MonoBehaviour).transform.rotation).GetComponent<Projectile>();
        bullet.damage = weapon.Damage;

        yield return new WaitForSeconds(1 / shoot_Rate);

        weapon.CanShoot = true;
        shootingCoroutine = null; // libérer le flag

        yield break;
    }
}