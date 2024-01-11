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
    public string URL;

    [System.Serializable]
    public class UserRegister
    {
        public string name;
        public string username;
        public string password;
        public string email;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        URL = "http://127.0.0.1:8081/api/auth/signup";
    }

    public void GetData()
    {
        StartCoroutine(FetchData());
    }

    public IEnumerator FetchData()
    {
        UserRegister data = new UserRegister();
        data.username = usernameInput.text;
        data.password = passwordInput.text;
        data.name = nameInput.text;
        data.email = emailInput.text;

        Debug.Log(JsonUtility.ToJson(data).ToString());

        using (UnityWebRequest request = UnityWebRequest.Post(URL, JsonUtility.ToJson(data), "application/json"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                ErrorResponse error = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
                errorText.text = error.error;
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                User.UserData userResponse = JsonUtility.FromJson<User.UserData>(request.downloadHandler.text);
                User.instance.user = userResponse;
                User.instance.logedIn = true;

                PanelManager.instance.GoHome();
            }

;
        }
    }

    public void OnSignUp()
    {
        if (usernameInput.text.Length == 0 && passwordInput.text.Length == 0) return; //verifier tout les champs

        GetData();
    }
}

