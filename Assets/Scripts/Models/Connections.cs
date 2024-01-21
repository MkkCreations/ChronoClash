using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class Connections : MonoBehaviour
{
    public static Connections instance;
    public ListConnections list;

    [System.Serializable]
    public class ListConnections
    {
        public List<Connection> connections = new List<Connection>();
    }

    [System.Serializable]
    public class Connection
    {
        public string id;
        public string requestIp;
        public string requestUserAgent;
        public string date;
    }  

    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }
}

