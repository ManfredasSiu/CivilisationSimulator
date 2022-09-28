using System;
using DefaultNamespace;
using UnityEngine;

public class MapObjectData : MonoBehaviour
{
    [SerializeField]
    ObjectSpriteContainer m_ObjectSpriteContainer;
    
    [SerializeField]
    float m_Health = 200;

    [SerializeField]
    MapObjectType m_ObjectType;
    
    [SerializeField]
    float m_Slowness = 0;

    float m_CurrentHealth;
    
    public float health => m_Health;

    public MapObjectType objectType => m_ObjectType;
    
    public ObjectSpriteContainer spriteContainer => m_ObjectSpriteContainer;
    
    public float pathSlowness => m_Slowness;

    void Start()
    {
        m_CurrentHealth = m_Health;
    }

    public void UpdateHealth(float delta)
    {
        m_CurrentHealth += delta;
    }

    public float GetHealthPercentage()
    {
        return m_CurrentHealth / m_Health;
    }
}

[Flags]
public enum MapObjectType
{
    Collider = 0,
    Pathway = 1,
    Furniture = 2,
    Collectible = 4
}
