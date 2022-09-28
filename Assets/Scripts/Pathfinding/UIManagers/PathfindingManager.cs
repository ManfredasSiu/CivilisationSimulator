using System;
using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace PathFinding.Scripts.UIManagers
{
    public class PathfindingManager : MonoBehaviour
    {
        static Pathfinding m_Pathfinding;

        static GridLayout m_TilemapGrid; 

        [SerializeField] 
        Vector3 m_OriginPosition = Vector3.zero;

        [SerializeField]
        MapGridDataManager m_MapData;

        [SerializeField] 
        float m_CellSize;
        
        public static event Action OnPathfindingChanged;

        Tilemap m_PathTilemap => m_MapData.pathTileMap;
        
        ObjectMap m_ColliderTileMap => m_MapData.colliderObjectContainer;

        TileScriptableObject m_TileStructuresContainer => m_MapData.tileStructuresContainer;
        
        int m_Width => m_MapData.mapWidth;

        int m_Height => m_MapData.mapHeight;
        
        public static Pathfinding pathfinding => m_Pathfinding;
        public static GridLayout tilemapGrid => m_TilemapGrid;
        public static event Action<List<PathNode>> OnPathfindingEdited;

        public static GameObject FindBestTarget(Vector3 positionWithDelta, params GameObject[] gameObjects)
        {
            if (gameObjects.Length == 0)
                return null;
            GameObject bestGameObject = gameObjects.First();
            float bestFCost = float.MaxValue;
            foreach (var target in gameObjects)
            {
                var path = m_Pathfinding.FindPath(positionWithDelta, target.transform.position, out var pathFCost);
                if (path == null)
                {
                    continue;
                }
            
                if (pathFCost < bestFCost)
                {
                    bestGameObject = target;
                    bestFCost = pathFCost;
                }
            }

            return bestGameObject;
        }

        void Awake()
        {
            m_TilemapGrid = m_PathTilemap.transform.parent.GetComponentInParent<GridLayout>();
            
            if (m_ColliderTileMap != null)
            {
                m_MapData.colliderObjectContainer.ObjectMapEdited += UpdateWall;
            }

            m_Pathfinding = CreatePathfinding();
        }

        void UpdateWall(Vector3Int[] posArray, GameObject[] objArray)
        {
            var grid = m_Pathfinding.NodeGrid;

            var changedCells = new List<PathNode>();

            for (var x = 0; x < posArray.Length; x++)
            {
                var worldPos = m_TilemapGrid.CellToWorld(posArray[x]);
                
                var gridTile = grid.GetGridObject(worldPos);
             
                if (objArray[x] == null)
                {
                    gridTile.isWalkable = true;
                    changedCells.Add(gridTile);
                }
                else
                {
                    gridTile.isWalkable = false;
                    changedCells.Add(gridTile);
                }
            }

            if (changedCells.Any())
            {
                OnPathfindingEdited?.Invoke(changedCells);
            }
        }

        Pathfinding CreatePathfinding()
        {
            var currentPathFinding = new Pathfinding(m_Width, m_Height, m_CellSize, m_OriginPosition);
            var grid = currentPathFinding.NodeGrid;
            
            var tileStructures = m_TileStructuresContainer.tileDataStructures;

            var slownessStructure = tileStructures.First(tileData => tileData.TileType == TileType.Ground_Slow);
            
            var slownessTiles = slownessStructure.Tiles;

            for (var x = m_OriginPosition.x; x < m_Width+m_OriginPosition.x; x++)
            {
                for (var y = m_OriginPosition.y; y < m_Height+m_OriginPosition.y; y++)
                {
                    var worldPos = new Vector3(x, y, m_OriginPosition.z);
                    var cellPosition = m_TilemapGrid.WorldToCell(worldPos);
                    
                    var pathTile = m_PathTilemap.GetTile(Vector3Int.FloorToInt(cellPosition));
                    var colliderTile = m_ColliderTileMap.GetObject(Vector3Int.FloorToInt(cellPosition));
                    
                    if (colliderTile != null)
                    {
                        grid.GetGridObject(worldPos).isWalkable = false;
                    }

                    else if (slownessTiles.Contains(pathTile))
                    {
                        grid.GetGridObject(worldPos).ApplySlowness(slownessStructure.SlownessIndicator);
                    }
                }
            }
            
            return currentPathFinding;
        }
    }
}
