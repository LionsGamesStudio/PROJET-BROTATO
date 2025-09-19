using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class SetTextMoney : MonoBehaviour

{
    public TextMeshProUGUI moneyText; // Texte UI à assigner dans l'inspecteur

    [SerializeField]
    private GameObject cible;
    private Class_Perso personnage; // A changer 

    // Start is called before the first frame update
    private void Start()
    {
        if (!cible)
        {
            Debug.LogError("Le joueur n'est pas bien initialisé !");
            return; // on sort si player manquant
        }
        
        personnage = cible.GetComponent<Class_Perso>();

        moneyText.text = ": " + personnage.money.ToString() + " $";

        Debug.Log("Le texte affiché est " + moneyText.text); // Test à enlever après
    }

    private void Update()
    {
        moneyText.text = ": " + personnage.money.ToString() + " $";
    }

}
