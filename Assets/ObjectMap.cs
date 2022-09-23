using System.Collections.Generic;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DefaultNamespace
{
    public class ObjectMap
    {
        Dictionary<Vector3Int, GameObject> m_Objects = new Dictionary<Vector3Int, GameObject>();

        public void SetGameObject(Vector3Int pos, GameObject obj)
        {
            m_Objects[pos] = obj;
        }

        public GameObject GetGameObject(Vector3Int pos)
        {
            if (!m_Objects.ContainsKey(pos))
            {
                return null;
            }
            
            return m_Objects[pos];
        }

        public void RemoveGameObject(Vector3Int pos)
        {
            if (!m_Objects.ContainsKey(pos))
            {
                return;
            }
            
            m_Objects.Remove(pos);
        }
    }
}
