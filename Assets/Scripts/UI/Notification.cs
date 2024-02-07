using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Notification : MonoBehaviour
{
    public static Notification instance;

    public Image popUp;
    public TMP_Text message;
    public Color errorColor;
    public Color defaultColor;

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
            gameObject.SetActive(false);
            DontDestroyOnLoad(gameObject);
        }
    }

    public void ShowMessage(string text, bool error)
    {
        // new Color(230, 64, 44, 0.5f);
        // new Color(50, 230, 40, 0.5f);
        if (error) popUp.color = errorColor;
        else popUp.color = defaultColor;

        gameObject.SetActive(true);

        message.text = text;

        Invoke("HidePopUp", 5f);
    }

    private void HidePopUp()
    {
        message.text = "";
        gameObject.SetActive(false);
    }
}
