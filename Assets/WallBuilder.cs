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
    
    public GameObject wallTile;
    public GameObject doorTile;

    List<Wall> m_WallList;

    Camera m_MainCam;
    
    ObjectMap m_ColliderTileMap => m_MapData.colliderObjectContainer;
    
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

            if (m_ColliderTileMap.HasObject(tilePos))
            {
                return;
            }

            var wallObject = Instantiate(wallTile, m_MapData.colliderObjectContainer.transform);
            
            wallObject.transform.position = PathfindingManager.tilemapGrid.CellToWorld(tilePos);

            var newWall = new Wall(tilePos, wallObject);
            
            m_WallList.Add(newWall);
            
             m_ColliderTileMap.SetObject(tilePos, wallObject);
        }
        
        if (Mouse.current.rightButton.wasPressedThisFrame)
        {
            var mousePos = m_MainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            var tilePos = new Vector3Int(Mathf.FloorToInt(mousePos.y), Mathf.FloorToInt(mousePos.x), 0);
            
            if (!m_ColliderTileMap.HasObject(tilePos) && m_WallList.All(wall => wall.tilePos != tilePos))
            {
                return;
            }

            var wallToRemove = m_WallList.First(wall => wall.tilePos == tilePos);
            
            m_WallList.Remove(wallToRemove);
            
            m_ColliderTileMap.SetObject(tilePos, null);
        }

        if (Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            var mousePos = m_MainCam.ScreenToWorldPoint(Mouse.current.position.ReadValue());

            var tilePos = new Vector3Int(Mathf.FloorToInt(mousePos.y), Mathf.FloorToInt(mousePos.x), 0);

            if (m_ColliderTileMap.HasObject(tilePos))
            {
                return;
            }

            var wallObject = Instantiate(doorTile, m_MapData.colliderObjectContainer.transform);
            
            wallObject.transform.position = PathfindingManager.tilemapGrid.CellToWorld(tilePos);

            var newWall = new Wall(tilePos, wallObject);
            
            m_WallList.Add(newWall);
            
            m_ColliderTileMap.SetObject(tilePos, wallObject);
        }
    }
}
