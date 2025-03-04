using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Actif_Inactif_Inventory : MonoBehaviour
{
    public TMP_Text buttonText; // Bouton à assigner
    public GameObject targetObject; // Inventaire UI à assigner  
    // Start is called before the first frame update
    void Start()
    {
        buttonText.text = "Actif";
        buttonText.color = Color.green;
        targetObject.SetActive(true);
    }

    // Update is called once per frame
    public void Actif_Inactif()
    {
        if (buttonText.text == "Actif") 
        {
            buttonText.text = "Inactif";
            buttonText.color = Color.red;
            targetObject.SetActive(false);

            Debug.Log("Changement du texte + couleur (Actif --> Inactif)");
        }

        else 
        {
            buttonText.text = "Actif";
            buttonText.color = Color.green;
            targetObject.SetActive(true);

            Debug.Log("Changement du texte + couleur (Inactif --> Actif)");
        }


    }
}
