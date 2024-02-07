using System.Collections;
using System.Collections.Generic;
using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using static UnityEngine.GraphicsBuffer;
using static User;

public class MyAccount : MonoBehaviour
{
    public TMP_InputField username;
    public TMP_InputField email;
    public TMP_InputField password;
    public TMP_InputField newPassword;
    public TMP_InputField confirmNewPassword;
    public RawImage avatar;

    public TMP_Text errorText;

    public Button changeAvatarButton;

    public Button submitButton;


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

        // If exists img convert it from base64 to Texture2D
        if (User.instance.user.user.image.Length != 0)
        {
            avatar.texture = ImageTools.CreateTextureFromString(User.instance.user.user.image);
        }
    }

    public void OnChangeAvanatr()
    {
        // Open file window to import files from system
        ImageTools.OpenFileExplorer();
        if (ImageTools.path != null)
        {
            StartCoroutine(ImageTools.GetTexture(UpdateImage));
        }
    }

    public void UpdateImage()
    {
        // Convert img from Texture2D to base64
        User.instance.user.user.image = Convert.ToBase64String(ImageTools.texture);

        // Show the imported img to the box
        avatar.texture = ImageTools.CreateTextureFromString(Convert.ToBase64String(ImageTools.texture));

        // Update the user in DB
        var request = UnityWebRequest.Put(HttpConst.UPDATE_USER.Value, JsonUtility.ToJson(User.instance.user.user));
        request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
        request.SetRequestHeader("Content-Type", "application/json");
        StartCoroutine(Requests.instance.Me(request));
    }

    public void OnSubmitButton() 
    {
        errorText.text = "";
        // Si les 3 champs de mot de passe sont remplis alors l'utilisateur veut changer de mot de passe
        if (IsFormCorrect())
        {
            ChangePwdDTO data = new()
            {
                actualPassword = password.text,
                newPassword = newPassword.text,
                confirmPassword = confirmNewPassword.text
            };
            var request = UnityWebRequest.Put(HttpConst.CHANGE_PASSWORD.Value, JsonUtility.ToJson(data));
            request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
            request.SetRequestHeader("Content-Type", "application/json");
            StartCoroutine(Requests.instance.ChangePassword(request, errorText));
        }
    }

    private bool IsFormCorrect()
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
