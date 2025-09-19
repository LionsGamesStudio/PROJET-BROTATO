
using UnityEngine;
using TMPro;
public class SetTextVague: MonoBehaviour

{
    public TextMeshProUGUI vagueText; // Texte UI à assigner dans l'inspecteur
    
    [SerializeField]
    private GameObject cible;
    private Class_Perso personnage;
    // Start is called before the first frame update
    private void Start()
    {
        if (!cible)
        {
            Debug.Log("Le joueur est mal assignée à la main");
        }

        personnage = cible.GetComponent<Class_Perso>();

        vagueText.text = ": " + personnage.vague.ToString();

        Debug.Log("Le texte affiché est " + vagueText.text); // Test à enlever après

    }
    private void Update()
    {
        vagueText.text = ": " + personnage.vague.ToString();
    }
}
