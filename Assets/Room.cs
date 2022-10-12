using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DefaultNamespace
{
    public class Room
    {
        int m_RoomId;

        List<PathNode> m_RoomCells;
        
        List<PathNode> m_RoomWalls;

        public List<PathNode> pathNodes => m_RoomCells;
        
        public List<PathNode> wallNodes => m_RoomWalls;

        public bool isOutside = false;

        public bool isEmpty => !m_RoomCells.Any();

        public Room()
        {
            m_RoomCells = new List<PathNode>();
            m_RoomWalls = new List<PathNode>();
        }
        
        public bool ContainsCell(int x, int y)
        {
            for (int i = 0; i < m_RoomCells.Count; i++)
            {
                if (m_RoomCells[i].x == x && m_RoomCells[i].y == y)
                    return true;
            }
            
            return false;
        }
        
        public bool ContainsCell(PathNode cell)
        {
            for (int i = 0; i < m_RoomCells.Count; i++)
            {
                if (m_RoomCells[i] == cell)
                    return true;
            }
            
            return false;
        }
        
        public bool ContainsWall(PathNode wall)
        {
            for (int i = 0; i < m_RoomWalls.Count; i++)
            {
                if (m_RoomWalls[i] == wall)
                    return true;
            }

            return false;
        }
        
        public List<PathNode> GetOutsideWalls()
        {
            return m_RoomWalls.Where(wall => !IsInsideWall(wall)).ToList();
        }
        
        public void AssignCell(PathNode cell)
        {
            m_RoomCells.Add(cell);
        }
        
        public void AssignCells(IEnumerable<PathNode> cells)
        {
            m_RoomCells.AddRange(cells);
        }
        
        public void AssignWall(PathNode wall)
        {
            m_RoomWalls.Add(wall);
        }
        
        public void AssignWalls(IEnumerable<PathNode> walls)
        {
            m_RoomWalls.AddRange(walls);
        }

        public void ClearTiles()
        {
            m_RoomCells.Clear();
        }
        
        bool IsInsideWall(PathNode wall)
        {
            if ((m_RoomWalls.Any(roomWall => roomWall.x == wall.x + 1) || m_RoomCells.Any(roomCell => roomCell.x == wall.x + 1)) 
                && (m_RoomWalls.Any(roomWall => roomWall.x == wall.x - 1) || m_RoomCells.Any(roomCell => roomCell.x == wall.x - 1)) 
                && (m_RoomWalls.Any(roomWall => roomWall.y == wall.y + 1) || m_RoomCells.Any(roomCell => roomCell.y == wall.y + 1)) 
                && (m_RoomWalls.Any(roomWall => roomWall.y == wall.y - 1) || m_RoomCells.Any(roomCell => roomCell.y == wall.y - 1)))
            {
                return true;
            }

            return false;
        }

    }
}
