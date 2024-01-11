using System;

[System.Serializable]
public class GameDTO
{
    public User.UserData owner;
    public string enemy;
    public bool win;
    public int xp;
}