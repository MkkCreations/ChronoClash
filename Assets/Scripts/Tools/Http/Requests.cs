using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using static User;

public class Requests : MonoBehaviour
{
    public static Requests instance;

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public IEnumerator Login(UnityWebRequest req, TMP_Text errorText)
    {
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        {
            ErrorResponse error = JsonUtility.FromJson<ErrorResponse>(req.downloadHandler.text);
            errorText.text = error.error;
            Notification.instance.ShowMessage(error.error, true);
        }
        else
        {
            User.UserData userResponse = JsonUtility.FromJson<User.UserData>(req.downloadHandler.text);
            User.instance.user = userResponse;
            User.instance.logedIn = true;
            Notification.instance.ShowMessage($"{User.instance.user.user.name} is loged in", false);

            PanelManager.instance.GoHome();
        }
    }

    public IEnumerator Signup(UnityWebRequest req, TMP_Text errorText)
    {
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        {
            string error = req.downloadHandler.text;
            errorText.text = error;
            Notification.instance.ShowMessage(error, true);
        }
        else
        {
            User.UserData userResponse = JsonUtility.FromJson<User.UserData>(req.downloadHandler.text);
            User.instance.user = userResponse;
            User.instance.logedIn = true;

            Notification.instance.ShowMessage($"{User.instance.user.user.name}, your account has been succesfuly created", false);

            PanelManager.instance.GoHome();
        }
    }

    public IEnumerator ChangePassword(UnityWebRequest req, TMP_Text errorText)
    {
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        {
            errorText.text = req.downloadHandler.text;
            Notification.instance.ShowMessage(req.downloadHandler.text, true);
        }
        else
        {
            Notification.instance.ShowMessage("Password has been correctly changed", false);
            PanelManager.instance.GoHome();
        }
    }

    public IEnumerator AddGame(UnityWebRequest req)
    {
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        {
            ErrorResponse error = JsonUtility.FromJson<ErrorResponse>(req.downloadHandler.text);
            Notification.instance.ShowMessage(error.error, true);
        }
        else
        {
            User.UserData userResponse = JsonUtility.FromJson<User.UserData>(req.downloadHandler.text);
            GameObject.FindObjectOfType<User>().user.user = userResponse.user;
            GameObject.FindObjectOfType<User>().logedIn = true;
            Notification.instance.ShowMessage("The game has been added", false);
        }
    }
}

