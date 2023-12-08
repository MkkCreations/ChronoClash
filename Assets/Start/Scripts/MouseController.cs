using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public GameObject cursor;
    private MapManager mapManager;
    public CharacterInfo character = null;
    public List<CharacterInfo> unitList = new List<CharacterInfo>();
    private bool moved = false;

    private void Start()
    {

        mapManager = GetComponentInChildren<MapManager>();

        unitList.AddRange(GameObject.FindObjectsOfType<CharacterInfo>());

        /* foreach (CharacterInfo u in unitList)
        {
            UpdateUnitGridPosition(u);
        } */

        character = unitList[0];
        character.selected = true;
        character.GetInRangeTiles();
    }

    void Update()
    {
        if (moved && !character.isMoving)
        {
            moved = false;
            // switch to next unit
            character.Deselect();
            character = unitList[(unitList.IndexOf(character) + 1) % unitList.Count];
            character.Select();
            character.GetInRangeTiles();
        }

        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue && character && !character.isMoving)
        {
            OverlayTile tile = hit.Value.collider.gameObject.GetComponentInChildren<OverlayTile>();
            cursor.transform.position = tile.transform.position;
            cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;

            character.tileToMove = MapManager.Instance.map[tile.grid2DLocation];

            if (Input.GetMouseButtonDown(0))
            {
                if (!character.rangeFinderTiles.Contains(character.tileToMove) || character.standingOnTile == character.tileToMove)
                {
                    character.isMoving = false;
                    character.GetInRangeTiles();
                    return;
                }
                else
                {
                    character.isMoving = true;
                    character.tileToMove.gameObject.GetComponent<OverlayTile>().HideTile();
                    moved = true;
                }
            }
        }
    }

    private static RaycastHit2D? GetFocusedOnTile()
    {
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

        RaycastHit2D[] hits = Physics2D.RaycastAll(mousePos2D, Vector2.zero);

        if (hits.Length > 0)
        {
            return hits.OrderByDescending(i => i.collider.transform.position.z).First();
        }

        return null;
    }



    void UpdateUnitGridPosition(CharacterInfo u)
    {
        OverlayTile t = mapManager.GetTileFromPoint(u.transform.position);

        if (t == null)
        {
            Debug.LogError("Unit not on the grid", u);
            return;
        }

        u.transform.position = t.transform.position;
    }

}
