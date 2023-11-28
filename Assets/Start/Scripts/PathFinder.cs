using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


// This class is used to find the shortest path between two tiles
public class PathFinder
{
    private Dictionary<Vector2Int, OverlayTile> searchableTiles;

    public List<OverlayTile> FindPath(OverlayTile start, OverlayTile end, List<OverlayTile> inRangeTiles)

    {
        searchableTiles = new Dictionary<Vector2Int, OverlayTile>();

        List<OverlayTile> openList = new List<OverlayTile>();
        HashSet<OverlayTile> closedList = new HashSet<OverlayTile>();

        if (inRangeTiles.Count > 0)
        {
            foreach (var item in inRangeTiles)
            {
                searchableTiles.Add(item.grid2DLocation, MapManager.Instance.map[item.grid2DLocation]);
            }
        }
        else
        {
            searchableTiles = MapManager.Instance.map;
        }

        openList.Add(start);

        // While there are still tiles to check
        while (openList.Count > 0)
        {
            OverlayTile currentOverlayTile = openList.OrderBy(x => x.F).First();

            openList.Remove(currentOverlayTile);
            closedList.Add(currentOverlayTile);

            if (currentOverlayTile == end)
            {
                return GetFinishedList(start, end);
            }

            // Check all the neighbours of the current tile
            foreach (var tile in GetNeightbourOverlayTiles(currentOverlayTile))
            {
                // If the tile is already in the closed list or is not on the same level as the current tile, skip it
                if (tile.isWater() || closedList.Contains(tile) || Mathf.Abs(currentOverlayTile.transform.position.z - tile.transform.position.z) > 1)
                {
                    continue;
                }

                // Calculate the G and H values for the tile
                tile.G = GetManhattenDistance(start, tile);
                tile.H = GetManhattenDistance(end, tile);

                tile.Previous = currentOverlayTile;

                if (!openList.Contains(tile))
                {
                    openList.Add(tile);
                }
            }
        }

        return new List<OverlayTile>();
    }

    // Get the finished list of tiles
    private List<OverlayTile> GetFinishedList(OverlayTile start, OverlayTile end)
    {
        List<OverlayTile> finishedList = new List<OverlayTile>();
        OverlayTile currentTile = end;

        // Add all the tiles to the list
        while (currentTile != start)
        {
            finishedList.Add(currentTile);
            currentTile = currentTile.Previous;
        }

        finishedList.Reverse();

        return finishedList;
    }

    // Calculate the manhatten distance between two tiles
    private int GetManhattenDistance(OverlayTile start, OverlayTile tile)
    {
        return Mathf.Abs(start.gridLocation.x - tile.gridLocation.x) + Mathf.Abs(start.gridLocation.y - tile.gridLocation.y);
    }

    // Get all the neighbours of a tile
    private List<OverlayTile> GetNeightbourOverlayTiles(OverlayTile currentOverlayTile)
    {
        var map = MapManager.Instance.map;

        List<OverlayTile> neighbours = new List<OverlayTile>();

        // Right
        Vector2Int locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x + 1,
            currentOverlayTile.gridLocation.y
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        // Left
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x - 1,
            currentOverlayTile.gridLocation.y
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        // Top
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y + 1
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        // Bottom
        locationToCheck = new Vector2Int(
            currentOverlayTile.gridLocation.x,
            currentOverlayTile.gridLocation.y - 1
        );

        if (searchableTiles.ContainsKey(locationToCheck))
        {
            neighbours.Add(searchableTiles[locationToCheck]);
        }

        return neighbours;
    }
}
