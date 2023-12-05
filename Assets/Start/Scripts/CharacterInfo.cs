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
    public float speed;
    private bool selected = false;

    private RangeFinder rangeFinder;
    private List<OverlayTile> rangeFinderTiles;

    private void Start()
    {
        rangeFinder = new RangeFinder();
        rangeFinderTiles = new List<OverlayTile>();

    }

    void Update()
    {
       

    }

    public void Select()
    {
        selected = true;
        GetInRangeTiles();
    }

    public List<OverlayTile> GetRangeFinderTiles()
    {
        return rangeFinderTiles;
    }

    public void setRangeFinderTiles(List<OverlayTile> list)
    {
        this.rangeFinderTiles = list;
    }

    public void Deselect()
    {
        selected = false;
        foreach (OverlayTile item in rangeFinderTiles)
        {
            item.HideTile();
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
        rangeFinderTiles = rangeFinder.GetTilesInRange(new Vector2Int(standingOnTile.gridLocation.x, standingOnTile.gridLocation.y), 3);

        foreach (OverlayTile item in rangeFinderTiles)
        {
            if (item.isWater()) continue;
            item.ShowTile();
        }
    }
}