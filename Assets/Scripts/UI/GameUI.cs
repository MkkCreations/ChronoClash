using Enums.TypeEntite;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameUI : MonoBehaviour
{
    public Button endTurnButton;
    public TextMeshProUGUI leftPlayerText;
    public TextMeshProUGUI rightPlayerText;
    public TextMeshProUGUI waitingUnitsText;
    public Image unitInfo;
    public TextMeshProUGUI winText;
    public Image quitVerification;
    public TextMeshProUGUI numberCoinText;
    // Panel for items shop 
    public GameObject shopPanel;

    // instance
    public static GameUI instance;

    void Awake()
    {
        // set the instance to this script
        instance = this;
    }

    // called when the "End Turn" button has been pressed
    public void OnEndTurnButton()
    {
        PlayerController.me.EndTurn();
    }

    // toggles the interactivity of the end turn button
    public void ToggleEndTurnButton(bool toggle)
    {
        endTurnButton.interactable = toggle;
        waitingUnitsText.gameObject.SetActive(toggle);
    }

    // updates the current amount of units you can move this turn
    public void UpdateWaitingUnitsText(int waitingUnits)
    {
        waitingUnitsText.text = waitingUnits + " Units Waiting";
    }

    // sets the requested player's text
    public void SetPlayerText(PlayerController player)
    {
        TextMeshProUGUI text = player == GameManager.instance.leftPlayer ? leftPlayerText : rightPlayerText;
        text.text = player.photonPlayer.NickName;
    }

    public void UpdateCoinText(int coin)
    {
        numberCoinText.text = coin.ToString();
    }

    // sets the unit info text
    public void SetUnitInfoText(Unit unit)
    {
        unitInfo.gameObject.SetActive(true);
        TextMeshProUGUI unitInfoText = unitInfo.gameObject.GetComponentInChildren<TextMeshProUGUI>();
        unitInfoText.text = "";

        unitInfoText.text += string.Format("<b>Hp:</b> {0} / {1}", unit.curHp, unit.maxHp);
        unitInfoText.text += string.Format("\n<b>Move Range:</b> {0}", unit.maxMoveDistance);
        unitInfoText.text += string.Format("\n<b>Attack Range:</b> {0}", unit.maxAttackDistance);
        unitInfoText.text += string.Format("\n<b>Damage:</b> {0} - {1}", unit.minDamage, unit.maxDamage);
    }

    // displays the win text
    public void SetWinText(string winnerName)
    {
        winText.gameObject.SetActive(true);
        winText.text = winnerName + " Wins";
    }

    public void OnVerificationQuiteGame()
    {
        quitVerification.gameObject.SetActive(!quitVerification.gameObject.activeSelf);
    }

    // quite game
    public void OnQuitGame()
    {
        PlayerController.me.AddGameToAPI(false, GameManager.instance.GetOtherPlayer(PlayerController.me).photonPlayer.NickName);
        NetworkManager.instance.Leave();
    }

    public void OnOpenShop(TypeEntite typeBuildingShop)
    {
        shopPanel.SetActive(true);
        ShopListPanel.instance.UpdateList();
    }

    public void OnCloseShop()
    {
        shopPanel.SetActive(false);
    }
}
