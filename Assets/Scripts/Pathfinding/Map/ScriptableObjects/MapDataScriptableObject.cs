using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;


[CreateAssetMenu(fileName = "Map Data", menuName = "MapData/Map Data", order = 1)]
public class MapDataScriptableObject : ScriptableObject
{
    [SerializeField]
    public List<MapTile> mapTiles = new List<MapTile>();
}
