using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using static User;
using System;

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
            User.instance.user = JsonUtility.FromJson<User.UserData>(req.downloadHandler.text);
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
            ErrorResponse error = JsonUtility.FromJson<ErrorResponse>(req.downloadHandler.text);
            errorText.text = error.error;
            Notification.instance.ShowMessage(error.error, true);
        }
        else
        {
            User.instance.user = JsonUtility.FromJson<User.UserData>(req.downloadHandler.text);
            User.instance.logedIn = true;

            Notification.instance.ShowMessage($"{User.instance.user.user.name}, your account has been succesfuly created", false);

            PanelManager.instance.GoHome();
        }
    }

    public IEnumerator Logout(UnityWebRequest req)
    {
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        {
            ErrorResponse error = JsonUtility.FromJson<ErrorResponse>(req.downloadHandler.text);
            Notification.instance.ShowMessage(error.error, true);
        }
        else
        {
            Notification.instance.ShowMessage("You logged out", false);
        }
    }

    public IEnumerator Me(UnityWebRequest req)
    {
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        {
            ErrorResponse error = new();
            try
            {
                error = JsonUtility.FromJson<ErrorResponse>(req.downloadHandler.text);
            } finally
            {
                Notification.instance.ShowMessage(error.error ?? req.downloadHandler.text, true);
            }
        }
        else
        {
            User.instance.user.user = JsonUtility.FromJson<User.UserResponse>(req.downloadHandler.text);
            Notification.instance.ShowMessage("Info has been updated", false);
        }
    }

    public IEnumerator ChangePassword(UnityWebRequest req, TMP_Text errorText)
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
            User.instance.user.user = JsonUtility.FromJson<User.UserResponse>(req.downloadHandler.text);
            User.instance.logedIn = true;
            Notification.instance.ShowMessage("The game has been added", false);
        }
    }

    public IEnumerator GetConnections(UnityWebRequest req, Action showConnections)
    {
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        {
            ErrorResponse error = JsonUtility.FromJson<ErrorResponse>(req.downloadHandler.text);
            Notification.instance.ShowMessage(error.error, true);
        }
        else
        {
            try
            {
                Connections.instance.list = JsonUtility.FromJson<Connections.ListConnections>(req.downloadHandler.text);
            } catch
            {
                Debug.Log("Empty list");
                Connections.instance.list = new Connections.ListConnections();
            } finally
            {
                showConnections();
                Notification.instance.ShowMessage("List of connections ready", false);
            }
        }
    }

    public IEnumerator GetUsers(UnityWebRequest req, Action showPeople)
    {
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        {
            Notification.instance.ShowMessage(req.downloadHandler.text, true);
        }
        else
        {
            try
            {
                People.instance.list = JsonUtility.FromJson<People.ListPeople>(req.downloadHandler.text);
            }
            catch
            {
                Debug.Log("Empty list");
                People.instance.list = new People.ListPeople();
            }
            finally
            {
                showPeople();
                Notification.instance.ShowMessage("List of people ready", false);
            }
        }
    }

    public IEnumerator AddFriend(UnityWebRequest req, Action ShowPeople)
    {
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        {
            Notification.instance.ShowMessage(req.downloadHandler.text, true);
        }
        else
        {
            ShowPeople();
            Notification.instance.ShowMessage(req.downloadHandler.text, false);
        }
    }

    public IEnumerator GetFriendNotifs(UnityWebRequest req, Action ShowNotifs)
    {
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        {
            Notification.instance.ShowMessage(req.downloadHandler.text, true);
        }
        else
        {
            FriendNotifications.instance.list = JsonUtility.FromJson<FriendNotifications.ListFriendNotif>(req.downloadHandler.text);
            ShowNotifs();
            Notification.instance.ShowMessage("Notifications list is ready", false);
        }
    }
}

