using Photon.Pun;

public class GameManager : MonoBehaviourPun
{
    public PlayerController leftPlayer;
    public PlayerController rightPlayer;

    public PlayerController curPlayer;

    public float postGameTime;

    public static GameManager instance;

    private void Awake()
    {
        instance = this;
    }

    public void Start()
    {
        // the master client will set the players
        if (PhotonNetwork.IsMasterClient)
            SetPlayers();
    }

    public void Update()
    {
        // show player coin
        if (PlayerController.me != null)
            GameUI.instance.UpdateCoinText(PlayerController.me.getCoin());
    }

    void SetPlayers()
    {
        // Set the owners of the two player's photon views
        leftPlayer.photonView.TransferOwnership(1);
        rightPlayer.photonView.TransferOwnership(2);

        // Initialize the players
        leftPlayer.photonView.RPC("Initialize", RpcTarget.AllBuffered, PhotonNetwork.CurrentRoom.GetPlayer(1));
        rightPlayer.photonView.RPC("Initialize", RpcTarget.AllBuffered, PhotonNetwork.CurrentRoom.GetPlayer(2));

        photonView.RPC("SetNextTurn", RpcTarget.AllBuffered);
    }

    [PunRPC]
    void SetNextTurn()
    {
        // is this the first turn?
        if (curPlayer == null)
            curPlayer = leftPlayer;
        else
            curPlayer = curPlayer == leftPlayer ? rightPlayer : leftPlayer;

        // if it's our turn - enable the end turn button
        if (curPlayer == PlayerController.me)
        {
            PlayerController.me.BeginTurn();
        }
        // toggle the end turn button
        GameUI.instance.ToggleEndTurnButton(curPlayer == PlayerController.me);
    }

    public PlayerController GetOtherPlayer(PlayerController player)
    {
        return player == leftPlayer ? rightPlayer : leftPlayer;
    }

    public void CheckWinCondition()
    {
        if (PlayerController.me.units.Count == 0)
        { 
            photonView.RPC("WinGame", RpcTarget.All, PlayerController.enemy == leftPlayer ? 0 : 1);
        }
    }

    [PunRPC]
    public void WinGame(int winner)
    {
        PlayerController player = winner == 0 ? leftPlayer : rightPlayer;

        if (player.photonPlayer.IsLocal)
            PlayerController.me.AddGameToAPI(true, GetOtherPlayer(player).photonPlayer.NickName);
        else
            PlayerController.me.AddGameToAPI(false, player.photonPlayer.NickName);

        GameUI.instance.SetWinText(player.photonPlayer.NickName);

        Invoke(nameof(GoBackToMenu), postGameTime);
    }

    // leave the room and go back to the Home
    void GoBackToMenu()
    {
        NetworkManager.instance.Leave();
    }
}
