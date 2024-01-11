using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelManager : MonoBehaviour
{
    public static PanelManager instance;
    public Login loginPanel;
    public Home homePanel;
    public Register registerPanel;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (GameObject.FindObjectOfType<NetworkManager>())
        {
            Destroy(GameObject.FindObjectOfType<NetworkManager>().gameObject);
        }
        if (GameObject.FindObjectOfType<User>().logedIn)
            GoHome();
    }

    public void GoHome()
    {
        if (loginPanel.gameObject.activeSelf)
        {
            loginPanel.gameObject.SetActive(false);
            homePanel.gameObject.SetActive(true);
        }

        else if (registerPanel.gameObject.activeSelf)
        {
            registerPanel.gameObject.SetActive(false);
            homePanel.gameObject.SetActive(true);
        }
    }

    public void GoLogin()
    {
        if (homePanel.gameObject.activeSelf)
        {
            loginPanel.gameObject.SetActive(true);
            homePanel.gameObject.SetActive(false);
        }
        if (registerPanel.gameObject.activeSelf)
        {
            loginPanel.gameObject.SetActive(true);
            registerPanel.gameObject.SetActive(false);
        }
    }

    public void GoRegister()
    {
        registerPanel.gameObject.SetActive(true);
        loginPanel.gameObject.SetActive(false);

    }

}