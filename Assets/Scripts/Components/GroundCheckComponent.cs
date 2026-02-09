using UnityEditor.UIElements;
using UnityEngine;

[RequireComponent(typeof(Collider2D))]
public class GroundCheckComponent : MonoBehaviour
{
    private bool m_IsGrounded = true;
    private Collider2D m_Collider;
    [SerializeField] string m_GroundTag;
    
    void Start()
    {
        m_Collider = GetComponent<Collider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public bool IsGrounded()
    {
        return m_IsGrounded;
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag(m_GroundTag))
        {
            m_IsGrounded = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(m_GroundTag))
        {
            m_IsGrounded = false;
        }
    }
}
