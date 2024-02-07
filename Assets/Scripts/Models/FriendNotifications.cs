using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static User;

[System.Serializable]
public class FriendNotifications : MonoBehaviour
{
    public static FriendNotifications instance;
    public ListFriendNotif list;

    [System.Serializable]
    public class ListFriendNotif
    {
        public List<FriendNotif> notifications = new();
    }

    [System.Serializable]
    public class FriendNotif
    {
        public string id;
        public string username;
        public string name;
        public string image;
        public Level level;
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

