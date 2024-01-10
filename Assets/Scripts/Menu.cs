using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviourPunCallbacks
{
    public static Menu instance;
    [Header("Screens")]
    public GameObject mainScreen;
    public GameObject lobbyScreen;

    [Header("Main Screen")]
    public Button playButton;

    [Header("Lobby Screen")]
    public TextMeshProUGUI player1NameText;
    public TextMeshProUGUI player2NameText;
    public TextMeshProUGUI gameStartingText;
    public TextMeshProUGUI roomName;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        playButton.interactable = false;
        gameStartingText.gameObject.SetActive(false);
        SetScreen(lobbyScreen);
    }

    public override void OnConnectedToMaster()
    {
        playButton.interactable = true;

        if (User.instance.roomName != null) CreateRoom();
        else PlayRandom();
    }

    public void SetScreen(GameObject screen)
    {
        // disable all screens
        mainScreen.SetActive(false);
        lobbyScreen.SetActive(false);

        // enable the requested screen
        screen.SetActive(true);
    }

    public void OnPlayButton()
    {
        PhotonNetwork.NickName = User.instance.user.user.name;
        NetworkManager.instance.CreateOrJoinRoom();
    }

    public void PlayRandom()
    {
        PhotonNetwork.NickName = User.instance.user.user.name;
        NetworkManager.instance.CreateOrJoinRoom();
    }

    public void CreateRoom()
    {
        PhotonNetwork.NickName = User.instance.user.user.name;
        NetworkManager.instance.CreateRoomGame(User.instance.roomName);
    }

    public override void OnJoinedRoom()
    {
        SetScreen(lobbyScreen);
        photonView.RPC("UpdateLobbyUI", RpcTarget.All);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        UpdateLobbyUI();
    }

    // Updates the lobby screen
    [PunRPC]
    void UpdateLobbyUI()
    {
        player1NameText.text = PhotonNetwork.CurrentRoom.GetPlayer(1).NickName;
        player2NameText.text = PhotonNetwork.PlayerList.Length == 2 ? PhotonNetwork.CurrentRoom.GetPlayer(2).NickName : "...";
        roomName.text = User.instance.roomName != null ? string.Format("Room Name: {0}", User.instance.roomName) : "";

        // Set the game starting text
        if (PhotonNetwork.PlayerList.Length == 2)
        {
            gameStartingText.gameObject.SetActive(true);

            if (PhotonNetwork.IsMasterClient)
                Invoke("TryStartGame", 3.0f);
        }
    }

    void TryStartGame()
    {
        if (PhotonNetwork.PlayerList.Length == 2)
            NetworkManager.instance.photonView.RPC("ChangeScene", RpcTarget.All, "Game");
        else
            gameStartingText.gameObject.SetActive(false);
    }

    public void OnLeaveButton()
    {
        PhotonNetwork.LeaveRoom();
        SceneManager.LoadScene("Home");
    }
}
