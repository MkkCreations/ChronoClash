using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class OverlayTile : MonoBehaviour
{
    // G = distance from start node
    public int G;
    // H = distance from end node
    public int H;
    // F = G + H (total cost)
    public int F { get { return G + H; } }

    // The previous tile in the path
    public OverlayTile Previous;

    // The grid location of the tile
    public Vector3Int gridLocation;
    public Vector2Int grid2DLocation { get { return new Vector2Int(gridLocation.x, gridLocation.y); } }

    public List<Sprite> arrows;

    private void Update()
    {
        // Hide tile when clicked
        if (Input.GetMouseButtonDown(0))
        {
            HideTile();
        }
    }

    public void HideTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0);
    }

    public void ShowTile()
    {
        gameObject.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.6f);
    }

    /// <summary>
    ///     This function tells us if the tile is water
    /// </summary>
    /// <returns>
    ///     bool
    /// </returns>
    public bool isWater()
    {
        if (transform.position.z == 1) { return true; }
        return false;
    }

    /// <summary>
    ///     <para>ArrowDirection</para>
    ///     This function sets the sprit in the tile in the rang by giving it the direction from the ArrowDirection enum. If the direction is None it turns the sprid color to 0
    /// </summary> 
    public void SetSprite(ArrowTranslator.ArrowDirection d)
    {
        SpriteRenderer sprite = GetComponentsInChildren<SpriteRenderer>()[1];
        if (d == ArrowTranslator.ArrowDirection.None)
        {
            sprite.color = new Color(1, 1, 1, 0);
        }
        else
        {
            sprite.color = new Color(1, 1, 1, 1);
            sprite.sprite = arrows[(int)d];
            sprite.sortingOrder = 1;
        }
    }
}
