using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

public class PlayerController : MonoBehaviourPun
{
    public Player photonPlayer;
    public string[] unitsToSpawn;
    public Transform[] spawnPoints;
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
            GameObject unit = PhotonNetwork.Instantiate(unitsToSpawn[x], new Vector3(Mathf.RoundToInt(spawnTiles[x].transform.position.x), Mathf.RoundToInt(spawnTiles[x].transform.position.y)), Quaternion.identity);
            unit.GetComponent<Unit>().standingOnTile = spawnTiles[x];
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
            Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            TrySelect(new Vector3(Mathf.RoundToInt(pos.x), Mathf.RoundToInt(pos.y), 0));
        }
    }

    void TrySelect(Vector3 selectPos)
    {
        // Are we selecting our unit?
        Unit unit = units.Find(u => u.transform.position == selectPos);

        if (unit != null)
        {
            SelectUnit(unit);
            return;
        }

        if (!selectedUnit)
            return;

        // Are we selecting an enemy unit?
        Unit enemyUnit = enemy.units.Find(u => u.transform.position == selectPos);

        if (enemyUnit != null)
        {
            TryAttack(enemyUnit);
            return;
        }

        TryMove(selectPos);
    }

    void SelectUnit(Unit unitToSelect)
    {
        // can we select the unit?
        if (!unitToSelect.CanSelect())
            return;

        // un-select the current unit
        if (selectedUnit != null)
            selectedUnit.ToggleSelect(false);

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
        GameUI.instance.unitInfoText.gameObject.SetActive(false);
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
        if (selectedUnit.CanAttack(enemyUnit.transform.position))
        {
            selectedUnit.Attack(enemyUnit);
            SelectNextAvailableUnit();

            // Update the UI
            GameUI.instance.UpdateWaitingUnitsText(units.FindAll(u => u.CanSelect()).Count);
        }
    }

    void TryMove(Vector3 movePos)
    {
        // Can we move to that pos
        if (selectedUnit.CanMove(movePos))
        {
            selectedUnit.Move(movePos);
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
    
}
