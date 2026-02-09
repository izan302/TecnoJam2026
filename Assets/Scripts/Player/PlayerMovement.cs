using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float m_MoveSpeed = 5f;
    [SerializeField] private float m_JumpForce = 4f;
    private float m_HorizontalInput;
    bool m_IsFacingRight = false;
    GroundCheckComponent m_GroundCheck;
    Rigidbody2D m_RigidBody;
    void Start()
    {
        m_RigidBody = GetComponent<Rigidbody2D>();
        m_GroundCheck = GetComponentInChildren<GroundCheckComponent>();
    }
    void Update()
    {
        m_HorizontalInput = InputManager.Instance.GetHorizontalAxis();
        FlipSprite();

        if (InputManager.Instance.GetJump() && m_GroundCheck.IsGrounded())
        {
            m_RigidBody.linearVelocity = new Vector2(m_RigidBody.linearVelocityX, m_JumpForce);
        }
    }
    void FixedUpdate()
    {
        float l_HorizontalMovement = m_HorizontalInput * m_MoveSpeed;
        m_RigidBody.linearVelocity = new Vector2(l_HorizontalMovement, m_RigidBody.linearVelocityY);
        
    }

    void FlipSprite()
    {
        if (m_IsFacingRight && m_HorizontalInput < 0f || !m_IsFacingRight && m_HorizontalInput > 0f)
        {
            m_IsFacingRight = !m_IsFacingRight;
            Vector3 l_LocalScale = transform.localScale;
            l_LocalScale.x *= -1f;
            transform.localScale = l_LocalScale;
        }
    }
}
