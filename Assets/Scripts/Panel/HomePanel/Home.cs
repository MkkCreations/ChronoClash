using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using static User;
using UnityEngine.Networking;

public class Home : MonoBehaviour
{
    public static Home instance;

    public GameObject loginPanel;

    public GameObject settingsWindow;

    public GameObject listAllRoomsWindow;

    public GameObject createRoom;

    public GameObject playerInfoPanel;
    public GameObject myAccountPlayerPanel;
    bool isMyAccountPlayerPanelActive = false;

    public GameObject myAccountWindow;

    public GameObject logsPanel;
    public GameObject gamesPanel;
    public GameObject connectionsPanel;
    public GameObject peoplePanel;
    public GameObject notificationPanel;
    public GameObject friendsPanel;
    public GameObject rankPanel;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerInfoPanel.SetActive(true);
        friendsPanel.SetActive(true);

        GetUserData();
    }

    public void GetUserData()
    {
        var request = UnityWebRequest.Get(HttpConst.ME.Value);
        request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
        StartCoroutine(Requests.instance.Me(request));
    }

    // Rooms
    public void OnListAllRoomsButton() { listAllRoomsWindow.SetActive(true); }

    public void OnCloseListAllRoomsButton() { listAllRoomsWindow.SetActive(false); }

    public void OnCreateRoom() { createRoom.SetActive(true); }

    public void OnCloseCreateRoom() { createRoom.SetActive(false); }

    public void OnRandomRoom() { SceneManager.LoadScene("Menu"); }
    // End Rooms

    // Settings
    public void OnSettingsButton() { settingsWindow.SetActive(true); }

    public void OnCloseSettingsButton() { settingsWindow.SetActive(false); }
    // End Settings

    // Panel Info player
    public void OnPlayerAvatarButton()
    {
        if (isMyAccountPlayerPanelActive)
            this.HideAccountPlayerPanel();
        else
            this.ShowAccountPlayerPanel();
    }

    private void ShowAccountPlayerPanel()
    {
        myAccountPlayerPanel.SetActive(true);
        isMyAccountPlayerPanelActive = true;
    }

    private void HideAccountPlayerPanel()
    {
        myAccountPlayerPanel.SetActive(false);
        isMyAccountPlayerPanelActive = false;
    }

    public void OnDisconnectButton()
    {
        Logout();

        // Ferme tous les panels
        settingsWindow.SetActive(false);
        listAllRoomsWindow.SetActive(false);
        createRoom.SetActive(false);
        friendsPanel.SetActive(false);
        User.instance.Reset();
        myAccountPlayerPanel.SetActive(false);
        // R�initialise les inputs de login
        loginPanel.SetActive(true);
        Login.instance.ResetInputFields();
        gameObject.SetActive(false);

        Connections.instance.list.connections = new();
        FriendNotifications.instance.list.notifications = new();
    }

    // MyAccountWindow
    public void OnMyAccountButton() { 
        myAccountWindow.SetActive(true);
        myAccountWindow.GetComponent<MyAccount>().LoadData();
        this.HideAccountPlayerPanel();
    }
    
    public void OnCloseMyAccountButton() { myAccountWindow.SetActive(false); }
    // End MyAccountWindow
    // End Panel Info player

    public void OnLogsPanelButton() { logsPanel.SetActive(true); }
    public void OnCloseLogsPanelButton() { logsPanel.SetActive(false); }

    public void OnGamesPanelButton() { gamesPanel.SetActive(true); }
    public void OnCloseGamesPanelButton() { gamesPanel.SetActive(false); }

    public void OnConnectionsPanelButton() { connectionsPanel.SetActive(true); }
    public void OnCloseConnectionsPanelButton() { connectionsPanel.SetActive(false); }

    public void OnPeoplePanelButton() { peoplePanel.SetActive(true); }
    public void OnClosePeoplePanelButton() { peoplePanel.SetActive(false); }

    public void OnNotifPanelButton() { notificationPanel.SetActive(true); }
    public void OnCloseNotifPanelButton() { notificationPanel.SetActive(false); }

    public void OnRankPanelButton() { rankPanel.SetActive(true); }
    public void OnCloseRankPanelButton() { rankPanel.SetActive(false); }

    public void OnQuitApplication()
    {
        Logout();
        Application.Quit();
    }

    private void Logout()
    {
        LogoutDTO data = new()
        {
            refreshToken = User.instance.user.refreshToken
        };
        var request = UnityWebRequest.Post(HttpConst.LOGOUT.Value, JsonUtility.ToJson(data), "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
        StartCoroutine(Requests.instance.Logout(request));
    }
}
