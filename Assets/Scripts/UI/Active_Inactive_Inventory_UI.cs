using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class Active_Inactive_Inventory_UI : MonoBehaviour
{
    public TMP_Text buttonText; // It goes on Inventory UI button OnClick() directly
    public GameObject targetObject; 

    // Start is called before the first frame update
    void Start()
    {
        buttonText.text = "Actif";
        buttonText.color = Color.green;
        targetObject.SetActive(true);
    }

    // Update is called once per frame
    public void Active_Inactive()
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
