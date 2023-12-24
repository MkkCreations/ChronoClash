using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Home : MonoBehaviour
{
    public static Home instance;
    public TMP_Text welcomeText;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (isActiveAndEnabled)
            welcomeText.text = string.Format("Welcome {0}", User.instance.user.user.name);
    }
}
