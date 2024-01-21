using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Register : MonoBehaviour
{

    public static Register instance;
    public TMP_InputField nameInput;
    public TMP_InputField passwordInput;
    public TMP_InputField usernameInput;
    public TMP_InputField emailInput;
    public TMP_Text errorText;
    private string URL = HttpConst.REGISTER.Value;

    private void Awake()
    {
        instance = this;
    }

    public void Fetch()
    {
        UserRegisterDTO data = new UserRegisterDTO()
        {
            username = usernameInput.text,
            password = passwordInput.text,
            name = nameInput.text,
            email = emailInput.text

        };
        UnityWebRequest request = UnityWebRequest.Post(URL, JsonUtility.ToJson(data), "application/json");
        StartCoroutine(Requests.instance.Signup(request, errorText));
    }

    public void OnSignUp()
    {
        if (usernameInput.text.Length == 0 && passwordInput.text.Length == 0) return; //verifier tout les champs

        Fetch();
    }
}

