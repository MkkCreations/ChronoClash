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
        var request = UnityWebRequest.Post(URL, JsonUtility.ToJson(game), "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
        StartCoroutine(FetchData(request));
    }

    public IEnumerator FetchData(UnityWebRequest req)
    {
        yield return req.SendWebRequest();

        if (req.result == UnityWebRequest.Result.ConnectionError || req.result == UnityWebRequest.Result.ProtocolError)
        {
            ErrorResponse error = JsonUtility.FromJson<ErrorResponse>(req.downloadHandler.text);
            Debug.Log(error.error);
        }
        else
        {
            User.UserData userResponse = JsonUtility.FromJson<User.UserData>(req.downloadHandler.text);
            User.instance.user = userResponse;
            User.instance.logedIn = true;
        }
    }
}

