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

        var objectData = m_MapSet[position].GetComponent<MapObjectData>();


        var sprites = m_MapSet[position].GetComponent<MapObjectData>().spriteContainer;
        if (sprites == null)
        {
            return false;
        }

        SpriteSetter spriteSetter = m_MapSet[position].GetComponent<SpriteSetter>();
        DoorController doorController = null;

        if (objectData.objectType == MapObjectType.Pathway)
        {
            doorController = m_MapSet[position].GetComponent<DoorController>();
        }

        var corners = new List<Vector3Int>();
        corners.Add(new Vector3Int(position.x + 1, position.y, position.z));
        corners.Add(new Vector3Int(position.x, position.y + 1, position.z));
        corners.Add(new Vector3Int(position.x, position.y - 1, position.z));
        corners.Add(new Vector3Int(position.x - 1, position.y, position.z));

        if (corners.All(corner => m_MapSet.ContainsKey(corner)))
        {
            if (doorController)
            {
                doorController.SetDoorPlacementHorizontal(true);
                return true;
            }
            
            spriteSetter.SetSprites(sprites.crossSprite);
        }
        else if (m_MapSet.ContainsKey(corners[0]) && m_MapSet.ContainsKey(corners[3]))
        {
            if (doorController)
            {
                doorController.SetDoorPlacementHorizontal(false);
                return true;
            }           
            
            if (m_MapSet.ContainsKey(corners[2]))
            {
                spriteSetter.SetSprites(sprites.rightSprite);
            }

            if (m_MapSet.ContainsKey(corners[1]))
            {
                spriteSetter.SetSprites(sprites.leftSprite);
            }
        }
        else if (m_MapSet.ContainsKey(corners[1]) && m_MapSet.ContainsKey(corners[2]))
        {
            if (doorController)
            {
                doorController.SetDoorPlacementHorizontal(true);
                return true;
            }    
            
            if (m_MapSet.ContainsKey(corners[3]))
            {
                spriteSetter.SetSprites(sprites.upSprite);
            }
            else if (m_MapSet.ContainsKey(corners[0]))
            {
                spriteSetter.SetSprites(sprites.downSprite);
            }
        }

        else if (m_MapSet.ContainsKey(corners[2]))
        {
            if (doorController)
            {
                doorController.SetDoorPlacementHorizontal(true);
                return true;
            }
            
            if (m_MapSet.ContainsKey(corners[0]))
            {
                spriteSetter.SetSprites(sprites.downRightSprite);
            }
            else if (m_MapSet.ContainsKey(corners[3]))
            {
                spriteSetter.SetSprites(sprites.upRightSprite);
            }
        }

        else if (m_MapSet.ContainsKey(corners[1]))
        {
            if (doorController)
            {
                doorController.SetDoorPlacementHorizontal(true);
                return true;
            }
            if (m_MapSet.ContainsKey(corners[0]))
            {
                spriteSetter.SetSprites(sprites.downLeftSprite);
            }
            else if (m_MapSet.ContainsKey(corners[3]))
            {
                spriteSetter.SetSprites(sprites.upLeftSprite);
            }
        }
        else
        {
            if (doorController)
            {
                doorController.SetDoorPlacementHorizontal(true);
                return true;
            }
            spriteSetter.SetSprites(sprites.aloneSprite);
        }

        return true;
    }
}
