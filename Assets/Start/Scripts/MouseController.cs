using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class MouseController : MonoBehaviour
{

    public GameObject cursor;
    public CharacterInfo character = null;
    private PathFinder pathFinder;
    private ArrowTranslator arrowTranslator;
    private List<OverlayTile> path;
    private bool isMoving;


    private void Start()
    {
        pathFinder = new PathFinder();
        arrowTranslator = new ArrowTranslator();

        path = new List<OverlayTile>();
        isMoving = false;
    }

    void Update()
    {

        RaycastHit2D? hit = GetFocusedOnTile();

        if (hit.HasValue)
        {
            CharacterInfo characterNew = hit.Value.collider.gameObject.GetComponent<CharacterInfo>();
            if (characterNew != null && characterNew != character)
            {
                character = characterNew;
                character.Select();
                character.GetInRangeTiles();
            } else if (characterNew == null && character != null)
            {
                character.Deselect();
                character = null;
            }

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
                        character.GetInRangeTiles();
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

    private void MoveAlongPath()
    {
        var step = character.speed * Time.deltaTime;

        float zIndex = path[0].transform.position.z;
        transform.position = Vector2.MoveTowards(transform.position, path[0].transform.position, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);

        if (Vector2.Distance(transform.position, path[0].transform.position) < 0.00001f)
        {
            character.PositionCharacterOnLine(path[0]);
            path.RemoveAt(0);
        }

        if (path.Count == 0)
        {
            character.GetInRangeTiles();
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

}
