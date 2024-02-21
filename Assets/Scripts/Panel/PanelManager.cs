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
        if (User.instance.logedIn)
            GoHome();
    }

    public void GoHome()
    {
        loginPanel.gameObject.SetActive(false);
        registerPanel.gameObject.SetActive(false);
        homePanel.gameObject.SetActive(true);
        Home.instance.friendsPanel.SetActive(true);
    }

    public void GoLogin()
    {
        homePanel.gameObject.SetActive(false);
        loginPanel.gameObject.SetActive(true);
        registerPanel.gameObject.SetActive(false);
    }

    public void GoRegister()
    {
        registerPanel.gameObject.SetActive(true);
        loginPanel.gameObject.SetActive(false);
    }

}