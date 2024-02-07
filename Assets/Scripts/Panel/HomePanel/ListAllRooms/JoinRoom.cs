using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using UnityEngine.SceneManagement;

public class JoinRoom : MonoBehaviour
{
    public TMP_InputField nameRoom; 

    public void JoinRoomInList(string roomName)
    {
        User.instance.roomName = roomName;
    }

    public void JoinPrivateRoom()
    {
        User.instance.roomName = nameRoom.text;
        User.instance.isForPrivateRoom = true;
        SceneManager.LoadScene("Menu");
    }
}
