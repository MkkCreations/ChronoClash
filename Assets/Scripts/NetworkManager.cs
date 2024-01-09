using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // instance
    public static NetworkManager instance;

    void Awake()
    {
        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // connect to the server
        PhotonNetwork.ConnectUsingSettings();
    }

    // Joins a random room or creates a new room
    public void CreateOrJoinRoom()
    {
        if (PhotonNetwork.CountOfRooms > 0)
            PhotonNetwork.JoinRandomRoom();
        else
        {
            RoomOptions options = new RoomOptions();
            options.MaxPlayers = 2;

            PhotonNetwork.CreateRoom(null, options);
        }
    }

    public void CreateRoomGame(string roomName)
    {
        // Cr�er et rejoindre une room (max 2 joueurs)
        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = 2,
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions);

    }

    // change the scene using Photon's system
    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

}
