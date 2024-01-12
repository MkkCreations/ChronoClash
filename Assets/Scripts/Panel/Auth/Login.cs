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

    public void GetData()
    {
        StartCoroutine(FetchData());
    }

    public IEnumerator FetchData()
    {
        UserLoginDTO data = new UserLoginDTO();
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

;
        }
    }

    public void OnSignIn()
    {
        if (usernameInput.text.Length == 0 && passwordInput.text.Length == 0) return;

        GetData();
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