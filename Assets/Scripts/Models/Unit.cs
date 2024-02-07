using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using System.ComponentModel;
using Enums.TypeEntite;

public class Unit : MonoBehaviourPun
{
    public int curHp;
    public int maxHp;
    public float moveSpeed;
    public int minDamage;
    public int maxDamage;

    public int maxMoveDistance;
    public int maxAttackDistance;

    public bool usedThisTurn;

    public bool isMoving = false;
    public bool selecting = false;

    public GameObject selectedVisual;
    public SpriteRenderer spriteVisual;

    [Header("UI")]
    public Image healthFillImage;

    [Header("Sprite Variants")]
    public Sprite leftPlayerSprite;
    public Sprite rightPlayerSprite;

    public OverlayTile standingOnTile;

    public RangeFinder rangeFinder;
    public List<OverlayTile> rangeFinderTiles;
    public List<OverlayTile> attackRangTiles;

    private PathFinder pathFinder;
    public List<OverlayTile> path;

    private OverlayTile tileToMove;

    public ArrowTranslator arrowTranslator;

    public TypeEntite typeEntite;

    private void Start()
    {
        rangeFinder = new RangeFinder();
        rangeFinderTiles = new List<OverlayTile>();
        pathFinder = new PathFinder();
        path = new List<OverlayTile>();
        arrowTranslator = new ArrowTranslator();
        attackRangTiles = new List<OverlayTile>();
    }

    private void Update()
    {
        if (selecting && !isMoving)
        {
            GetInRangeTiles();
            GetAttackInRangeTiles();
            tileToMove = MapManager.instance.HoveredTile;
            if (rangeFinderTiles.Contains(tileToMove))
            {
                path = pathFinder.FindPath(standingOnTile, tileToMove, rangeFinderTiles);

                RemoveArrows();

                for (int i = 0; i < path.Count; i++)
                {
                    var previousTile = i > 0 ? path[i - 1] : standingOnTile;
                    var futureTile = i < path.Count - 1 ? path[i + 1] : null;

                    var arrow = arrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                    path[i].SetSprite(arrow);
                }
            }
        }

        if (path.Count > 0 && isMoving)
        {
            MoveAlongPath();
        }
    }

    public void MoveAlongPath()
    {
        var step = moveSpeed * Time.deltaTime;

        transform.position = Vector2.MoveTowards(transform.position, path[0].transform.position, step);

        if (Vector2.Distance(transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(path[0]);
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            isMoving = false;
        }
    }

    public void PositionCharacterOnLine(OverlayTile tile)
    {
        transform.position = new Vector2(tile.transform.position.x, tile.transform.position.y);
        standingOnTile.inTileUnit = null;
        standingOnTile = tile;
        tile.SetUnit(this);
    }

    [PunRPC]
    void Initialize(bool isMine)
    {
        if (isMine)
            PlayerController.me.units.Add(this);
        else
            GameManager.instance.GetOtherPlayer(PlayerController.me).units.Add(this);

        healthFillImage.fillAmount = 1.0f;

        // Set sprite variant
        spriteVisual.sprite = transform.position.x < 0 ? leftPlayerSprite : rightPlayerSprite;

        // Rotate the unit
        spriteVisual.transform.up = transform.position.x < 0 ? Vector3.left : Vector3.right;
    }

    public void GetInRangeTiles()
    {
        List<OverlayTile> allRange = rangeFinder.GetTilesInRange(standingOnTile.grid2DLocation, maxMoveDistance);
        rangeFinderTiles = new List<OverlayTile>();
        foreach (OverlayTile item in allRange)
        {
            if (item.inTileUnit && item.inTileUnit != this)
            {
                item.HideTile();
                continue;
            }
            rangeFinderTiles.Add(item);
            item.ShowTile();
        }
    }

    public void GetAttackInRangeTiles()
    {
        attackRangTiles = rangeFinder.GetTilesInRange(standingOnTile.grid2DLocation, maxAttackDistance);
    }

    private void RemoveArrows()
    {
        foreach (var item in rangeFinderTiles)
        {
            MapManager.instance.map[item.grid2DLocation].SetSprite(ArrowTranslator.ArrowDirection.None);
        }
    }

    public bool CanMove(OverlayTile tile)
    {
        tileToMove = tile;
        if (!rangeFinderTiles.Contains(tileToMove) || standingOnTile == tileToMove)
        {
            isMoving = false;
            GetInRangeTiles();
        }
        else
        {
            isMoving = true;
            tileToMove.HideTile();
        }
        return isMoving;
    }

    public bool CanSelect()
    {
        if (usedThisTurn)
            return false;
        else
            return true;
    }

    public bool CanAttack(Unit enemyUnit)
    {
        if (attackRangTiles.Exists(t => new Vector2(Mathf.RoundToInt(t.transform.position.x), Mathf.RoundToInt(t.transform.position.y)) == new Vector2(Mathf.RoundToInt(enemyUnit.transform.position.x), Mathf.RoundToInt(enemyUnit.transform.position.y))))
            return true;
        else
            return false;
    }

    public void ToggleSelect(bool selected)
    {
        selectedVisual.SetActive(selected);
        selecting = !selecting;
    }

    public void Attack(Unit unitToAttack)
    {
        usedThisTurn = true;
        unitToAttack.photonView.RPC("TakeDamage", PlayerController.enemy.photonPlayer, Random.Range(minDamage, maxDamage + 1));
    }

    [PunRPC]
    void TakeDamage(int damage)
    {
        curHp -= damage;

        if (curHp <= 0) photonView.RPC("Die", RpcTarget.All);
        else
        {
            photonView.RPC("UpdateHealthBar", RpcTarget.All, (float)curHp / (float)maxHp);
        }
    }

    [PunRPC]
    void UpdateHealthBar(float fillAmount)
    {
        healthFillImage.fillAmount = fillAmount;
    }

    [PunRPC]
    void Die()
    {
        if (!photonView.IsMine) {
            PlayerController.enemy.units.Remove(this);
            PlayerController.me.addCoin(500);
        }
        else
        {
            PlayerController.me.units.Remove(this);
            PlayerController.enemy.addCoin(500);

            // check the win condition
            GameManager.instance.CheckWinCondition();

            // destroy the unit across the network
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
