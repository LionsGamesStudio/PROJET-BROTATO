
using UnityEngine;
using TMPro;
public class SetTextVague: MonoBehaviour

{
    public TextMeshProUGUI vagueText; // Texte UI à assigner dans l'inspecteur
    private ClassPerso personnage;
    // Start is called before the first frame update
    private void Start()
    {
        GameObject cible = GameObject.Find("Personnage");

        personnage = cible.GetComponent<ClassPerso>();

        vagueText.text = ": " + personnage.vague.ToString();

        Debug.Log("Le texte affiché est : " + vagueText.text); // Test à enlever après

    }
    private void Update()
    {
        vagueText.text = ": " + personnage.vague.ToString();
    }
}
