using System.Collections;
using System.Collections.Generic;
using DefaultNamespace;
using UnityEngine;

public class SpriteSetter : MonoBehaviour
{
    MapObjectData m_ObjectData => GetComponent<MapObjectData>();

    List<SpriteRenderer> m_Renderers = new List<SpriteRenderer>();

    void Awake()
    {
        if (m_ObjectData)
        {
            if (m_ObjectData.objectType == MapObjectType.Pathway)
            {
                m_Renderers.AddRange(GetComponentsInChildren<SpriteRenderer>());

            }
            else if (m_ObjectData.objectType == MapObjectType.Collider)
            {
                m_Renderers.Add(GetComponent<SpriteRenderer>());
            }
        }
    }

    public void SetSprites(Sprite sprite)
    {
        if (!m_ObjectData)
        {
            return;
        }

        foreach (var renderer in m_Renderers)
        {
            renderer.sprite = sprite;
        }
    }
}
