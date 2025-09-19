using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;


public class AttackAreaDetection : MonoBehaviour
{

    private IEnemy monster;
    
    void Start()
    {
        // Setup the range to don't have problem
        SphereCollider sphereCollider = GetComponent<SphereCollider>();
        monster = GetComponent<IEnemy>() ;

        //Debug.Log("Le monstre est initialisé en tant que : " + monster.name + " avec le type " + monster.GetType());
        sphereCollider.radius = monster.RadiusRange ; 
        
    }   


    void OnTriggerEnter(Collider other) 
    {
        IPlayer testPlayer = other.GetComponent<IPlayer>();
        
        if (testPlayer != null)
        {
            Debug.Log("Le joueur rentre dans la zone de collision prévue, avec le type " + testPlayer.GetType());
            UpdateAttack(testPlayer); // On met à jour le fait que l'ennemie attaque ou non
        }
    }

    void OnTriggerExit(Collider other)
    {
        IPlayer testPlayer = other.GetComponent<IPlayer>();
        
        if (testPlayer != null)
        {
            Debug.Log("Le joueur sort de la zone de collision prévue, avec le type " + testPlayer.GetType());
            UpdateAttack(null); // On met à jour le fait que l'ennemie attaque ou non
        }
        
    }
    void UpdateAttack(IPlayer player) // Gère si on prend un nouvel ennemie ou pas 
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

}