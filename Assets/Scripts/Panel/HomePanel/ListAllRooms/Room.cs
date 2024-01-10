using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Room : MonoBehaviour
{
    public Text Name;

    public void Join()
    {
        GameObject.Find("CreateAndJoin").GetComponent<JoinRoom>().JoinRoomInList(Name.text);
    }
}
