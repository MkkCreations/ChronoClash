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

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (isActiveAndEnabled)
            welcomeText.text = string.Format("Welcome {0}", User.instance.user.user.name);
    }

    public void OnListAllRooms()
    {
        
    }

    public void OnCreateRoom()
    {

    }

    public void OnRandomRoom()
    {
        SceneManager.LoadScene("Menu");
    }

    public void OnSettingsButton() 
    { 
        settingsWindow.SetActive(true);
    }

    public void OnCloseSettingsButton()
    {
        settingsWindow.SetActive(false);
    }

    public void OnQuitApplication()
    {
        Application.Quit();
    }
}
