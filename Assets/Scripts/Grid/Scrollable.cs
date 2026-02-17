using UnityEngine;

public class Scrollable : MonoBehaviour
{
    [Header("Referencias")]
    public Transform m_TopAnchor;    
    public Transform m_BottomAnchor;

    [Header("Configuración")]
    [SerializeField] private float m_ScrollSpeed = 10f;
    [SerializeField] private float m_SmoothSpeed = 12f;
    [SerializeField] private float m_ButtonStep = 2f;

    private float m_TargetY;
    private float m_ContentHeight;
    private bool m_IsReady = false;

    public bool AtBottom => Mathf.Abs(m_TargetY - m_BottomAnchor.position.y) < 0.01f;
    public bool AtTop => Mathf.Abs(m_TargetY - GetLimitTop()) < 0.01f;

    public void SetupLimits(float _Height)
    {
        m_ContentHeight = _Height;
        m_TargetY = transform.position.y;
        m_IsReady = true;
    }

    private float GetLimitTop()
    {
        float l_WindowHeight = Mathf.Abs(m_TopAnchor.position.y - m_BottomAnchor.position.y);
        float l_LimitBottom = m_BottomAnchor.position.y;

        if (m_ContentHeight > l_WindowHeight)
        {
            return l_LimitBottom - (m_ContentHeight - l_WindowHeight);
        }
        return l_LimitBottom;
    }

    private void ApplyClamp()
    {
        float l_LimitBottom = m_BottomAnchor.position.y;
        float l_LimitTop = GetLimitTop();
        float l_Min = Mathf.Min(l_LimitBottom, l_LimitTop);
        float l_Max = Mathf.Max(l_LimitBottom, l_LimitTop);
        m_TargetY = Mathf.Clamp(m_TargetY, l_Min, l_Max);
    }

    public void ScrollDown()
    {
        m_TargetY += m_ButtonStep;
        ApplyClamp();
    }

    public void ScrollUp()
    {
        m_TargetY -= m_ButtonStep;
        ApplyClamp();
    }

    void Update()
    {
        if (!m_IsReady) return;

        float l_ScrollInput = Input.GetAxis("Mouse ScrollWheel");
        if (l_ScrollInput != 0)
        {
            m_TargetY -= l_ScrollInput * m_ScrollSpeed;
            ApplyClamp();
        }

        Vector3 l_NextPos = new Vector3(transform.position.x, m_TargetY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, l_NextPos, Time.deltaTime * m_SmoothSpeed);
    }
}