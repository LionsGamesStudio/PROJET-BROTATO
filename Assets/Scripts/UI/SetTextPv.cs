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
    public TextMeshProUGUI pvText; // Texte UI à assigner dans l'inspecteur
    private ClassPerso personnage;
    // Start is called before the first frame update
    private void Start()
    {
        GameObject cible = GameObject.Find("-- XR Origin");

        personnage = cible.GetComponent<ClassPerso>();

        pvText.text = ": " + personnage.pv.ToString();

        Debug.Log("Le texte affiché est " + pvText.text); // Test à enlever après

    
    }
    private void Update() // Si on veux économiser de la place, faut changer ça en fonction puis l'initialiser quand on prend ou on perd des Pv
    {
        pvText.text = ": " + personnage.pv.ToString();
    }
}
