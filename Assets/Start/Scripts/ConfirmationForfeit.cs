using UnityEngine;
using UnityEngine.UI;

public class ConfirmationPanel : MonoBehaviour
{
    public GameObject confirmationPanel; // Panneau de confirmation dans l'inspecteur Unity
    public static bool isConfirmationActive = false; // Variable pour contrôler l'état de confirmation

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isConfirmationActive)
            {
                CancelConfirmation();
            }
            else
            {
                ShowConfirmationPanel();
            }
        }
    }

    void Start()
    {
        // Assure-toi que le panneau de confirmation est désactivé au démarrage
        confirmationPanel.SetActive(false);
    }

    // Cette fonction sera appelée lorsque tu cliques sur le bouton "Abandonner"
    public void ShowConfirmationPanel()
    {
        // Activer le panneau de confirmation pour demander la confirmation
        confirmationPanel.SetActive(true);
        isConfirmationActive = true; // Marquer la confirmation comme active
    }

    // Cette fonction sera appelée lorsque tu cliques sur "Annuler" dans le panneau de confirmation
    public void CancelConfirmation()
    {
        // Désactiver le panneau de confirmation sans abandonner
        confirmationPanel.SetActive(false);
        isConfirmationActive = false; // Marquer la confirmation comme inactive
    }

    // Cette fonction sera appelée lorsque tu cliques sur "Abandonner" dans le panneau de confirmation
    public void ConfirmAbandon()
    {
        // Mettre ici le code pour abandonner ou quitter la partie
        Debug.Log("Abandonner la partie...");

        // Désactiver le panneau de confirmation après avoir abandonné
        confirmationPanel.SetActive(false);
        isConfirmationActive = false; // Marquer la confirmation comme inactive après abandon
    }
}
