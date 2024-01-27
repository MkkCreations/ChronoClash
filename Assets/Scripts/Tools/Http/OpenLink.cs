using System;
using UnityEngine;

public class OpenLink : MonoBehaviour
{
    public void OpenGitHub()
    {
        Application.OpenURL("https://github.com/MkkCreations/ChronoClash");
    }

    public void OpenLandingPage()
    {
        Application.OpenURL("https://landingpage-production-d729.up.railway.app/");
    }
}

