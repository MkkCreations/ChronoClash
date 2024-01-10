using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Tilemaps;

public class MapManager: MonoBehaviour
{
    public static MapManager instance;

    public GameObject overlayPrefab;
    public GameObject overlayContainer;

    public Dictionary<Vector2Int, OverlayTile> map;
    public bool ignoreBottomTiles;

    public OverlayTile HoveredTile = null;

    // Awake is called before Start
    private void Awake()
    {
        // Singleton pattern
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            instance = this;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        // Get all tilemaps in the scene
        var tileMaps = gameObject.transform.GetComponentsInChildren<Tilemap>().OrderByDescending(x => x.GetComponent<TilemapRenderer>().sortingOrder);
        /*
            Create a dictionary of all tiles in the scene
            Key: Vector2Int of the tile's grid location
            Value: OverlayTile component of the tile
        */
        map = new Dictionary<Vector2Int, OverlayTile>();

        foreach (var t in overlayContainer.GetComponentsInChildren<OverlayTile>())
        {
            map.Add(t.grid2DLocation, t);
        }

        Debug.Log(map.Count);

        /*
            Loop through all tiles in the tilemap
            If the tile is not in the dictionary, instantiate an overlay tile
            Set the overlay tile's position to the tile's position
            Add the tile to the dictionary
        */
        /*foreach (var tm in tileMaps)
        {
            BoundsInt bounds = tm.cellBounds;
            for (int z = bounds.max.z; z >= bounds.min.z; z--)
            {
                for (int y = bounds.min.y -1 ; y < bounds.max.y; y++)
                {
                    for (int x = bounds.min.x -1 ; x < bounds.max.x; x++)
                    {
                        // If the tile is on the bottom layer and ignoreBottomTiles is true, skip the tile
                        if (z == 0 && ignoreBottomTiles)
                        {
                            return;
                        }
                        // If the tile is not empty, instantiate an overlay tile
                        if (!tm.HasTile(new Vector3Int(x, y, z)))
                        {
                            continue;
                        }
                        // If the tile is not in the dictionary, instantiate an overlay tile
                        // Set the overlay tile's position to the tile's position
                        if (!map.ContainsKey(new Vector2Int(x, y)))
                        {

                            var overlayTile = Instantiate(overlayPrefab, overlayContainer.transform);
                            overlayTile.name = string.Format("Tile {0}{1}", x, y);
                            var cellWorldPosition = tm.GetCellCenterWorld(new Vector3Int(x, y, z));
                            overlayTile.transform.position = new Vector3(cellWorldPosition.x, cellWorldPosition.y, cellWorldPosition.z + 1);
                            overlayTile.gameObject.GetComponent<OverlayTile>().gridLocation = new Vector3Int(x, y, z);
                            map.Add(new Vector2Int(x, y), overlayTile.gameObject.GetComponent<OverlayTile>());
                        }

                    }
                }
            }
        }*/
    
    }

    public List<OverlayTile> GetSurroundingTiles(Vector2Int originTile)
    {
        var surroundingTiles = new List<OverlayTile>();


        Vector2Int TileToCheck = new Vector2Int(originTile.x + 1, originTile.y);
        if (map.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                surroundingTiles.Add(map[TileToCheck]);
        }

        TileToCheck = new Vector2Int(originTile.x - 1, originTile.y);
        if (map.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                surroundingTiles.Add(map[TileToCheck]);
        }

        TileToCheck = new Vector2Int(originTile.x, originTile.y + 1);
        if (map.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                surroundingTiles.Add(map[TileToCheck]);
        }

        TileToCheck = new Vector2Int(originTile.x, originTile.y - 1);
        if (map.ContainsKey(TileToCheck))
        {
            if (Mathf.Abs(map[TileToCheck].transform.position.z - map[originTile].transform.position.z) <= 1)
                surroundingTiles.Add(map[TileToCheck]);
        }

        return surroundingTiles;
    }

    public OverlayTile GetTileFromMouse()
    {

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit2D hit = Physics2D.GetRayIntersection(ray, Mathf.Infinity);

        if (hit.collider != null)
        {
            HoveredTile = hit.collider.GetComponent<OverlayTile>();
            return HoveredTile;
            // raycast hit this gameobject
        }
        return null;
    }

    public OverlayTile GetTileFromPoint(Vector3 point)
    {

        Collider2D c = Physics2D.OverlapPoint(point);

        if (c != null)
        {
            return c.GetComponent<OverlayTile>();
        }
        return null;
    }
}

