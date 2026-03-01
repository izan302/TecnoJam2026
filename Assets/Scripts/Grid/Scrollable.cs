using System;
using UnityEngine;
using UnityEngine.UI;

public class Scrollable : MonoBehaviour
{
    [Header("Referencias")]
    public Transform m_TopAnchor;    
    public Transform m_BottomAnchor;
    [SerializeField] Scrollbar m_Scrollbar;

    [Header("Configuración")]
    [SerializeField] private float m_ScrollSpeed = 10f;
    [SerializeField] private float m_SmoothSpeed = 12f;
    [SerializeField] private float m_ButtonStep = 2f;

    [Header("Auto Scroll")]
    [SerializeField] private float m_AutoScrollSpeed = 0.5f;
    private bool m_IsAutoScrolling = false;
    private bool m_WasTutorialPlaying = true;

    private float m_TargetY;
    private float m_ContentHeight;
    private bool m_IsReady = false;

    public bool AtBottom => Mathf.Abs(m_TargetY - m_BottomAnchor.position.y) < 0.01f;
    public bool AtTop => Mathf.Abs(m_TargetY - GetLimitTop()) < 0.01f;

    public void SetupLimits(float _Height)
    {
        m_ContentHeight = _Height;
        m_TargetY = GetLimitTop();
        transform.position = new Vector3(transform.position.x, m_TargetY, transform.position.z);
        m_IsReady = true;
        
        if (GabeNewell.Instance != null && !GabeNewell.Instance.m_IsTutorialPlaying)
        {
            m_IsAutoScrolling = true;
        }
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
        m_IsAutoScrolling = false;
        m_TargetY += m_ButtonStep;
        ApplyClamp();
        UpdateScrollbar();
    }

    public void ScrollUp()
    {
        m_IsAutoScrolling = false;
        m_TargetY -= m_ButtonStep;
        ApplyClamp();
        UpdateScrollbar();
    }

    public void SetScrollPosition(Single s)
    {
        if (!m_IsReady) return;
        m_IsAutoScrolling = false;

        float l_LimitBottom = m_BottomAnchor.position.y;
        float l_LimitTop = GetLimitTop();

        m_TargetY = Mathf.Lerp(l_LimitBottom, l_LimitTop, s);
        ApplyClamp();
    }

    private void UpdateScrollbar()
    {
        if (m_Scrollbar == null) return;

        float l_LimitBottom = m_BottomAnchor.position.y;
        float l_LimitTop = GetLimitTop();
        if (Mathf.Approximately(l_LimitBottom, l_LimitTop))
        {
            m_Scrollbar.value = 1f;
            return;
        }
        float l_Normalized = Mathf.InverseLerp(l_LimitBottom, l_LimitTop, m_TargetY);
        m_Scrollbar.SetValueWithoutNotify(l_Normalized);
    }

    void Update()
    {
        if (!m_IsReady) return;

        bool l_TutorialActive = GabeNewell.Instance != null && GabeNewell.Instance.m_IsTutorialPlaying;

        if (m_WasTutorialPlaying && !l_TutorialActive)
        {
            m_IsAutoScrolling = true;
        }
        m_WasTutorialPlaying = l_TutorialActive;

        float l_ScrollInput = Input.GetAxis("Mouse ScrollWheel");
        
        if (l_ScrollInput != 0)
        {
            m_IsAutoScrolling = false;
            m_TargetY -= l_ScrollInput * m_ScrollSpeed;
            ApplyClamp();
            UpdateScrollbar();
        }
        else if (m_IsAutoScrolling && !l_TutorialActive)
        {
            m_TargetY = Mathf.MoveTowards(m_TargetY, m_BottomAnchor.position.y, m_AutoScrollSpeed * Time.deltaTime);
            UpdateScrollbar();
            if (Mathf.Approximately(m_TargetY, m_BottomAnchor.position.y)) m_IsAutoScrolling = false;
        }

        Vector3 l_NextPos = new Vector3(transform.position.x, m_TargetY, transform.position.z);
        transform.position = Vector3.Lerp(transform.position, l_NextPos, Time.deltaTime * m_SmoothSpeed);
    }
}