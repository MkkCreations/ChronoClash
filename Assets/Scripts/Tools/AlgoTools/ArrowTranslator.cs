using System;
using UnityEngine;

public class ArrowTranslator
{
    public enum ArrowDirection
    {
        None = 0,
        UpDown = 1,
        LeftRight = 2,
        TopLeft = 3,
        BottomLeft = 4,
        TopRight = 5,
        BottomRight = 6,
        UpFinished = 7,
        DownFinished = 8,
        LeftFinished = 9,
        RightFinished = 10
    }

    public ArrowDirection TranslateDirection(OverlayTile previousTile, OverlayTile currentTile, OverlayTile futureTile)
    {
        bool isFinal = futureTile == null;

        Vector2Int pastDirection = previousTile != null ? (currentTile.grid2DLocation - previousTile.grid2DLocation) : new Vector2Int(0, 0);
        Vector2Int futureDirection = futureTile != null ? (futureTile.grid2DLocation - currentTile.grid2DLocation) : new Vector2Int(0, 0);
        Vector2Int direction = pastDirection != futureDirection ? pastDirection + futureDirection : futureDirection;

        if (direction == new Vector2(0, 1) && !isFinal)
        {
            return ArrowDirection.UpDown;
        }

        if (direction == new Vector2(0, -1) && !isFinal)
        {
            return ArrowDirection.UpDown;
        }

        if (direction == new Vector2(1, 0) && !isFinal)
        {
            return ArrowDirection.LeftRight;
        }

        if (direction == new Vector2(-1, 0) && !isFinal)
        {
            return ArrowDirection.LeftRight;
        }

        if (direction == new Vector2(1, 1))
        {
            if (pastDirection.y < futureDirection.y)
                return ArrowDirection.BottomLeft;
            else
                return ArrowDirection.TopRight;
        }

        if (direction == new Vector2(-1, 1))
        {
            if (pastDirection.y < futureDirection.y)
                return ArrowDirection.BottomRight;
            else
                return ArrowDirection.TopLeft;
        }

        if (direction == new Vector2(1, -1))
        {
            if (pastDirection.y > futureDirection.y)
                return ArrowDirection.TopLeft;
            else
                return ArrowDirection.BottomRight;
        }

        if (direction == new Vector2(-1, -1))
        {
            if (pastDirection.y > futureDirection.y)
                return ArrowDirection.TopRight;
            else
                return ArrowDirection.BottomLeft;
        }

        if (direction == new Vector2(0, 1) && isFinal)
        {
            return ArrowDirection.UpFinished;
        }

        if (direction == new Vector2(0, -1) && isFinal)
        {
            return ArrowDirection.DownFinished;
        }

        if (direction == new Vector2(-1, 0) && isFinal)
        {
            return ArrowDirection.LeftFinished;
        }

        if (direction == new Vector2(1, 0) && isFinal)
        {
            return ArrowDirection.RightFinished;
        }

        return ArrowDirection.None;
    }
}