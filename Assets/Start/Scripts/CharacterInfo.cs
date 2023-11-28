using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class CharacterInfo : MonoBehaviour
{
    public OverlayTile standingOnTile;

    private void Update()
    {
        GetComponent<SpriteRenderer>().sortingOrder = 2;
    }
}
