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

    public GameObject settingsWindow;

    public GameObject listAllRoomsWindow;

    public GameObject createRoom;

    private void Awake()
    {
        instance = this;
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

    public void OnQuitApplication()
    {
        Application.Quit();
    }
}
