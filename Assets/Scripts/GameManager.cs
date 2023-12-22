using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

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
        if (curPlayer == null)
            curPlayer = leftPlayer;
        else
            curPlayer = curPlayer == leftPlayer ? rightPlayer : leftPlayer;

        if (curPlayer == PlayerController.me)
            PlayerController.me.BeginTurn();

        // Toggle the end turn button
    }

    public PlayerController GetOtherPlayer(PlayerController player)
    {
        return player == leftPlayer ? rightPlayer : leftPlayer;
    }
}
