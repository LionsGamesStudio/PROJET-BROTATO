using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Pv_HealthBar : MonoBehaviour
{

    public Slider slider; // Gérer avec l'interface de unity 

    public void SetMaxHealth(int maxHealth)  // Quand le jeu commnence
    {
        slider.maxValue = maxHealth;
        slider.value = maxHealth;
    }

    public void SetHealth(int health) // Quand on prends des dégats 
    {
        slider.value = health;
    }
}
