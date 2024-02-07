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
            RoomOptions options = new()
            {
                MaxPlayers = 2
            };
            PhotonNetwork.CreateRoom(null, options);
        }
    }

    public void CreateRoomGame(string roomName)
    {
        // Cr�er et rejoindre une room (max 2 joueurs)
        RoomOptions roomOptions = new()
        {
            MaxPlayers = 2,
        };
        PhotonNetwork.CreateRoom(roomName, roomOptions);

    }

    public void JoinPrivateRoom(string roomName)
    {
        PhotonNetwork.JoinRoom(roomName);
    }

    // change the scene using Photon's system
    [PunRPC]
    public void ChangeScene(string sceneName)
    {
        PhotonNetwork.LoadLevel(sceneName);
    }

    public void Leave()
    {
        PhotonNetwork.LeaveRoom();
        PhotonNetwork.Disconnect();
        ChangeScene("Home");
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        PlayerController.me.AddGameToAPI(true, otherPlayer.NickName);
        Leave();
    }
}
