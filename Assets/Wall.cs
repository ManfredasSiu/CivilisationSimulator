using System.Resources;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace DefaultNamespace
{
    public class Wall
    {
        GameObject m_WallObject;

        Vector3Int m_OccupyingTile;

        public Vector3Int tilePos => m_OccupyingTile;
        
        public Wall(Vector3Int occupyingTile, GameObject wallObject)
        {
            m_OccupyingTile = occupyingTile;
            m_WallObject = wallObject;
        }
    }
}
