using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    public class Wall
    {
        TileBase m_WallType;

        Vector3Int[] m_OccupyingTiles;
        
        public Wall(Vector3Int[] occupyingTiles, TileBase wallType)
        {
            m_OccupyingTiles = occupyingTiles;
            m_WallType = wallType;
        }
    }
}
