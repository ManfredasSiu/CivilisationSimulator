using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DefaultNamespace
{
    public class ObjectMap : MonoBehaviour
    {
        Dictionary<Vector3Int, GameObject> m_Objects = new Dictionary<Vector3Int, GameObject>();

        public event Action<Vector3Int[], GameObject[]> ObjectMapEdited;

        public void SetObjects(Vector3Int[] posArray, GameObject[] objArray)
        {
            for (int x = 0; x < objArray.Length; x++)
            {
                if (objArray[x] == null && m_Objects.ContainsKey(posArray[x]))
                {
                    Destroy(m_Objects[posArray[x]]);
                    
                    m_Objects.Remove(posArray[x]);
                }
                else
                {
                    m_Objects[posArray[x]] = objArray[x];
                }
            }
            
            ObjectMapEdited?.Invoke(posArray, objArray);
        }
        
        public void SetObject(Vector3Int pos, GameObject obj)
        {
            if (obj == null && m_Objects.ContainsKey(pos))
            {
                Destroy(m_Objects[pos]);
                
                m_Objects.Remove(pos);
                
                ObjectMapEdited?.Invoke(new []{pos}, new []{obj});
                return;
            }
            
            m_Objects[pos] = obj;
            
            ObjectMapEdited?.Invoke(new []{pos}, new []{obj});
        }

        public GameObject GetObject(Vector3Int pos)
        {
            if (!m_Objects.ContainsKey(pos))
            {
                return null;
            }

            return m_Objects[pos];
        }

        public bool HasObject(Vector3Int pos)
        {
            if (m_Objects.ContainsKey(pos))
            {
                return true;
            }

            return false;
        }
    }
}
