
using UnityEngine;
using UnityEngine.SceneManagement;

public class PausePanel : MonoBehaviour
{
    public static bool gameIsPaused = false;

    public GameObject forfeitMenuUi;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if(gameIsPaused) {
                Resume();
             }
            else
            {
                Paused();
            }
        }
    
    }


    void Paused()
    {
        gameIsPaused = true;
        forfeitMenuUi.SetActive(true);


    }

    public void Resume()
    {
        gameIsPaused = false;
        forfeitMenuUi.SetActive(false);
    }


}
