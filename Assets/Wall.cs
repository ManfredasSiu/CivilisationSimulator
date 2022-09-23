using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    public class Wall
    {
        TileBase m_WallType;

        Vector3Int m_OccupyingTile;

        public Vector3Int tilePos => m_OccupyingTile;
        
        public Wall(Vector3Int occupyingTile, TileBase wallType)
        {
            m_OccupyingTile = occupyingTile;
            m_WallType = wallType;
        }
    }
}
