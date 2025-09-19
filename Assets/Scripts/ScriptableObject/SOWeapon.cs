using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[CreateAssetMenu(fileName = "Weapon", menuName = "ScriptableObject/Weapon")]
public class SOWeapon : ScriptableObject
{
    public string name_Weapon; // Type d'arme 
    public int damage; // Dégats de l'arme
    public float shoot_Rate; // Vitesse d'attaque par seconde
    public float radius_Range; // Rayon dans lequel on tire 
    public int position; // Position dans l'inventaire 
    public string munition_type; // Type de munition que l'arme va créer
    public string rarity; // Rareté de l'arme changeant ses stats de base 

}
