using System;
using System.Collections.Generic;
using PathFinding.Scripts.UIManagers;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapGenerator : MonoBehaviour
{
    [SerializeField]
    MapGridDataManager m_GridData;
    
    [SerializeField]
    MapDataScriptableObject m_MapData;

    [SerializeField]
    GameObject m_MapBoundingCollider;
    
    [SerializeField]
    float m_OffsetX;

    [SerializeField]
    float m_OffsetY;

    [SerializeField]
    float m_Magnification;

    // Start is called before the first frame update
    void Start()
    {
        GenerateTileMap();
    }

    public void SetMapData(MapDataScriptableObject newMapData)
    {
        m_MapData = newMapData;
    }

    void GenerateTileMap()
    {
        var width = m_GridData.mapWidth;
        var height = m_GridData.mapHeight;
        
        var pathTilemap = m_GridData.pathTileMap;
        var colliderTileMap = m_GridData.colliderObjectContainer;
        
        var pathTiles = new Tuple<List<Vector3Int>, List<TileBase>>(new List<Vector3Int>(), new List<TileBase>());
        var colliderTiles = new Tuple<List<Vector3Int>, List<GameObject>>(new List<Vector3Int>(), new List<GameObject>());
        
        for (int xCoord = 0; xCoord < width; xCoord++)
        {
            for (int yCoord = 0; yCoord < height; yCoord++)
            {
                var perlinNoise = GetPerlinNoise(xCoord, yCoord);

                foreach (var tileData in m_MapData.mapTiles)
                {
                    if (perlinNoise <= tileData.percentage)
                    {
                        if (tileData.prefab != null)
                        {
                            var obstacleObject = Instantiate(tileData.prefab, m_GridData.colliderObjectContainer.transform);
                            var currentPosInGrid = new Vector3Int(xCoord, yCoord, 0);
                            obstacleObject.transform.position = PathfindingManager.tilemapGrid.CellToWorld(currentPosInGrid);
                            
                            colliderTiles.Item1.Add(currentPosInGrid);
                            colliderTiles.Item2.Add(obstacleObject);
                            break;
                        }
                        else
                        {
                            pathTiles.Item1.Add(new Vector3Int(xCoord,yCoord,0));
                            pathTiles.Item2.Add(tileData.tile);
                            break;
                        }
                    }
                }
            }
        }
        
        SetBoundingColliders(width, height);
        
        colliderTileMap.SetObjects(colliderTiles.Item1.ToArray(), colliderTiles.Item2.ToArray());
        pathTilemap.SetTiles(pathTiles.Item1.ToArray(),pathTiles.Item2.ToArray());
    }

    void SetBoundingColliders(int width, int height)
    {
        var east = Instantiate(m_MapBoundingCollider, m_GridData.boundingBoxContainer.transform);
        var eastAvg = new Vector3Int(-1, height, 0);
        east.transform.localScale = new Vector3(1, height, 1);
        east.transform.position = PathfindingManager.tilemapGrid.CellToWorld(eastAvg);
        
        var south = Instantiate(m_MapBoundingCollider, m_GridData.boundingBoxContainer.transform);
        var southAvg = new Vector3Int(-1, -1, 0);
        south.transform.localScale = new Vector3(width, 1, 1);
        south.transform.position = PathfindingManager.tilemapGrid.CellToWorld(southAvg);
        
        var north = Instantiate(m_MapBoundingCollider, m_GridData.boundingBoxContainer.transform);
        var northAvg = new Vector3Int(width, -1, 0);
        north.transform.localScale = new Vector3(width, 1, 1);
        north.transform.position = PathfindingManager.tilemapGrid.CellToWorld(northAvg);
        
        var west = Instantiate(m_MapBoundingCollider, m_GridData.boundingBoxContainer.transform);
        var westAvg = new Vector3Int(-1, -1, 0);
        west.transform.localScale = new Vector3(1, height, 1);
        west.transform.position = PathfindingManager.tilemapGrid.CellToWorld(westAvg);
    }

    float GetPerlinNoise(int x, int y)
    {
        var xNoise = (x - m_OffsetX) / m_Magnification;
        var yNoise = (y - m_OffsetY) / m_Magnification;
        
        
        var rawPerlin = Mathf.PerlinNoise(xNoise, yNoise);

        return Mathf.Clamp(rawPerlin, 0, 1);
    }
}
