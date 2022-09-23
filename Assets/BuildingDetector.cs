using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Assets.Grid.Scripts.Map.Logic;
using DefaultNamespace;
using JetBrains.Annotations;
using PathFinding.Scripts.UIManagers;
using UnityEngine;
using UnityEngine.Tilemaps;

public class BuildingDetector : MonoBehaviour
{
    List<Room> m_Rooms;

    GenericGrid<PathNode> m_Grid;

    [SerializeField]
    bool paintRooms;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Grid = PathfindingManager.pathfinding.NodeGrid;
        m_Rooms = new List<Room>();

        PathfindingManager.OnPathfindingEdited += PaintBucketFill;
    }

    void Update()
    {
        if (paintRooms)
        {
            foreach (var room in m_Rooms)
            {
                var color = Color.red;
                if (room.isOutside)
                {
                    color = Color.blue;
                }
                
                foreach (var node in room.pathNodes)
                {
                    var wPos = m_Grid.GetWorldPosition(node.x, node.y);
                    Debug.DrawLine(wPos, wPos+Vector3.one, color);
                }
            }
        }
    }

    public void PaintBucketFill(List<PathNode> changedCells)
    {
        foreach (var tile in changedCells)
        {
            if (m_Rooms.Any(room => room.ContainsWall(tile)))
            {
                foreach (var room in m_Rooms.FindAll(room => room.ContainsWall(tile)))
                {
                    Debug.Log("Room Removed");

                    m_Rooms.Remove(room);
                }
            }
            else if (m_Rooms.Any(room => room.ContainsCell(tile)))
            {
                Debug.Log("Room Removed");

                m_Rooms.RemoveAll(room => room.ContainsCell(tile));
            }
        }

        var roomCreationTask = new Task(() => CreateRoomsThread(changedCells));
        
        roomCreationTask.Start();
    }

    void CreateRoomsThread(IEnumerable<PathNode> changedTiles)
    {
        foreach (var tile in changedTiles)
        {
            var x = tile.x;
            var y = tile.y;

            if (tile.isWalkable && !m_Rooms.Any(room => room.ContainsCell(x, y)))
            {
                var newRoom = new Room();
                RecursiveFill(ref newRoom, x - 1, y);
                if (!newRoom.isOutside && !newRoom.isEmpty)
                {
                    Debug.Log("New Room created on root");
                    m_Rooms.Add(newRoom);
                }
            }
            
            var currentGridObject = m_Grid.GetGridObject(x - 1, y);
            
            if (currentGridObject != null && currentGridObject.isWalkable && !m_Rooms.Any(room => room.ContainsCell(x - 1, y)))
            {
                var newRoom = new Room();
                RecursiveFill(ref newRoom, x - 1, y);
                if (!newRoom.isOutside && !newRoom.isEmpty)
                {
                    Debug.Log("New Room created");
                    m_Rooms.Add(newRoom);
                }
            }
            
            currentGridObject = m_Grid.GetGridObject(x + 1, y);

            if (currentGridObject != null && currentGridObject.isWalkable && !m_Rooms.Any(room => room.ContainsCell(x + 1, y)))
            {
                var newRoom = new Room();
                RecursiveFill(ref newRoom, x + 1, y);
                if (!newRoom.isOutside && !newRoom.isEmpty)
                {
                    Debug.Log("New Room created x+1");
                    m_Rooms.Add(newRoom);
                }
            }

            currentGridObject = m_Grid.GetGridObject(x, y-1);

            if (currentGridObject != null && currentGridObject.isWalkable && !m_Rooms.Any(room => room.ContainsCell(x, y - 1)))
            {
                var newRoom = new Room();
                RecursiveFill(ref newRoom, x, y - 1);

                if (!newRoom.isOutside && !newRoom.isEmpty)
                {
                    Debug.Log("New Room created y-1");
                    m_Rooms.Add(newRoom);
                }
            }

            currentGridObject = m_Grid.GetGridObject(x, y+1);

            if (currentGridObject != null && currentGridObject.isWalkable && !m_Rooms.Any(room => room.ContainsCell(x, y + 1)))
            {
                var newRoom = new Room();
                RecursiveFill(ref newRoom, x, y + 1);

                if (!newRoom.isOutside && !newRoom.isEmpty)
                {
                    Debug.Log("New Room created y+1");
                    m_Rooms.Add(newRoom);
                }
            }
        }

        if (!m_Rooms.Any(room => room.isOutside))
        {
            var outsideCells = m_Grid.GetAllCellList().Where(cell => cell.isWalkable && !cell.isDoor && !m_Rooms.Any(room => room.ContainsCell(cell)));
            var boundingWalls = m_Grid.GetAllCellList().Where(wall => !wall.isWalkable || wall.isDoor && !m_Rooms.Any(room => room.ContainsWall(wall)));

            var outsideRoom = new Room();
            outsideRoom.isOutside = true;
            outsideRoom.AssignCells(outsideCells);

            foreach (var room in m_Rooms)
            {
                foreach (var wall in room.GetOutsideWalls())
                {
                    if (m_Rooms.Any(compareRoom => compareRoom != room && compareRoom.ContainsWall(wall)))
                    {
                        continue;
                    }
                    
                    outsideRoom.AssignWall(wall);
                }
            }

            m_Rooms.Add(outsideRoom);
        }
    }

    void RecursiveFill(ref Room currentRoom, int x, int y)
    {
        if (currentRoom.isOutside)
        {
            return;
        }
        if (x >= m_Grid.Width || x < 0 || y < 0 || y >= m_Grid.Height)
        {
            currentRoom.isOutside = true;
            currentRoom.ClearTiles();
            return;
        }

        if (currentRoom.ContainsCell(x,y))
        {
            return;
        }
        
        var currentCell = m_Grid.GetGridObject(x, y, true);

        if (!currentCell.isWalkable || currentCell.isDoor)
        {
            currentRoom.AssignWall(currentCell);
            return;
        }

        currentRoom.AssignCell(currentCell);
        
        RecursiveFill(ref currentRoom, x, y+1);
        RecursiveFill(ref currentRoom, x+1, y);
        RecursiveFill(ref currentRoom, x-1, y);
        RecursiveFill(ref currentRoom, x, y-1);
    }
}
