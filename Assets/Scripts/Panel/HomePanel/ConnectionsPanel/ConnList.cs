using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

public class ConnList : MonoBehaviour
{
    public GameObject scrollView;
    public GameObject connectionTamplate;

    private void Awake()
    {
        var request = UnityWebRequest.Get(HttpConst.CONNECTIONS.Value);
        request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
        StartCoroutine(Requests.instance.GetConnections(request, ShowConnections));
    }

    public void ShowConnections()
    {
        foreach (Connections.Connection conn in Connections.instance.list.connections)
        {
            GameObject connInst = Instantiate(connectionTamplate);
            connInst.transform.SetParent(scrollView.transform);
            connInst.GetComponent<ConnTemplate>().SetData(conn.id, conn.requestIp, conn.requestUserAgent, DateTime.Parse(conn.date), ShowConnections);
        }
    }
}
