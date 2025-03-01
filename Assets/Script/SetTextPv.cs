using System.Collections;
using System.Collections.Generic;
using System.IO;
using JetBrains.Annotations;
using UnityEditor.SceneManagement;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class SetTextPv : MonoBehaviour

{
    public TextMeshProUGUI PvText; // Texte UI à assigner dans l'inspecteur
    private ClassPerso personnage;
    // Start is called before the first frame update
    private void Start()
    {
        GameObject cible = GameObject.Find("Personnage");

        personnage = cible.GetComponent<ClassPerso>();

        PvText.text = ": " + personnage.Pv.ToString();

        Debug.Log("Le texte affiché est : " + PvText.text); // Test à enlever après

    
    }
    private void Update() // Si on veux économiser de la place, faut changer ça en fonction puis l'initialiser quand on prend ou on perd des Pv
    {
        PvText.text = ": " + personnage.Pv.ToString();
    }
}
