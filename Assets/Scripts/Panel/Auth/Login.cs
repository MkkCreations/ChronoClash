using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

public class Login : MonoBehaviour
{
    public static Login instance;
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
                User.UserData userResponse = JsonUtility.FromJson<User.UserData>(request.downloadHandler.text);
                User.instance.user = userResponse;
                Debug.Log(userResponse.user.email);

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
