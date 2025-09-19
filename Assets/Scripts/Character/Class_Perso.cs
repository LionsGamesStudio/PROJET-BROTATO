using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using FluxFramework.Core;
using FluxFramework.Attributes;
using Flux;
public class Class_Perso : FluxMonoBehaviour, IPlayer
{
    [SerializeField]
    [ReactiveProperty("Player.maxHealth")]
    private float pvMax = 30f;

    [SerializeField]
    [ReactiveProperty("Player.health")]
    [FluxRange(0, 30)] 
    private float pv = 30f;
    public int money = 30;
    public int armure = 0;
    public int vague = 1; // Voir pour le bouger

    public int enemyKilled; // Voir pour le bouger

    public void EnemyKilled() // A foutre ailleurs
    {
        enemyKilled +=1;
    }

    [FluxAction("TakeDamage")]
    public void TakeDamage(int degats)
    {
        int degats_subit = degats * (100 - armure) / 100; // round in the inferior digit here

        Debug.Log("Le personnage à subit : " + degats_subit.ToString() + " Degâts");

        UpdateReactiveProperty("Player.health",pv - degats_subit);
        

        Debug.Log("Il reste " + pv.ToString() + " Pv restant");

        // See if we die
        if (pv <= 0)
        {
            Die();
        }
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
                //healthBar.SetHealth(pv); // On met à jour la barre de vie 
            }
            else
            {
                Debug.Log("Pv max atteint");
                yield break; // Stoppe immédiatement la coroutine
            }
        }
    }

    [FluxButton("Die")]

    public void Die()
    {
        // Recharger la scène actuelle
        // Faire un écran de mort à afficher
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

        // Attendre un temps
    }

    public void NextVague() // A foutre ailleurs
    {
        vague += 1;
    }

}




