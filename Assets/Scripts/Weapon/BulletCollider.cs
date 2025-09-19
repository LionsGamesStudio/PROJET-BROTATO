using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Projectile : MonoBehaviour // ON BULLET
{
    public int damage; // A changer

    // Start is called before the first frame update
    void Start()
    {
        Destroy(gameObject,3f); // Le xf correspond au cooldown
    }


    void Update()
    {
        // Notre tir avance de tant de vitesse donnée par Time.deltatime 
        transform.Translate(Vector3.forward * 30 * Time.deltaTime); // Changer la valeur du tir
    }

    void OnTriggerEnter(Collider other)
    {
        IEnemy enemy = other.GetComponent<IEnemy>();

        if (enemy != null)
        {
            StartCoroutine(Attack(enemy));
            
            if (enemy.Pv <= 0)
            {
                enemy.Die();
            }

            Destroy(gameObject);
        }
    }

    public IEnumerator Attack(IDamageable enemy)
    {
        enemy.TakeDamage(damage);
        yield break;
    }



}
