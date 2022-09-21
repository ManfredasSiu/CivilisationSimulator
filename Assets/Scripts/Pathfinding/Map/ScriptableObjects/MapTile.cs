using System;
using UnityEngine;
using UnityEngine.Tilemaps;

[Serializable]
public class MapTile
{
    [Range(0, 1)]
    public float percentage;

    public TileBase tile;
}
