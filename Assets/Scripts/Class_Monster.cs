using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Class_Monster : MonoBehaviour
{

    public int Pv;
    public float vitesse_de_deplacement;
    public int Armure;
    public string type_Monster;

    public Transform target; // Assigner le joueur afin que les monstres s'oriente vers lui

    
    void Update() // S'oriente vers le joueur + avance
    {
        transform.LookAt(target);
        transform.Translate(Vector3.forward * vitesse_de_deplacement * Time.deltaTime); // Avance vers le point defini de meme hauteur que les monstres
    }
}
