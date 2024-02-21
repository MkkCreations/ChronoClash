using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;

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

    private User user;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        user = User.instance;
        playButton.interactable = false;
        gameStartingText.gameObject.SetActive(false);
        SetScreen(lobbyScreen);
    }

    public override void OnConnectedToMaster()
    {
        playButton.interactable = true;
        if(user.isForPrivateRoom) JoinPrivateRoom();
        else if (user.roomName.Length != 0) CreateRoom();
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
        PhotonNetwork.NickName = user.user.user.name;
        NetworkManager.instance.CreateOrJoinRoom();
    }

    public void PlayRandom()
    {
        PhotonNetwork.NickName = user.user.user.name;
        NetworkManager.instance.CreateOrJoinRoom();
    }

    public void CreateRoom()
    {
        PhotonNetwork.NickName = user.user.user.name;
        NetworkManager.instance.CreateRoomGame(user.roomName);
    }

    public void JoinPrivateRoom()
    {
        PhotonNetwork.NickName = user.user.user.name;
        NetworkManager.instance.JoinPrivateRoom(user.roomName);
    }

    public override void OnJoinedRoom()
    {
        Notification.instance.ShowMessage($"{user.roomName} Room has been created", false);
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
        
        if (isPrivateRoomName())
            roomName.text = string.Format("Private Room Code: {0}", user.roomName);
        else
            roomName.text = user.roomName != null ? string.Format("Room Name: {0}", user.roomName) : "";

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
        user.roomName = "";
        user.isForPrivateRoom = false;
        NetworkManager.instance.Leave();
        Notification.instance.ShowMessage($"You leaved the room {user.roomName}", false);
        SceneManager.LoadScene("Home");
    }

    private bool isPrivateRoomName()
    {
        //Si le nom de la salle est non nulle et que c'est un nombre � 8 chiffres alors c'est une partie priv�e
        if (user.roomName != null && user.roomName.Length == 8)
        {
            // V�rifie que le nom de la salle est bien un nombre avec un Int tryParse
            int result;
            if (int.TryParse(user.roomName, out result))
                return true;
            else
                return false;
        }
        else
            return false;
    }
}
