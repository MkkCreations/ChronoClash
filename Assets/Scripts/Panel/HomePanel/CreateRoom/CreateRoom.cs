using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class CreateRoom : MonoBehaviourPunCallbacks
{
    public TMP_InputField roomName;

    public Dropdown dropdownMap;
    public Dropdown dropdownColor;
    public Dropdown dropdownMode;
    public Toggle togglePrivate;

    public void Start()
    {
        // Désactiver Les dropdowns et le toggle
        dropdownMap.interactable = false;
        dropdownColor.interactable = false;
        dropdownMode.interactable = false;
        togglePrivate.interactable = false;
    }

    public void CreateRoomGame()
    {
        // Créer et rejoindre une room (max 2 joueurs)
        RoomOptions roomOptions = new RoomOptions()
        {
            MaxPlayers = 2,
            IsVisible = true,
            IsOpen = true,
            PublishUserId = true
        };
        PhotonNetwork.JoinOrCreateRoom(roomName.text, roomOptions, TypedLobby.Default);

    }

    override public void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("Menu");
    }

}
