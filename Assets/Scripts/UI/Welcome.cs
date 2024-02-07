using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class Welcome : MonoBehaviour
{
    private void Start()
    {
        Screen.fullScreen = true;
        Invoke("ChangeScene", 3f);
    }

    private void ChangeScene()
    {
        SceneManager.LoadScene("Home");
    }
}
