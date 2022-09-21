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
    [SerializeField]
    Tilemap colliderTilemap;

    GridLayout m_UnifiedGrid;
    
    List<Room> m_Rooms;

    GenericGrid<PathNode> m_Grid;
    
    // Start is called before the first frame update
    void Start()
    {
        m_Grid = PathfindingManager.pathfinding.NodeGrid;
        m_Rooms = new List<Room>();

        m_UnifiedGrid = PathfindingManager.unifiedGrid;
        
        Tilemap.tilemapTileChanged += PaintBucketFill;
    }

    public void PaintBucketFill(Tilemap tilemap, Tilemap.SyncTile[] changedTiles)
    {
        if (tilemap != colliderTilemap)
        {
            return;
        }

        var changedCells = new List<PathNode>();
        
        foreach (var tile in changedTiles)
        {
            var cell = m_Grid.GetGridObject(m_UnifiedGrid.CellToWorld(tile.position));
            changedCells.Add(cell);
            
            if (m_Rooms.Any(room => room.ContainsCell(cell)))
            {
                Debug.Log("Room Removed");

                m_Rooms.RemoveAll(room => room.ContainsCell(cell));
            }
        }

        var roomCreationTask = new Task(() => CreateRoomsThread(changedCells));
        
        roomCreationTask.Start();
    }

    void CreateRoomsThread(IEnumerable<PathNode> changedTiles)
    {
        foreach (var tile in changedTiles)
        {
            var newRoom = new Room();

            var x = tile.x;
            var y = tile.y;
            
            Debug.Log(x + " detecting " + y );
            
            if (m_Grid.GetGridObject(x - 1, y).isWalkable && !m_Rooms.Any(room => room.ContainsCell(x - 1, y)))
            {
                RecursiveFill(ref newRoom, x - 1, y);
                Debug.Log("New Room created");
                m_Rooms.Add(newRoom);
            }


            if (m_Grid.GetGridObject(x + 1, y).isWalkable && !m_Rooms.Any(room => room.ContainsCell(x + 1, y)))
            {
                newRoom = new Room();
                RecursiveFill(ref newRoom, x + 1, y);
                Debug.Log("New Room created x+1");
                m_Rooms.Add(newRoom);
            }

            if (m_Grid.GetGridObject(x, y - 1).isWalkable && !m_Rooms.Any(room => room.ContainsCell(x, y - 1)))
            {
                newRoom = new Room();
                RecursiveFill(ref newRoom, x, y - 1);
                Debug.Log("New Room created y-1");

                m_Rooms.Add(newRoom);
            }

            if (m_Grid.GetGridObject(x, y + 1).isWalkable && !m_Rooms.Any(room => room.ContainsCell(x, y + 1)))
            {
                newRoom = new Room();
                RecursiveFill(ref newRoom, x, y + 1);
                Debug.Log("New Room created y+1");

                m_Rooms.Add(newRoom);
            }
        }
    }

    void RecursiveFill(ref Room currentRoom, int x, int y)
    {
        if (x >= m_Grid.Width || x <= 0 || y <= 0 || y >= m_Grid.Height)
        {
            currentRoom.isOutside = true;
            return;
        }

        if (currentRoom.ContainsCell(x,y))
        {
            return;
        }
        
        var currentCell = m_Grid.GetGridObject(x, y, true);

        if (!currentCell.isWalkable || currentCell.isDoor)
        {
            return;
        }
        
        Debug.DrawLine(m_Grid.GetWorldPosition(x,y), m_Grid.GetWorldPosition(x,y)+Vector3.one, Color.blue, 1000);
        
        currentRoom.AssignCell(currentCell);
        
        RecursiveFill(ref currentRoom, x, y+1);
        RecursiveFill(ref currentRoom, x+1, y);
        RecursiveFill(ref currentRoom, x-1, y);
        RecursiveFill(ref currentRoom, x, y-1);
    }
}
