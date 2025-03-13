using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;

public class MonsterAreaDetection : MonoBehaviour
{

    private Class_Monster monster;
    private ClassPerso player;
    
    void Start()
    {
        // Setup the range to don't have problem
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        monster = GetComponent<Class_Monster>() ;
        sphereCollider.radius = monster.radiusRange ; 
        
    }   


    void OnTriggerEnter(Collider other) 
    {   
        if (other.CompareTag("Player")) // Si l'objet touché est un joueur
            {
                Debug.Log("TEST 1 !");

                player = other.GetComponent<ClassPerso>();

                UpdateAttack(); // On met à jour le fait que l'ennemie attaque ou non
        
            }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) // Si l'ennemie touché est dans la zone
        {   
            Debug.Log("TEST 2");

            player = null;

            UpdateAttack();  
    
        }
        
    }
    void UpdateAttack() // Gère si on prend un nouvel ennemie ou pas 
    {
        if (player != null) 
        {
            monster.AttackEnable = true;
            Debug.Log("Maj de la possibilité d'attaquer --> true");
        }

        else 
        {
            monster.AttackEnable = false;
            Debug.Log("Maj de la possibilité d'attaquer --> false");
        }
    }

    


// Gerer si l'ennemie meurt à l'avance

}