using System;
using UnityEngine;
using UnityEngine.UI;

public class InGameDisplay : MonoBehaviour
{
    [SerializeField]
    Image m_HealthBar;
    
    Inputs m_Inputs;

    Camera m_MainCamera;
    
    void Start()
    {
        m_Inputs = new Inputs();
        m_Inputs.UI.Enable();
        m_MainCamera = Camera.main;
    }

    void Update()
    {
        var mousePos = m_Inputs.UI.Point.ReadValue<Vector2>();
        
        
        var rayPos = m_MainCamera.ScreenToWorldPoint(mousePos);

        var hit = Physics2D.Raycast(rayPos, Vector2.zero);
        
        if (hit.collider) {
            var objectHit = hit.transform;
        
            Debug.Log("Hit");
            
            if (objectHit.TryGetComponent<MapObjectData>(out var objectData))
            {
                m_HealthBar.gameObject.SetActive(true);
               
                var healthBarTransform = m_HealthBar.transform;
                //healthBarTransform.localScale = new Vector3(objectHit.transform.localScale.x*healthBarTransform.localScale.x, healthBarTransform.localScale.y);
                healthBarTransform.position = new Vector3(objectHit.position.x + objectHit.localScale.x/2, objectHit.position.y + objectHit.localScale.y * 0.8f);
                
                Debug.Log(m_HealthBar.fillAmount);
                m_HealthBar.fillAmount = objectData.GetHealthPercentage();
            }
            else
            {
                m_HealthBar.gameObject.SetActive(false);
            }
        }
        else
        {
            m_HealthBar.gameObject.SetActive(false);
        }
    }
}
