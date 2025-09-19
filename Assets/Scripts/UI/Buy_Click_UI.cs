using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buy_Click_UI : MonoBehaviour
{
    void Buy(int currentMoney, int cost) 
    {
        if (currentMoney >= cost) 
        {
            // The code to transfer the prefab of the gun generate randomly in a second script in the inventory 
        }
    }

    void DeleteUI()
    {
        // When we buy a gun or a bonus, we close the UI and change him to another
    }
 
    void Refresh(int currentMoney, int cost = 5)
    {
        // Refresh the shop
        if (currentMoney >= cost) 
        {
            // Refresh shop
        }
    }

    void GenerateRandomBonusOrWeapon()
    {

    }
}
