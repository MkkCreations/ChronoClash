using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AddGame : MonoBehaviour
{
    public static AddGame instance;
	private string URL = HttpConst.CREATEGAME.ToString();

    private void Awake()
    {
        instance = this;
    }

    public void CreateGame(GameDTO game)
    {
        StartCoroutine(FetchData(game));
    }

    public IEnumerator FetchData(GameDTO game)
    {

        using (UnityWebRequest request = UnityWebRequest.Post(URL, JsonUtility.ToJson(game), "application/json"))
        {
            request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
            yield return request.SendWebRequest();

            if (request.result == UnityWebRequest.Result.ConnectionError || request.result == UnityWebRequest.Result.ProtocolError)
            {
                ErrorResponse error = JsonUtility.FromJson<ErrorResponse>(request.downloadHandler.text);
            }
            else
            {
                User.UserData userResponse = JsonUtility.FromJson<User.UserData>(request.downloadHandler.text);
                User.instance.user = userResponse;
                User.instance.logedIn = true;

                PanelManager.instance.GoHome();
            }
        }
    }
}

