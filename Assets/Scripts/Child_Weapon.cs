using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.Tracing;
using JetBrains.Annotations;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class Child_Weapon : Class_Weapon

    
{
    public Transform target; // Assigner le monstre 
    // Start is called before the first frame update
    void Shoot() // Tire un tir en direction de l'ennemi compris dans la range
    {
        
    }

    void FindTarget() // Faut gérer ça avec des tags
    {
        // Regarde en direction du monstre le plus proche
        //target = GetComponent<Class_Monster>().transform ; // On recupère la postion d'un ennemie jsp si ça marche a test
        transform.LookAt(target.position);
    }

    void Update()
    {
        FindTarget();
    }


}
