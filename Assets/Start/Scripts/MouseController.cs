using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public GameObject cursor;
    private MapManager mapManager;
    public CharacterInfo character = null;
    public List<CharacterInfo> unitList = new List<CharacterInfo>();
    private PathFinder pathFinder;
    private ArrowTranslator arrowTranslator;
    private RangeFinder rangeFinder;
    private List<OverlayTile> rangeFinderTiles;
    private List<OverlayTile> path;
    private bool isMoving;


    private void Start()
    {
        mapManager = GetComponentInChildren<MapManager>();
        pathFinder = new PathFinder();
        arrowTranslator = new ArrowTranslator();

        path = new List<OverlayTile>();
        isMoving = false;

        unitList.AddRange(GameObject.FindObjectsOfType<CharacterInfo>());

        foreach (CharacterInfo u in unitList)
        {
            UpdateUnitGridPosition(u);
        }
    }

    void Update()
    {

        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
           

            List<OverlayTile> rangeFinderTiles = new List<OverlayTile>();
            OverlayTile tile = hit.Value.collider.gameObject.GetComponent<OverlayTile>();
            cursor.transform.position = tile.transform.position;
            cursor.gameObject.GetComponent<SpriteRenderer>().sortingOrder = tile.transform.GetComponent<SpriteRenderer>().sortingOrder;

            if (character != null)
            { 
                 rangeFinderTiles = character.GetRangeFinderTiles();

                if (rangeFinderTiles.Contains(tile) && !isMoving)
                {
                    path = pathFinder.FindPath(character.standingOnTile, tile, rangeFinderTiles);

                    foreach (var item in rangeFinderTiles)
                    {
                        MapManager.Instance.map[item.grid2DLocation].SetSprite(ArrowTranslator.ArrowDirection.None);
                    }

                    for (int i = 0; i < path.Count; i++)
                    {
                        var previousTile = i > 0 ? path[i - 1] : character.standingOnTile;
                        var futureTile = i < path.Count - 1 ? path[i + 1] : null;

                        var arrow = arrowTranslator.TranslateDirection(previousTile, path[i], futureTile);
                        path[i].SetSprite(arrow);
                    }
                }
            }
            if (Input.GetMouseButtonDown(0))
            {
                if (character != null)
                {
                    if (!rangeFinderTiles.Contains(tile) || character.standingOnTile == tile)
                    {
                        isMoving = false;
                        GetInRangeTiles();
                        return;
                    }
                    else
                    {
                        isMoving = true;
                        tile.gameObject.GetComponent<OverlayTile>().HideTile();
                    }
                }
            }
            if (path.Count > 0 && isMoving)
            {
                MoveAlongPath();
            }
        }
    }

    private void OnMouseDown()
    {
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D? hit = GetFocusedOnTile();

            if (hit.HasValue)
            {
                CharacterInfo characterNew = null;
                try
                {
                    characterNew = hit.Value.collider.gameObject.GetComponent<CharacterInfo>();
                }
                catch
                {
                    print("no new character");
                }
                if (characterNew != null)
                {
                    character = characterNew;
                    character.Select();
                    GetInRangeTiles();
                }
                else if (characterNew == null && character != null)
                {
                    character.Deselect();
                    character = null;
                }
            }
        }
        
    }

    private void MoveAlongPath()
    {
        var step = character.speed * Time.deltaTime;

        float zIndex = path[0].transform.position.z;
        character.transform.position = Vector2.MoveTowards(character.transform.position, path[0].transform.position, step);
        character.transform.position = new Vector3(character.transform.position.x, transform.position.y, zIndex);

        if (Vector2.Distance(character.transform.position, path[0].transform.position) < 0.00001f)
        {
            PositionCharacterOnLine(path[0]);
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            GetInRangeTiles();
            isMoving = false;
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

    public void PositionCharacterOnLine(OverlayTile tile)
    {
        character.transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        character.standingOnTile = tile;
    }


    public void GetInRangeTiles()
    {
        rangeFinderTiles = rangeFinder.GetTilesInRange(new Vector2Int(character.standingOnTile.gridLocation.x, character.standingOnTile.gridLocation.y), 3);

        foreach (OverlayTile item in rangeFinderTiles)
        {
            if (item.isWater()) continue;
            item.ShowTile();
        }
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
