using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using PathFinding.Scripts.UIManagers;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Tilemaps;

public class WallBuilder : MonoBehaviour
{
    [SerializeField]
    MapGridDataManager m_MapData;
    
    public TileBase WallTile;

    List<Wall> m_WallList;

    Camera m_MainCam;
    
    Tilemap m_ColliderTileMap => m_MapData.colliderTileMap;
    
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

            if (m_ColliderTileMap.HasTile(tilePos))
            {
                return;
            }

            var newWall = new Wall(tilePos, WallTile);
            
            m_WallList.Add(newWall);
            
            m_ColliderTileMap.SetTile(tilePos, WallTile);
            
            Debug.Log(tilePos);
        }
        
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            var mousePos = m_MainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            var tilePos = new Vector3Int(Mathf.FloorToInt(mousePos.y), Mathf.FloorToInt(mousePos.x), 0);

            if (!m_ColliderTileMap.HasTile(tilePos) && m_WallList.All(wall => wall.tilePos != tilePos))
            {
                return;
            }

            var wallToRemove = m_WallList.First(wall => wall.tilePos == tilePos);
            
            m_WallList.Remove(wallToRemove);
            
            m_ColliderTileMap.SetTile(tilePos, null);
            
            Debug.Log(tilePos);
        }
    }
}
