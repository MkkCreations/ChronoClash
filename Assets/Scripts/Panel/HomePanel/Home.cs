using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

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

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerInfoPanel.SetActive(true);
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
        // Ferme tous les panels
        settingsWindow.SetActive(false);
        listAllRoomsWindow.SetActive(false);
        createRoom.SetActive(false);
        User.instance.Reset();
        myAccountPlayerPanel.SetActive(false);
        loginPanel.SetActive(true);
        // Réinitialise les inputs de login
        Login.instance.ResetInputFields();
        this.gameObject.SetActive(false);

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

    public void OnQuitApplication()
    {
        Application.Quit();
    }
}
