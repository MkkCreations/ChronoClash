using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.Networking;
using System;

public class PlayerController : MonoBehaviourPun
{
    public Player photonPlayer;
    public string[] unitsToSpawn;
    public string[] buildingsToSpawn;
    public OverlayTile[] spawnTilesUnits;
    public OverlayTile[] spawnTilesBuildings;

    public List<Unit> units = new();
    public Unit selectedUnit;

    public List<Building> buildings = new();
    public Building selectedBuilding;

    public static PlayerController me;
    public static PlayerController enemy;

    public int coin = 3000;

    public GameObject itemTobuy;

    [PunRPC]
    void Initialize(Player player)
    {
        photonPlayer = player;

        // If is our local player, spawn the units

        if (player.IsLocal)
        {
            me = this;
            SpawnBuildings();
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
            GameObject unit = PhotonNetwork.Instantiate(unitsToSpawn[x], new Vector3(spawnTilesUnits[x].transform.position.x, spawnTilesUnits[x].transform.position.y), Quaternion.identity);
            unit.GetComponent<Unit>().standingOnTile = spawnTilesUnits[x];
            spawnTilesUnits[x].SetUnit(unit.GetComponent<Unit>());
            unit.GetPhotonView().RPC("Initialize", RpcTarget.OthersBuffered, false);
            GameObject unit = PhotonNetwork.Instantiate(unitsToSpawn[x], new Vector3(spawnTiles[x].transform.position.x, spawnTiles[x].transform.position.y), Quaternion.identity);
            unit.GetComponent<Unit>().standingOnTile = spawnTiles[x];
            spawnTiles[x].SetUnit(unit.GetComponent<Unit>());
            unit.GetPhotonView().RPC("Initialize", RpcTarget.Others, false);
            unit.GetPhotonView().RPC("Initialize", photonPlayer, true);
        }
    }

    void SpawnBuildings()
    {
        for (int x = 0; x < buildingsToSpawn.Length; ++x)
        {
            GameObject building = PhotonNetwork.Instantiate(buildingsToSpawn[x], new Vector3(spawnTilesBuildings[x].transform.position.x, spawnTilesBuildings[x].transform.position.y), Quaternion.identity);
            building.GetComponent<Building>().standingOnTile = spawnTilesBuildings[x];
            spawnTilesBuildings[x].SetBuilding(building.GetComponent<Building>());
            building.GetPhotonView().RPC("Initialize", RpcTarget.OthersBuffered, false);
            building.GetPhotonView().RPC("Initialize", photonPlayer, true);
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
        Building building = tile.inTileBuilding;

        if (unit != null)
        {
            SelectUnit(unit);
            return;
        }

        if (building != null)
        {
            SelectBuilding(building);
            return;
        }

        if (!selectedUnit && !selectedBuilding)
            return;

        // Are we selecting an enemy unit?
        Unit enemyUnit = enemy.units.Find(u => new Vector2(Mathf.RoundToInt(u.transform.position.x), Mathf.RoundToInt(u.transform.position.y)) == new Vector2(Mathf.RoundToInt(tile.transform.position.x), Mathf.RoundToInt(tile.transform.position.y)));
        enemy.units.ForEach(u => print($"{u.transform.position} --- {tile.transform.position}"));

        if (enemyUnit != null && selectedUnit != null)
        {
            TryAttack(enemyUnit);
            return;
        }

        if(selectedUnit != null)
            TryMove(tile);

        if (selectedBuilding != null && itemTobuy != null)
        {
            print(selectedBuilding);
            selectedBuilding.GetInRangeTiles();
            if (selectedBuilding.deploymentUnitRangeTiles.Contains(tile))
                if (selectedBuilding.CanDeploy(tile))
                    DeployUnit(tile);
        }
    }

    void SelectUnit(Unit unitToSelect)
    {
        // can we select the unit?
        if (!unitToSelect.CanSelect())
            return;

        // un-select the current unit
        if (selectedUnit != null)
            DeSelectUnit();

        // un-select the current building
        if (selectedBuilding != null)
            DeSelectBuilding();

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

    void SelectBuilding(Building building)
    {
        if(!building.CanSelect())
            return;

        if (selectedBuilding != null)
            DeSelectBuilding();

        if(selectedUnit != null)
            DeSelectUnit();

        selectedBuilding = building;
        selectedBuilding.ToggleSelect(true);
        building.ShowPanelShop();
    }

    void DeSelectBuilding()
    {
        selectedBuilding.ToggleSelect(false);
        selectedBuilding.HidePanelShop();
        selectedBuilding = null;
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

        // Add 1500 coins to the player
        addCoin(1500);
        // Start the next turn
        GameManager.instance.photonView.RPC("SetNextTurn", RpcTarget.All);
    }

    public void BeginTurn()
    {
        foreach (Unit unit in units)
            unit.usedThisTurn = false;

        foreach (Building building in buildings)
            building.usedThisTurn = false;

        // Update UI
        GameUI.instance.UpdateWaitingUnitsText(units.Count);
    }

    public void AddGameToAPI(bool win, string enemy)
    {
        GameDTO gameDTO = new()
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
    
    public int getCoin()
    {
        return this.coin;
    }

    public void addCoin(int coin)
    {
        this.coin += coin;
    }

    public void useCoin(int coin)
    {
        this.coin -= coin;
        if(this.coin < 0)
            this.coin = 0;
    }

    public bool BuyItem(GameObject item)
    {
        if (item.GetComponent<Unit>().cost > this.coin)
            return false;

        itemTobuy = item;
        return true;
    }

    public void DeployUnit(OverlayTile tile)
    {
        GameObject unit = PhotonNetwork.Instantiate(itemTobuy.name, new Vector3(tile.transform.position.x, tile.transform.position.y), Quaternion.identity);
        unit.GetComponent<Unit>().standingOnTile = tile;
        tile.SetUnit(unit.GetComponent<Unit>());
        unit.GetPhotonView().RPC("Initialize", RpcTarget.OthersBuffered, false);
        unit.GetPhotonView().RPC("Initialize", photonPlayer, true);

        // Ajoute l'unit� � la liste des unit�s du joueur
        units.Add(unit.GetComponent<Unit>());

        this.useCoin(itemTobuy.GetComponent<Unit>().cost);
        itemTobuy = null;

        // Bloque le batiment pour le tour
        selectedBuilding.usedThisTurn = true;

        // Bloque la nouvelle unit� pour le tour 
        unit.GetComponent<Unit>().usedThisTurn = true;

        // Update the UI
        GameUI.instance.UpdateWaitingUnitsText(units.FindAll(u => u.CanSelect()).Count);
    }


}
