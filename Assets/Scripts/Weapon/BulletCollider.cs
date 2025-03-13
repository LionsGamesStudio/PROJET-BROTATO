using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ClassBullet : MonoBehaviour // ON BULLET
{
    public int damage;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,3f); // Le 5F correspond au cooldown
    }

    void DamageEnemy(Class_Monster enemy,int degats) 
    {
        enemy.pv -= degats;
        Debug.Log("L'ennemie a pris une balle et à subit "+degats+" dégats !");
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            Class_Monster enemy = other.GetComponent<Class_Monster>();

            DamageEnemy(enemy,damage);

            enemy.TestDie();

            Destroy(gameObject);
        }
    }


    void Update()
    {
        // Notre tir avance de tant de vitesse donnée par Time.deltatime 
        transform.Translate(Vector3.forward * 30 * Time.deltaTime); // Changer la valeur du tir
    }
}
