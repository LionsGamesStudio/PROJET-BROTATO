using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class AreaWeaponEnemyDetector : MonoBehaviour
{  
    public GameObject bulletPrefab;
    private Class_Weapon weapon;

    public List<Class_Monster> enemiesInRange = new List<Class_Monster>(); 
    // Pour viser le monstre le plus proche si il meurt on va devoir faire la liste des monstres puis choisir le premier qui a été append :)
    public Class_Monster monster_locked;

    private bool shootBullet = false; // Booléen utile plus tard

    // Y'a 2 moyen de savoir si un ennemie est dans une variable, soit je créer une valeur booléenne, soit je fais un null ou !null

    void Start()
    {
        // Setup the range of the weapon with the range will be usefull later 
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        weapon = GetComponent<Class_Weapon>() ;
        sphereCollider.radius = weapon.radius_Range ; 
    }   


    void OnTriggerEnter(Collider other) 
    {   
        if (other.CompareTag("Enemy")) // Si l'ennemie touché est dans la zone
            {
                //Debug.Log("Un enemy est entré dans la zone");

                Class_Monster enemy = other.GetComponent<Class_Monster>(); // On récupère le monstre

                enemiesInRange.Add(enemy); // On l'ajoute à notre liste

                UpdateFocus(); // On met à jour le focus
        
            }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy")) // Si l'ennemie touché est dans la zone
        {   
            //Debug.Log("Un ennemie quitte la zone ou meurt !");
            
            Class_Monster enemy = other.GetComponent<Class_Monster>(); // On regarde quel enemy est sortie de la zone

            enemiesInRange.Remove(enemy);

            if (enemy == monster_locked) // On regarde si c'est notre monstre visé par l'arme
            {
                UpdateFocus(); // On met à jour notre arme
            }
            
        }
    }
    void UpdateFocus() // Gère si on prend un nouvel ennemie ou pas 
    {

        monster_locked = FindClosestEnemy(); // On prend tous les enemy et on target celui qui a la plus petite range


        // When we have our target we're gonna shot in Update
    }

    Class_Monster FindClosestEnemy()
    {
        Class_Monster closestEnemy = null;
        float minDistance = float.MaxValue; // Take the maxValue possible for a distance

        List<Class_Monster> toRemove = new List<Class_Monster>(); 

        foreach (Class_Monster enemy in enemiesInRange) // For each monster in the list
        {   
            if (enemy != null)
            {
                float distance = Vector3.Distance(enemy.transform.position, weapon.transform.position); // Searche for the minima distance
                if (distance < minDistance) 
                {
                    minDistance = distance;
                    closestEnemy = enemy;
                }
            }

            if (enemy == null)
            {   
                toRemove.Add(enemy);
            }
              
        }

        foreach (var enemy in toRemove)
        {
            enemiesInRange.Remove(enemy);
        }

        return closestEnemy;
    }


void Update()
{
    if (monster_locked == null || !monster_locked.gameObject.activeInHierarchy)
    {
        // Si l’ennemi est null ou inactif, on cherche un nouveau focus.
        UpdateFocus();
    }
    else
    {
        
        transform.LookAt(monster_locked.transform);

        if (!shootBullet)
        {
            StartCoroutine(CreateBullet(weapon.shoot_Rate));
        }
    }
}

    IEnumerator CreateBullet(float shoot_Rate) // VA FALLOIR FAIRE UN IEnumrator pour stopper le tire comme on le souhaite --> coroutine de l'attaque
    {
        {
            shootBullet = true;

            ClassBullet bullet = Instantiate(bulletPrefab,weapon.transform.position,weapon.transform.rotation).GetComponent<ClassBullet>(); // New bullet 
            bullet.damage = weapon.damage; // Hérite des dégats de l'arme
            
            yield return new WaitForSeconds(1/shoot_Rate);

            shootBullet = false;
        }
    }
}