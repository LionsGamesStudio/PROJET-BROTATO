using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ClassPerso : MonoBehaviour
{
    public int Pv = 26; // Public si on donne des données ça va pas être remis comme à la base si on les changes
    public int PvMax = 30;
    public Pv_HealthBar healthBar;
    public int money = 30;
    public int armure = 0;
    public int vague = 1;

    // Start is called before the first frame update
    void Start()
    {
        //PvMax = Pv; Test de la barre de vie
        healthBar.SetMaxHealth(PvMax); // On met à jour la barre de vie 
        
        Debug.Log("Le nombre de point de vie du joueur est de : " + Pv.ToString());
        StartCoroutine(RegenPV(5)); // Test de la potion

        SubirDegats_SupprimerSante(3); // Test de subir un projectile 
       
    }

    void SubirDegats_SupprimerSante(int degats) // PLus tard on pourra faire, si l'ennemi ou autre chose me touche - x dégats + IE à faire pour le coroutine
    {
        int degats_subit = degats*(100-armure)/100 ; //  Arrondi à l'entier inférieur --> A revoir pour être plus précis fonction ?
        
        Debug.Log("Le personnage à subit : " + degats_subit.ToString() +" Degâts");

        Pv -= degats_subit;

        Debug.Log("Il reste "+ Pv.ToString() + " Pv restant");

        // Mise à jour des PV --> Test avec la barre de vie 

        healthBar.SetHealth(Pv); // On met à jour la barre de vie 
    }

    IEnumerator RegenPV(int decompte) // Future fonction que l'on va mettre dans la potion ? 
{
    while (decompte > 0) 
    {
        if (Pv < 30) 
        {
            yield return new WaitForSeconds(1f);
            Pv += 1;
            decompte -= 1;
            Debug.Log("PV : " + Pv);

            // Mise à jour des PV --> Test avec la barre de vie 
            healthBar.SetHealth(Pv); // On met à jour la barre de vie 
        }
        else
        {
            Debug.Log("Pv max atteint");
            yield break; // Stoppe immédiatement la coroutine
        }
    }
}



}
