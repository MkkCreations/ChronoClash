﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class User : MonoBehaviour
{
    public static User instance;
    public UserData user;
    public bool logedIn = false;
    public string roomName = null;
    public bool isForPrivateRoom = false;

    public ExperienceBar experienceBar;

    [System.Serializable]
    public class UserData
    {
        public UserResponse user;
        public string accessToken;
        public string refreshToken;
    }

    [System.Serializable]
    public class UserResponse
    {
        public string id;
        public string name;
        public string username;
        public string email;
        public string image;
        public string role;
        public Level level;
        public List<Game> games;
        public List<Log> logs;
    }

    [System.Serializable]
    public class Level
    {
        public string id;
        public int level;
        public int xp;
    }

    [System.Serializable]
    public class Game
    {
        public string id;
        public string enemy;
        public int win;
        public string date;
    }

    [System.Serializable]
    public class Log
    {
        public string id;
        public string name;
        public List<LogsOperation> operations;
    }

    [System.Serializable]
    public class LogsOperation
    {
        public string id;
        public string type;
        public string description;
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
            DontDestroyOnLoad(gameObject);
        }
    }

    public void Reset()
    {
        user = null;
        logedIn = false;
        roomName = null;
        isForPrivateRoom = false;
    }
}

