using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using static User;
using UnityEngine.Networking;

public class PlayerController : MonoBehaviourPun
{
    public Player photonPlayer;
    public string[] unitsToSpawn;
    public OverlayTile[] spawnTiles;

    public List<Unit> units = new List<Unit>();
    public Unit selectedUnit;

    public static PlayerController me;
    public static PlayerController enemy;

    [PunRPC]
    void Initialize(Player player)
    {
        photonPlayer = player;

        // If is our local player, spawn the units

        if (player.IsLocal)
        {
            me = this;
            SpawnUnits();
        }
        else
            enemy = this;
        // Set the UI player text
        GameUI.instance.SetPlayerText(this);
    }

    void SpawnUnits()
    {
        for (int x = 0; x < unitsToSpawn.Length; ++x)
        {
            GameObject unit = PhotonNetwork.Instantiate(unitsToSpawn[x], new Vector3(spawnTiles[x].transform.position.x, spawnTiles[x].transform.position.y), Quaternion.identity);
            unit.GetComponent<Unit>().standingOnTile = spawnTiles[x];
            spawnTiles[x].SetUnit(unit.GetComponent<Unit>());
            unit.GetPhotonView().RPC("Initialize", RpcTarget.OthersBuffered, false);
            unit.GetPhotonView().RPC("Initialize", photonPlayer, true);
        }
    }

    private void Update()
    {
        if (!photonView.IsMine)
            return;

        if (Input.GetMouseButtonDown(0) && GameManager.instance.curPlayer == this)
        {
            OverlayTile tile = MapManager.instance.HoveredTile;
            TrySelect(tile);
        }
    }

    void TrySelect(OverlayTile tile)
    {
        // Are we selecting our unit?
        Unit unit = tile.inTileUnit;

        if (unit != null)
        {
            SelectUnit(unit);
            return;
        }

        if (!selectedUnit)
            return;

        // Are we selecting an enemy unit?
        Unit enemyUnit = enemy.units.Find(u => new Vector2(Mathf.RoundToInt(u.transform.position.x), Mathf.RoundToInt(u.transform.position.y)) == new Vector2(Mathf.RoundToInt(tile.transform.position.x), Mathf.RoundToInt(tile.transform.position.y)));
        enemy.units.ForEach(u => print($"{u.transform.position} --- {tile.transform.position}"));

        if (enemyUnit != null)
        {
            TryAttack(enemyUnit);
            return;
        }

        TryMove(tile);
    }

    void SelectUnit(Unit unitToSelect)
    {
        // can we select the unit?
        if (!unitToSelect.CanSelect())
            return;

        // un-select the current unit
        if (selectedUnit != null)
            DeSelectUnit();

        // select the new unit
        selectedUnit = unitToSelect;
        selectedUnit.ToggleSelect(true);

        // Set unit info text
        GameUI.instance.SetUnitInfoText(unitToSelect);
    }

    void DeSelectUnit()
    {
        selectedUnit.ToggleSelect(false);
        selectedUnit = null;

        // Disable the unit info text
        GameUI.instance.unitInfo.gameObject.SetActive(false);
    }

    void SelectNextAvailableUnit()
    {
        Unit availableUnit = units.Find(u => u.CanSelect());

        if (availableUnit != null)
            SelectUnit(availableUnit);
        else
            DeSelectUnit();
    }

    void TryAttack(Unit enemyUnit)
    {
        if (selectedUnit.CanAttack(enemyUnit))
        {
            selectedUnit.Attack(enemyUnit);
            SelectNextAvailableUnit();

            // Update the UI
            GameUI.instance.UpdateWaitingUnitsText(units.FindAll(u => u.CanSelect()).Count);
        }
    }

    void TryMove(OverlayTile tile)
    {
        // Can we move to that pos
        if (selectedUnit.CanMove(tile))
        {
            selectedUnit.usedThisTurn = true;
            SelectNextAvailableUnit();

            // Update the UI
            GameUI.instance.UpdateWaitingUnitsText(units.FindAll(u => u.CanSelect()).Count);
        }
    }

    public void EndTurn()
    {
        if (selectedUnit != null)
            DeSelectUnit();

        // Start the next turn
        GameManager.instance.photonView.RPC("SetNextTurn", RpcTarget.All);
    }

    public void BeginTurn()
    {
        foreach (Unit unit in units)
            unit.usedThisTurn = false;

        // Update UI
        GameUI.instance.UpdateWaitingUnitsText(units.Count);
    }

    public void AddGameToAPI(bool win, string enemy)
    {
        GameDTO gameDTO = new GameDTO
        {
            owner = User.instance.user.user.id,
            enemy = enemy,
            win = win,
            xp = win ? 40 : 20
        };

        var request = UnityWebRequest.Post(HttpConst.CREATEGAME.Value, JsonUtility.ToJson(gameDTO), "application/json");
        request.SetRequestHeader("Authorization", $"Bearer {User.instance.user.accessToken}");
        StartCoroutine(Requests.instance.AddGame(request));
    }
    
}
