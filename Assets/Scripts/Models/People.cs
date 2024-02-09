using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class People : MonoBehaviour
{
    public static People instance;
    public ListPeople list;

    [System.Serializable]
    public class ListPeople
    {
        public List<Person> users = new();
    }

    [System.Serializable]
    public class Person
    {
        public string id;
        public string username;
        public string name;
        public string email;
        public string image;
        public int level;
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

