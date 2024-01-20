using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static User;

public class MyAccount : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField newPassword;
    public TMP_InputField confirmNewPassword;

    public TMP_Text errorText;

    public Button changeAvatarButton;

    public Button submitButton;

    private string URL = HttpConst.CHANGE_PASSWORD.ToString();

    // Start is called before the first frame update
    void Start()
    {
        //D�sactiver les champs de saisie
        username.interactable = false;
        email.interactable = false;
        //Remplir les champs de saisie
        username.text = User.instance.user.user.name;
        email.text = User.instance.user.user.email;
        // Cacher les mots de passes de mot de passe
        password.contentType = TMP_InputField.ContentType.Password;
        newPassword.contentType = TMP_InputField.ContentType.Password;
        confirmNewPassword.contentType = TMP_InputField.ContentType.Password;
        // D�sactiver button "change avatar"
        changeAvatarButton.interactable = false;
    }

    public void OnSubmitButton() 
    {
        errorText.text = "";
        // Si les 3 champs de mot de passe sont remplis alors l'utilisateur veut changer de mot de passe
        if (isFormCorrect())
        {
            ChangePwdDTO data = new ChangePwdDTO()
            {
                actualPassword = password.text,
                newPassword = newPassword.text,
                confirmPassword = confirmNewPassword.text
            };
            var request = UnityWebRequest.Put(URL, JsonUtility.ToJson(data));
            request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
            request.SetRequestHeader("Content-Type", "application/json");
            StartCoroutine(Requests.instance.ChangePassword(request, errorText));
        }
    }

    private bool isFormCorrect()
    {
        if (password.text != "" && newPassword.text != "" && confirmNewPassword.text != "")
        {
            // Afficher un message d'erreur
            if (newPassword.text != confirmNewPassword.text)
            {
                errorText.text = "Les mots de passe ne correspondent pas";
                return false;
            }
            return true;
        }
        return false;
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
