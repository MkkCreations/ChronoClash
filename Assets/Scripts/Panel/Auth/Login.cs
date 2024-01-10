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
    public string URL;

    [System.Serializable]
    public class UserLogin
    {
        public string username;
        public string password;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        URL = "http://127.0.0.1:8081/api/auth/login";
    }

    public void GetData()
    {
        StartCoroutine(FetchData());
    }

    public IEnumerator FetchData()
    {
        UserLogin data = new UserLogin();
        data.username = usernameInput.text;
        data.password = passwordInput.text;

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
                User.UserData userResponse = JsonUtility.FromJson<User.UserData>(request.downloadHandler.text);
                User.instance.user = userResponse;
                User.instance.logedIn = true;

                PanelManager.instance.GoHome();
            }
             
;        }
    }

    public void OnSignIn()
    {
        if (usernameInput.text.Length == 0 && passwordInput.text.Length == 0) return;

        GetData();
    }
}
