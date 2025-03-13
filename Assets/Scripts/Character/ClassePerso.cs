using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ClassPerso : MonoBehaviour
{
    public int pv = 30; 
    private int pvMax;
    public Pv_HealthBar healthBar;
    public int money = 30;
    public int armure = 0;
    public int vague = 1;

    // Start is called before the first frame update
    void Start()
    {
        pvMax = pv; //Initiate this to start the game
        healthBar.SetMaxHealth(pvMax); // Update of the health bar
        
        Debug.Log("Le nombre de point de vie du joueur est de : " + pv.ToString());
        //StartCoroutine(RegenPV(5)); // Test of the potion

        //SubirDegats_SupprimerSante(3); // Test of taking damage
       
    }

    public void SubirDegats_SupprimerSante(int degats) 
    {
        int degats_subit = degats*(100-armure)/100 ; // round in the inferior digit here
        
        Debug.Log("Le personnage à subit : " + degats_subit.ToString() +" Degâts");

        pv -= degats_subit;

        Debug.Log("Il reste "+ pv.ToString() + " Pv restant");

        // Update of HP 

        // See if we die

        Die();

        // Update of the health bar

        healthBar.SetHealth(pv);  
    }

    IEnumerator RegenPV(int decompte) // Futur function for potion --> Change place
    {
        while (decompte > 0) 
        {
            if (pv < 30) 
            {
                yield return new WaitForSeconds(1f);
                pv += 1;
                decompte -= 1;
                Debug.Log("PV : " + pv);

            // Mise à jour des PV --> Test avec la barre de vie 
                healthBar.SetHealth(pv); // On met à jour la barre de vie 
            }
            else
            {
                Debug.Log("Pv max atteint");
                yield break; // Stoppe immédiatement la coroutine
            }
        }
    }   
    void Die()
    {
        // Recharger la scène actuelle
        if (pv <= 0) 
        {
            // Faire un écran de mort à afficher
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            // Attendre un temps
        }
        
    }

    public void NextVague() 
    {
        vague += 1;
    }

}




