using System;
using UnityEngine;
using TMPro;
using UnityEngine.Networking;

public class ConnTemplate : MonoBehaviour
{
    private string id;
    public TMP_Text requestIp;
    public TMP_Text requestUserAgent;
    public TMP_Text date;
    private Action showConnections;

    public void SetData(string id, string requestIp, string requestUserAgent, DateTime date, Action showConnections)
    {
        this.id = id;
        this.requestIp.text = requestIp;
        this.requestUserAgent.text = requestUserAgent;
        this.date.text = date.ToString("g");
        this.showConnections = showConnections;
    }

    public void OnDeleteConnectionButton()
    {
        var request = UnityWebRequest.Delete($"{HttpConst.DELETE_CONNECTION.Value}/{id}");
        print($"{HttpConst.DELETE_CONNECTION.Value}/{id}");
        request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
        StartCoroutine(Requests.instance.GetConnections(request, showConnections));
    }
}
