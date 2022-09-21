using System.Collections.Generic;
using System.Linq;

namespace DefaultNamespace
{
    public class Room
    {
        int m_RoomId;

        List<PathNode> m_RoomCells;

        public bool isOutside = false;

        public Room()
        {
            m_RoomCells = new List<PathNode>();
        }
        
        public bool ContainsCell(int x, int y)
        {
            return m_RoomCells.Any(cell => cell.x == x && cell.y == y);
        }
        
        public bool ContainsCell(PathNode cell)
        {
            return m_RoomCells.Any(roomCell => roomCell == cell);
        }

        public void AssignCell(PathNode cell)
        {
            m_RoomCells.Add(cell);
        }
    }
}
