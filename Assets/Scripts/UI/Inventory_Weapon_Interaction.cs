using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Inventory_Weapon_Interaction : MonoBehaviour
{
    // Start is called before the first frame update

    // I will setup this script on Set(x) --> try to get weapon --> After i will send the information to the UI inventory 

    private GameObject weapon;
    private int nombreSet;  
    void Start()
    {
        weapon = GetComponentInChildren<GameObject>();

        if (HaveWeapon(weapon))
        {
            nombreSet = weapon.GetComponentInChildren<Class_Weapon>().position; // On récupère me numéro du set 
            // //--> Gérer ça au préable directement dans l'arme quand on l'ajoute


            // Envoie l'information au UI associé au numéro du set à la zone 

        

        }
    }

    // Update is called once per frame
    bool HaveWeapon(GameObject weapon)
    {
        return weapon != null;
    }


}
