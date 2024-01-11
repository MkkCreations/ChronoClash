using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MyAccount : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField newPassword;
    public TMP_InputField confirmNewPassword;

    public Button changeAvatarButton;

    public Button submitButton;

    // Start is called before the first frame update
    void Start()
    {
        //Désactiver les champs de saisie
        username.interactable = false;
        email.interactable = false;
        //Remplir les champs de saisie
        username.text = User.instance.user.user.name;
        email.text = User.instance.user.user.email;
        // Cacher les mots de passes de mot de passe
        password.contentType = TMP_InputField.ContentType.Password;
        newPassword.contentType = TMP_InputField.ContentType.Password;
        confirmNewPassword.contentType = TMP_InputField.ContentType.Password;
        // Désactiver button "change avatar"
        changeAvatarButton.interactable = false;
    }

    public void OnSubmitButton() 
    { 
        // Si les 3 champs de mot de passe sont remplis alors l'utilisateur veut changer de mot de passe
        if (password.text != "" && newPassword.text != "" && confirmNewPassword.text != "")
        {
            // Si le nouveau mot de passe et la confirmation sont identiques
            if (newPassword.text == confirmNewPassword.text)
            {
                // Si le mot de passe actuel est correct
                if (false) { }
                else { Debug.Log("Mot de passe actuel incorrect"); }
            } else {
                // Afficher un message d'erreur
                Debug.Log("Les mots de passe ne correspondent pas");
            }
        }
        this.Reset();
        // ferme le panel
        this.gameObject.SetActive(false);
    }

    private void Reset()
    {
        username.text = "";
        email.text = "";
        password.text = "";
        newPassword.text = "";
        confirmNewPassword.text = "";
    }

    public void LoadData() {
        username.text = User.instance.user.user.name;
        email.text = User.instance.user.user.email;
    }
}
