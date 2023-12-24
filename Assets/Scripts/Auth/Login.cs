using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    public Button signinButton;
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public string URL;

    [System.Serializable]
    public class UserLogin
    {
        public string username;
        public string password;
    }

    [System.Serializable]
    public class UserResponse
    {
        public string id;
        public string name;
        public string username;
        public string email;
        public string image;
        public string role;
        public UserLogsResponse logs;
    }

    [System.Serializable]
    public class UserLogsResponse
    {
        public string id;
        public string name;
        public UserLogsOperationResponse operations;
    }

    [System.Serializable]
    public class UserLogsOperationResponse
    {
        public string id;
        public string type;
        public string description;
        public string date;
    }

    [System.Serializable]
    public class LoginResponse
    {
        public UserResponse user;
        public string accessToken;
        public string refreshToken;

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

        Debug.Log(JsonUtility.ToJson(data).ToString());

        using (UnityWebRequest request = UnityWebRequest.Post(URL, JsonUtility.ToJson(data), "application/json"))
        {
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError)
            {
                Debug.Log(request.error);
            }
            else
            {
                Debug.Log(request.downloadHandler.text);
                LoginResponse user = JsonUtility.FromJson<LoginResponse>(request.downloadHandler.text);
                Debug.Log(user.user.email);
            }
             
;        }
    }

    public void OnSignIn()
    {
        if (usernameInput.text.Length == 0 && passwordInput.text.Length == 0) return;

        GetData();
    }
}
