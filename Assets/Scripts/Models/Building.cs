using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using System.ComponentModel;
using Enums.TypeEntite;

public class Building : MonoBehaviourPun
{
    public int curHp;
    public int maxHp;

    public int maxDeploymentUnitDistance;

    public bool usedThisTurn;

    public bool selecting = false;

    public GameObject selectedVisual;
    public SpriteRenderer spriteVisual;

    [Header("Sprite Variants")]
    public Sprite leftPlayerSprite;
    public Sprite rightPlayerSprite;

    public OverlayTile standingOnTile;

    public RangeFinder rangeFinder;
    public List<OverlayTile> deploymentUnitRangeTiles;
    public bool unitDeployed = false;

    public TypeEntite typeEntite;

    private void Start()
    {
        rangeFinder = new RangeFinder();
        deploymentUnitRangeTiles = new List<OverlayTile>();
    }

    private void Update()
    {
        /*if (selecting)
        {
            GetInRangeTiles();
        }*/
    }

    public void PositionCharacterOnLine(OverlayTile tile)
    {
        transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y);
        standingOnTile.inTileUnit = null;
        standingOnTile = tile;
        tile.SetBuilding(this);
    }

    [PunRPC]
    void Initialize(bool isMine)
    {
        if (isMine)
            PlayerController.me.buildings.Add(this);
        else
            GameManager.instance.GetOtherPlayer(PlayerController.me).buildings.Add(this);

        // Set sprite variant
        spriteVisual.sprite = transform.position.x < 0 ? leftPlayerSprite : rightPlayerSprite;
    }

    public void GetInRangeTiles()
    {
        List<OverlayTile> allRange = rangeFinder.GetTilesInRange(standingOnTile.grid2DLocation, maxDeploymentUnitDistance);
        deploymentUnitRangeTiles = new List<OverlayTile>();
        foreach (OverlayTile item in allRange)
        {
            if (item.inTileBuilding && item.inTileBuilding != this)
            {
                item.HideTile();
                continue;
            }
            deploymentUnitRangeTiles.Add(item);
            item.ShowTile();
        }
    }

    public bool CanSelect()
    {
        if (usedThisTurn)
            return false;
        else
            return true;
    }

    public bool CanDeploy(OverlayTile tile)
    {
        if (!deploymentUnitRangeTiles.Contains(tile)) {
            GetInRangeTiles();
            unitDeployed = false;
        } else {
            tile.HideTile();
            unitDeployed = true;
        }
        return unitDeployed;
    }

    public void ToggleSelect(bool selected)
    {
        selectedVisual.SetActive(selected);
        selecting = !selecting;
    }

    [PunRPC]
    void Die()
    {
        if (!photonView.IsMine)
        {
            ChangeSprite(true);
            PlayerController.me.buildings.Add(this);
            PlayerController.enemy.buildings.Remove(this);
            PlayerController.me.addCoin(500);
        }
        else
        {
            ChangeSprite(false);
            PlayerController.enemy.buildings.Add(this);
            PlayerController.me.buildings.Remove(this);
            PlayerController.enemy.addCoin(500);

            // check the win condition
            GameManager.instance.CheckWinCondition();
        }
    }

    private void ChangeSprite(bool sens = true)
    {
        if (sens)
        {
            Sprite temp = leftPlayerSprite;
            leftPlayerSprite = rightPlayerSprite;
            rightPlayerSprite = temp;
        } 
        else
        {
            Sprite temp = rightPlayerSprite;
            rightPlayerSprite = leftPlayerSprite;
            leftPlayerSprite = temp;
        }
    }

    public void ShowPanelShop()
    {
        if (typeEntite != TypeEntite.HQ)
            GameUI.instance.OnOpenShop(typeEntite);
    }

    public void HidePanelShop()
    {
        GameUI.instance.OnCloseShop();
    }
}