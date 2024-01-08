using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using System.IO;
using System.ComponentModel;

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

    private PathFinder pathFinder;
    public List<OverlayTile> path;

    private void Start()
    {
        rangeFinder = new RangeFinder();
        rangeFinderTiles = new List<OverlayTile>();
        pathFinder = new PathFinder();
        path = new List<OverlayTile>();
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
        rangeFinderTiles = rangeFinder.GetTilesInRange(standingOnTile.grid2DLocation, maxMoveDistance);

        foreach (OverlayTile item in rangeFinderTiles)
        {
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

    public bool CanMove(Vector3 movePos)
    {
        if (Vector3.Distance(transform.position, movePos) <= maxMoveDistance)
            return true;
        else
            return false;
    }

    public bool CanAttack(Vector3 attackPos)
    {
        if (Vector3.Distance(transform.position, attackPos) <= maxAttackDistance)
            return true;
        else
            return false;
    }

    public void ToggleSelect(bool selected)
    {
        selectedVisual.SetActive(selected);
    }

    public void Move(Vector3 targetPos)
    {
        usedThisTurn = true;

        Vector3 dir = (transform.position - targetPos).normalized;
        spriteVisual.transform.up = dir;

        StartCoroutine(MoveOverTime());

        IEnumerator MoveOverTime()
        {
            while(transform.position != targetPos)
            {
                transform.position = Vector3.MoveTowards(transform.position, targetPos, moveSpeed * Time.deltaTime);
                yield return null;
            }
        }
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
        if (!photonView.IsMine)
            PlayerController.enemy.units.Remove(this);
        else
        {
            PlayerController.me.units.Remove(this);

            // check the win condition
            GameManager.instance.CheckWinCondition();

            // destroy the unit across the network
            PhotonNetwork.Destroy(gameObject);
        }
    }
}
