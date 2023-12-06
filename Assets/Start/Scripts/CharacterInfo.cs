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
    public List<OverlayTile> rangeFinderTiles;

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

}