using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile standingOnTile;
    public float speed = 2f;
    public bool selected = false;

    public RangeFinder rangeFinder;
    public List<OverlayTile> rangeFinderTiles;
    private PathFinder pathFinder;
    private ArrowTranslator arrowTranslator;
    public List<OverlayTile> path;
    public bool isMoving = false;
    public OverlayTile tileToMove;

    private void Start()
    {
        rangeFinder = new RangeFinder();
        rangeFinderTiles = new List<OverlayTile>();
        pathFinder = new PathFinder();
        path = new List<OverlayTile>();
        arrowTranslator = new ArrowTranslator();

    }

    void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 2;

        if (selected && tileToMove)
        {
            if (rangeFinderTiles.Contains(tileToMove))
            {
                path = pathFinder.FindPath(standingOnTile, tileToMove, rangeFinderTiles);

                foreach (var item in rangeFinderTiles)
                {
                    MapManager.Instance.map[item.grid2DLocation].SetSprite(ArrowTranslator.ArrowDirection.None);
                }

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
        var step = speed * Time.deltaTime;

        float zIndex = path[0].transform.position.z;
        transform.position = Vector2.MoveTowards(transform.position, path[0].transform.position, step);
        transform.position = new Vector3(transform.position.x, transform.position.y, zIndex);

        if (Vector2.Distance(transform.position, path[0].transform.position) < 0.00001f)
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

    public void PositionCharacterOnLine(OverlayTile tile)
    {
        transform.position = new Vector3(tile.transform.position.x, tile.transform.position.y + 0.0001f, tile.transform.position.z);
        GetComponent<SpriteRenderer>().sortingOrder = tile.GetComponent<SpriteRenderer>().sortingOrder;
        standingOnTile = tile;
    }

    public void GetInRangeTiles()
    {
        rangeFinderTiles = rangeFinder.GetTilesInRange(standingOnTile.grid2DLocation, 3);

        foreach (OverlayTile item in rangeFinderTiles)
        {
            if (item.isWater()) continue;
            item.ShowTile();
        }
    }

    public void Select()
    {
        selected = true;
    }

    public void Deselect()
    {
        selected = false;
        foreach (OverlayTile item in rangeFinderTiles)
        {
            MapManager.Instance.map[item.grid2DLocation].SetSprite(ArrowTranslator.ArrowDirection.None);
            item.HideTile();
        }
    }

}