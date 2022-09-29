
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathFinding.Scripts.UIManagers
{
    public class MapGridDataManager : MonoBehaviour
    {
        [SerializeField]
        ObjectMap m_ColliderObjectContainer;
        
        [SerializeField]
        GameObject m_BoundingBoxContainer;

        [SerializeField]
        Tilemap m_PathTileMap;

        [SerializeField] 
        TileScriptableObject m_TileStructuresContainer;

        [SerializeField]
        int m_MapWidth;
        
        [SerializeField]
        int m_MapHeight;

        public int mapWidth => m_MapWidth;
        public int mapHeight => m_MapHeight;
        
        public ObjectMap colliderObjectContainer => m_ColliderObjectContainer;
        public GameObject boundingBoxContainer => m_BoundingBoxContainer;
        public Tilemap pathTileMap => m_PathTileMap;
        public TileScriptableObject tileStructuresContainer => m_TileStructuresContainer;
    }
}
