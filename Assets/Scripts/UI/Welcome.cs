using UnityEngine;
using UnityEngine.SceneManagement;


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
