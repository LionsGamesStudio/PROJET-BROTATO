using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pv_HealthBar : MonoBehaviour
{

    public Slider slider; // Gérer avec l'interface de unity 

    public void SetMaxHealth(int maxHealth)  // When the game start
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth(int health) // When we take damage
    {
        slider.value = health;
    }
}
