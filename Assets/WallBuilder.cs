using System;
using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class WallBuilder : MonoBehaviour
{
    public Tilemap ObstaclesTileMap;
    public Tilemap PathTilemap;

    public TileBase WallTile;

    List<Wall> m_WallList;

    Camera m_MainCam;

    public static event Action<Vector3Int> WallPlaced;
    
    void Start()
    {
        m_WallList = new List<Wall>();
        m_MainCam = Camera.main;
    }

    void Update()
    { 
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            var mousePos = m_MainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            var tilePos = new Vector3Int(Mathf.FloorToInt(mousePos.y), Mathf.FloorToInt(mousePos.x), 0);

            if (ObstaclesTileMap.HasTile(tilePos))
            {
                return;
            }

            var newWall = new Wall(new[] {tilePos}, WallTile);
            
            m_WallList.Add(newWall);
            
            ObstaclesTileMap.SetTile(tilePos, WallTile);
            
            Debug.Log(tilePos);
            
            WallPlaced?.Invoke(tilePos);
        }
    }
}
