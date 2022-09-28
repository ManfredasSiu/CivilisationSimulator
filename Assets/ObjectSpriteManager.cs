using System.Collections.Generic;
using System.Linq;
using DefaultNamespace;
using UnityEngine;

public class ObjectSpriteManager : MonoBehaviour
{
    [SerializeField]
    ObjectMap m_ColliderObjectMap;

    Dictionary<Vector3Int, GameObject> m_MapSet => m_ColliderObjectMap.mapSet;
    
    void Awake()
    {
        m_ColliderObjectMap.ObjectMapEdited += UpdateSprites;
    }

    void UpdateSprites(Vector3Int[] position, GameObject[] gameObjects)
    {
        var alreadyUpdatedPositions = new HashSet<Vector3Int>();

        for (int colliderIndex = 0; colliderIndex < gameObjects.Length; colliderIndex++)
        {
            var currentPos = position[colliderIndex];
            
            if (gameObjects[colliderIndex] != null)
            {
                UpdateSprite(currentPos);
            }

            var currentNeighborPosition = new Vector3Int(currentPos.x + 1, currentPos.y, currentPos.z);
            if (!alreadyUpdatedPositions.Contains(currentNeighborPosition) && UpdateSprite(currentNeighborPosition))
            {
                alreadyUpdatedPositions.Add(currentNeighborPosition);
            }

            currentNeighborPosition = new Vector3Int(currentPos.x + 1, currentPos.y + 1, currentPos.z);
            if (!alreadyUpdatedPositions.Contains(currentNeighborPosition) && UpdateSprite(currentNeighborPosition))
            {
                alreadyUpdatedPositions.Add(currentNeighborPosition);
            }

            currentNeighborPosition = new Vector3Int(currentPos.x, currentPos.y + 1, currentPos.z);
            if (!alreadyUpdatedPositions.Contains(currentNeighborPosition) && UpdateSprite(currentNeighborPosition))
            {
                alreadyUpdatedPositions.Add(currentNeighborPosition);
            }
            
            currentNeighborPosition = new Vector3Int(currentPos.x-1, currentPos.y+1, currentPos.z);
            if (!alreadyUpdatedPositions.Contains(currentNeighborPosition) && UpdateSprite(currentNeighborPosition))
            {
                alreadyUpdatedPositions.Add(currentNeighborPosition);
            }
            
            currentNeighborPosition = new Vector3Int(currentPos.x-1, currentPos.y, currentPos.z);
            if (!alreadyUpdatedPositions.Contains(currentNeighborPosition) && UpdateSprite(currentNeighborPosition))
            {
                alreadyUpdatedPositions.Add(currentNeighborPosition);
            }
            
            currentNeighborPosition = new Vector3Int(currentPos.x-1, currentPos.y-1, currentPos.z);
            if (!alreadyUpdatedPositions.Contains(currentNeighborPosition) && UpdateSprite(currentNeighborPosition))
            {
                alreadyUpdatedPositions.Add(currentNeighborPosition);
            }
            
            currentNeighborPosition = new Vector3Int(currentPos.x, currentPos.y-1, currentPos.z);
            if (!alreadyUpdatedPositions.Contains(currentNeighborPosition) && UpdateSprite(currentNeighborPosition))
            {
                alreadyUpdatedPositions.Add(currentNeighborPosition);
            }
        }
        
    }

    bool UpdateSprite(Vector3Int position)
    {
        if (!m_MapSet.ContainsKey(position))
        {
            return false;
        }
        
        var sprites = m_MapSet[position].GetComponent<MapObjectData>().spriteContainer;
        if (sprites == null)
        {
            return false;
        }
        
        var renderer = m_MapSet[position].GetComponent<SpriteRenderer>();

        var corners = new List<Vector3Int>();
        corners.Add(new Vector3Int(position.x+1, position.y, position.z));
        corners.Add(new Vector3Int(position.x, position.y+1, position.z));
        corners.Add(new Vector3Int(position.x, position.y-1, position.z));
        corners.Add(new Vector3Int(position.x-1, position.y, position.z));
        
        if (corners.All(corner => m_MapSet.ContainsKey(corner)))
        {
            renderer.sprite = sprites.crossSprite;
        }
        else if (m_MapSet.ContainsKey(corners[0]) && m_MapSet.ContainsKey(corners[3]))
        {
            if (m_MapSet.ContainsKey(corners[2]))
            {
                renderer.sprite = sprites.rightSprite;
            }
            if (m_MapSet.ContainsKey(corners[1]))
            {
                renderer.sprite = sprites.leftSprite;
            }
        }
        else if (m_MapSet.ContainsKey(corners[1]) && m_MapSet.ContainsKey(corners[2]))
        {
            if(m_MapSet.ContainsKey(corners[3]))
            {
                renderer.sprite = sprites.upSprite;
            }
            else if (m_MapSet.ContainsKey(corners[0]))
            {
                renderer.sprite = sprites.downSprite;
            }
        }

        else if (m_MapSet.ContainsKey(corners[2]))
        {
            if (m_MapSet.ContainsKey(corners[0]))
            {
                renderer.sprite = sprites.downRightSprite;
            }
            else if (m_MapSet.ContainsKey(corners[3]))
            {
                renderer.sprite = sprites.upRightSprite;
            }
        }
        
        else if (m_MapSet.ContainsKey(corners[1]))
        {
            if (m_MapSet.ContainsKey(corners[0]))
            {
                renderer.sprite = sprites.downLeftSprite;
            }
            else if (m_MapSet.ContainsKey(corners[3]))
            {
                renderer.sprite = sprites.upLeftSprite;
            }
        }
        
        return true;
    }
}
