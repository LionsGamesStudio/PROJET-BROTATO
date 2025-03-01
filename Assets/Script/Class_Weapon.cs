using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using UnityEngine;

public class Class_Weapon : MonoBehaviour
{
    public string nom_Arme; // Type d'arme 
    public float damage; // Dégats de l'arme
    public float shoot_Rate; // Vitesse d'attaque par seconde
    public float radius_Range; // Rayon dans lequel on tire 
    public int position; // Position dans l'inventaire 
    public string munition_type; // Type de munition que l'arme va créer
    public string rarity; // Rareté de l'arme changeant ses stats de base 

    // Start is called before the first frame update
    private void Start()
    {
    
    }

    // Update is called once per frame
    private void Update()
    {
        
    }
}
