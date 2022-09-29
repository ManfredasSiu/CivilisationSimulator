using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    Vector2 maxSpeed = new Vector2(5, 5);
    
    Inputs m_UserInput;

    Rigidbody2D m_Rigidbody;

    // Start is called before the first frame update
    void Start()
    {
        m_UserInput = new Inputs();
        m_UserInput.Player.Move.Enable();

        m_Rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        var movementAxis = m_UserInput.Player.Move.ReadValue<Vector2>() * maxSpeed;
        var currentVelocity = m_Rigidbody.velocity;
        m_Rigidbody.AddForce((movementAxis-currentVelocity) *5f, ForceMode2D.Force);
    }
}
