using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    public static Login instance;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public TMP_Text errorText;
    private string URL = HttpConst.LOGIN.ToString();

    private void Awake()
    {
        instance = this;
    }

    public void Fetch()
    {
        UserLoginDTO data = new UserLoginDTO()
        {
            username = usernameInput.text,
            password = passwordInput.text
        };

        UnityWebRequest request = UnityWebRequest.Post(URL, JsonUtility.ToJson(data), "application/json");
        StartCoroutine(Requests.instance.Login(request, errorText));
    }

    public void OnSignIn()
    {
        if (usernameInput.text.Length == 0 && passwordInput.text.Length == 0) return;

        Fetch();
    }

    public void ResetInputFields()
    {
        usernameInput.text = "";
        passwordInput.text = "";
    }

    public void OnExit()
    {
        Application.Quit();
    }
}