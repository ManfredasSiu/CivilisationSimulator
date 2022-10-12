using System.Diagnostics;
using UnityEngine;

namespace DefaultNamespace
{    
    [CreateAssetMenu(fileName = "Object Sprite Container", menuName = "Map Data/Object Sprite Container", order = 1)]
    public class ObjectSpriteContainer : ScriptableObject
    {
        [SerializeField]
        Sprite m_CrossSprite;
    
        [SerializeField]
        Sprite m_AloneSprite;
    
        [SerializeField]
        Sprite m_DownSprite;
    
        [SerializeField]
        Sprite m_DownRightSprite;
        
        [SerializeField]
        Sprite m_DownLeftSprite;
    
        [SerializeField]
        Sprite m_UpSprite;

        [SerializeField]
        Sprite m_UpRightSprite;
        
        [SerializeField]
        Sprite m_UpLeftSprite;

        [SerializeField]
        Sprite m_RightSprite;
        
        [SerializeField]
        Sprite m_LeftSprite;

        public Sprite aloneSprite => m_AloneSprite;

        public Sprite downSprite => m_DownSprite;

        public Sprite downRightSprite => m_DownRightSprite;

        public Sprite upSprite => m_UpSprite;

        public Sprite upRightSprite => m_UpRightSprite;

        public Sprite rightSprite => m_RightSprite;
        
        public Sprite leftSprite => m_LeftSprite;

        public Sprite crossSprite => m_CrossSprite;
        
        public Sprite downLeftSprite => m_DownLeftSprite;

        public Sprite upLeftSprite => m_UpLeftSprite;
    }
}
