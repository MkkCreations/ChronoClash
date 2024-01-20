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
        StartCoroutine(Requests.instance.AddGame(request));
    }
}

