using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;
using UnityEngine.UI;

public class PersonTemplate : MonoBehaviour
{
    private string id;
    public RawImage avatar;
    public TMP_Text username;
    public TMP_Text level;
    private Action showPeople;

    public void SetData(string id, string username, string avatar, int level, Action showPeople)
    {
        this.id = id;
        if (avatar.Length != 0)
        {
            this.avatar.texture = ImageTools.CreateTextureFromString(avatar);
        }
        this.username.text = username;
        this.level.text = level.ToString();
        this.showPeople = showPeople;
    }

    public void OnDeleteConnectionButton()
    {
        var request = UnityWebRequest.Delete($"{HttpConst.DELETE_CONNECTION.Value}/{id}");
        print($"{HttpConst.DELETE_CONNECTION.Value}/{id}");
        request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
        StartCoroutine(Requests.instance.GetConnections(request, showPeople));
    }
}
