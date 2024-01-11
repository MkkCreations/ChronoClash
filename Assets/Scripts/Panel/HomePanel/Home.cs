using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class Home : MonoBehaviour
{
    public static Home instance;
    public TMP_Text welcomeText;

    public GameObject loginPanel;

    public GameObject settingsWindow;

    public GameObject listAllRoomsWindow;

    public GameObject createRoom;

    public GameObject playerInfoPanel;
    public GameObject myAccountPlayerPanel;
    bool isMyAccountPlayerPanelActive = false;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playerInfoPanel.SetActive(true);
    }

    private void Update()
    {
        if (isActiveAndEnabled)
            welcomeText.text = string.Format("Welcome {0}", User.instance.user.user.name);
    }

    // Rooms
    public void OnListAllRoomsButton() { listAllRoomsWindow.SetActive(true); }

    public void OnCloseListAllRoomsButton() { listAllRoomsWindow.SetActive(false); }

    public void OnCreateRoom() { createRoom.SetActive(true); }

    public void OnCloseCreateRoom() { createRoom.SetActive(false); }

    public void OnRandomRoom() { SceneManager.LoadScene("Menu"); }
    // End Rooms

    // Settings
    public void OnSettingsButton() 
    { 
        settingsWindow.SetActive(true);
    }

    public void OnCloseSettingsButton()
    {
        settingsWindow.SetActive(false);
    }
    // End Settings

    // Panel Info player
    public void OnPlayerAvatarButton()
    {
        if (isMyAccountPlayerPanelActive)
        {
            myAccountPlayerPanel.SetActive(false);
            isMyAccountPlayerPanelActive = false;
        }
        else
        {
            myAccountPlayerPanel.SetActive(true);
            isMyAccountPlayerPanelActive = true;
        }
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

    public void OnQuitApplication()
    {
        Application.Quit();
    }
}
