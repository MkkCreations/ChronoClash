using UnityEngine;

[System.Serializable]
public class User : MonoBehaviour
{
    public static User instance;
    public UserData user;
    public bool logedIn = User.instance.user.accessToken.Length > 0;

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
        public UserLogsResponse logs;
    }

    [System.Serializable]
    public class UserLogsResponse
    {
        public string id;
        public string name;
        public UserLogsOperationResponse operations;
    }

    [System.Serializable]
    public class UserLogsOperationResponse
    {
        public string id;
        public string type;
        public string description;
        public string date;
    }

    private void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }
}

