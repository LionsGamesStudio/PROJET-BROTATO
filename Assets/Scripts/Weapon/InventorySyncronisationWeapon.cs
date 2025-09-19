using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.XR.CoreUtils;
using UnityEngine;

public class InventorySyncronisationWeapon : MonoBehaviour
{
    // This thing does 1 think --> Look at the weapon of the player and add the associate image to the appropiate slot in inventory

    public List<GameObject> possibleWeapon = new List<GameObject>(); // Prefab of all image weapon
    // Start is called before the first frame update
    public List<Class_Weapon> actualWeapon = new List<Class_Weapon>();
    // Update is called once per frame

    [SerializeField]
    public Transform weapons; // Drag le GameObject "Armes 3D - Joueur" ici
    private GameObject[] sets;

    void Start()
    {   
        // Gestion of the 6 emplacements of player's weapons
        sets = new GameObject[weapons.childCount];
        for (int i = 0; i < weapons.childCount; i++)
        {
            sets[i] = weapons.GetChild(i).gameObject;
            // Debug.Log(sets[i].name);
        }


        actualWeapon.Add(sets[1].GetComponentInChildren<Class_Weapon>()); // Get the first weapon at the start of the game
        for (int i = 0; i < 5; i++)
        {
            actualWeapon.Add(null); // To have the six slot of weapon
        }
        Syncronisation(); // Syncronisation of the weapon
    }

    public void GetWeaponAndSyncronise() 
    {
        actualWeapon = new List<Class_Weapon>(); // Reset de la liste
        for (int i = 0; i < 6; i++)
        {
            actualWeapon.Add(sets[i].GetComponentInChildren<Class_Weapon>());
        } 

        //  return actualWeapon;
    }

    public void Syncronisation()
    {
        int index = 1;
        GameObject image;
        foreach (var weapon in actualWeapon)
        {
            if (weapon != null)
            {
                image = GetImageForWeapon(weapon);
                if (image != null)
                {
                    GetImageSyncronised(index, image);
                }
            }
            index += 1;
        }
    }

    private GameObject GetImageForWeapon(Class_Weapon weapon) 
    {
        if (weapon.name_Weapon == "Pistol")
        {
            Debug.Log("Récupération de l'image du pistolet");
            return possibleWeapon[0];
        }
        
        Debug.Log("Problème de syncronisation image/weapon");
        return null;
    }

    private void GetImageSyncronised(int index, GameObject image)
    {
        // Put the image in the associate slot in the inventory
        var armeTransform = GameObject.Find($"Arme ({index})").transform; // Zeub
        var imageWeapon = Instantiate(image, armeTransform); 
        imageWeapon.transform.localPosition = Vector3.zero; 
    }
}
