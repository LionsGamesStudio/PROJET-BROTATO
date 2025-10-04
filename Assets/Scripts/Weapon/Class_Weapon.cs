// using System.Collections;
// using System.Collections.Generic;
// using System.Diagnostics.Tracing;
// using JetBrains.Annotations;
// using Unity.VisualScripting;
// using UnityEngine;
// using UnityEngine.Rendering;

// public class Class_Weapon : MonoBehaviour, IWeapon


// {
//     [SerializeField]
//     public string name_Weapon; // Type d'arme 



//     public int position; // Position dans l'inventaire 
//     public string munition_type; // Type de munition que l'arme va créer
//     public string rarity; // Rareté de l'arme changeant ses stats de base 

//     [SerializeField]
//     private bool canShoot = true; // Utilisé pour savoir si l'on peux tirer
//     public bool CanShoot
//     {
//         get { return canShoot; }
//         set { canShoot = value; }
//     }

//     public float radius_Range; // Rayon dans lequel on tire 
//     public float Radius_Range
//     { 
//         get { return radius_Range; }
//         set { radius_Range = value; }
//     }

//     public float shoot_Rate; // Vitesse d'attaque par seconde

//     public float Shoot_Rate // Vitesse d'attaque par seconde
//     { // Rayon dans lequel on tire 
//         get { return shoot_Rate; }
//         set { shoot_Rate = value; }
//     }
//     public int damage; // Dégats de l'arme
    
//     public int Damage
//     { 
//         get { return damage; }
//         set { damage = value; }
//     }

// }
