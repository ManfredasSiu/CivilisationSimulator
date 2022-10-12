using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DoorController : MonoBehaviour
{
    [SerializeField]
    List<GameObject> m_HorizontalDoorObjects;
    
    [SerializeField]
    List<GameObject> m_VerticalDoorObjects;

    [SerializeField]
    bool m_DoorPlacementHorizontal = true;

    Animator m_DoorAnimator;
    static readonly int k_IsHorizontal = Animator.StringToHash("isHorizontal");
    static readonly int k_IsDoorOpen = Animator.StringToHash("isDoorOpen");

    public List<GameObject> doorObjects
    {
        get
        {
            var concatedList = new List<GameObject>(m_HorizontalDoorObjects);
            return concatedList.Concat(m_VerticalDoorObjects).ToList();
        }
    }

    public void SetDoorPlacementHorizontal(bool isHorizontal)
    {
        m_DoorPlacementHorizontal = isHorizontal;

        foreach (var doorObject in m_HorizontalDoorObjects)
        {
            doorObject.SetActive(isHorizontal);
        }

        foreach (var doorObject in m_VerticalDoorObjects)
        {
            doorObject.SetActive(!isHorizontal);
        }
        
        m_DoorAnimator.SetBool(k_IsHorizontal, m_DoorPlacementHorizontal);
    }

    public void SetDoorSprite(Sprite sprite)
    {
        foreach (var doorObject in m_HorizontalDoorObjects)
        {
            doorObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }

        foreach (var doorObject in m_VerticalDoorObjects)
        {
            doorObject.GetComponent<SpriteRenderer>().sprite = sprite;
        }
    }

    public void OpenDoor()
    {
        m_DoorAnimator.SetBool(k_IsDoorOpen, true);
    }
    
    public void CloseDoor()
    {
        m_DoorAnimator.SetBool(k_IsDoorOpen, false);
    }

    void Awake()
    {
        m_DoorAnimator = GetComponent<Animator>();
    }
}
