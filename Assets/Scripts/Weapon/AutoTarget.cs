using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;

public class AutoTarget : MonoBehaviour
{  
    public GameObject bulletPrefab;
    private Class_Weapon weapon;

    public List<Slime> enemiesInRange = new List<Slime>(); 
    // Pour viser le monstre le plus proche si il meurt on va devoir faire la liste des monstres puis choisir le premier qui a été append :)
    public Slime monster_locked;

    private bool shootBullet = false; // Booléen utile plus tard



    void Start()
    {
        // Setup the range of the weapon with the range will be usefull later 
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        weapon = GetComponent<Class_Weapon>() ;
        sphereCollider.radius = weapon.radius_Range ; 
    }


    void OnTriggerEnter(Collider other)
    {
        Slime enemy = other.GetComponent<Slime>(); // On regarde quel enemy est sortie de la zone


        if (enemy != null)
        {
            //Debug.Log("Un enemy est entré dans la zone");

            enemiesInRange.Add(enemy); // On l'ajoute à notre liste

            UpdateFocus(); // On met à jour le focus

        }
        
    }

    void OnTriggerExit(Collider other)
    {
        Slime enemy = other.GetComponent<Slime>(); // On regarde quel enemy est sortie de la zone

        if (enemy != null)
        {
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

    Slime FindClosestEnemy()
    {
        Slime closestEnemy = null;
        float minDistance = float.MaxValue; // Take the maxValue possible for a distance

        List<Slime> toRemove = new List<Slime>(); 

        foreach (Slime enemy in enemiesInRange) // For each monster in the list
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
    
    else // Cette partie la je pense la mettre ailleurs
    {
        transform.LookAt(monster_locked.transform);

        if (!shootBullet)
        {
            // StartCoroutine(CreateBullet(weapon.shoot_Rate));
        }
    }
}

    // IEnumerator CreateBullet(float shoot_Rate)
    // {
    //     shootBullet = true;

    //     Projectile bullet = Instantiate(bulletPrefab,weapon.transform.position,weapon.transform.rotation).GetComponent<Projectile>(); // New bullet 
    //     bullet.damage = weapon.damage; // Hérite des dégats de l'arme

    //     yield return new WaitForSeconds(1 / shoot_Rate);

    //     shootBullet = false;
    // }
}