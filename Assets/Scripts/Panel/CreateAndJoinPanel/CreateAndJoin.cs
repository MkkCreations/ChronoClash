using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CreateAndJoin : MonoBehaviourPunCallbacks
{
    public TMP_InputField createInput;
    public TMP_InputField joinInput;

    public void CreateRoom()
    {
        PhotonNetwork.CreateRoom(createInput.text, new RoomOptions() { MaxPlayers = 2, IsVisible = true }, TypedLobby.Default, null);
    }

    public void JoinRoom(string roomName = "")
    {
        if (roomName == "")
            PhotonNetwork.JoinRoom(joinInput.text);
        else
            PhotonNetwork.JoinRoom(roomName);
    }

    public void JoinRoomInList(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    public void OnJoinedRomm()
    {
        PhotonNetwork.LoadLevel("Game");
    }
}
