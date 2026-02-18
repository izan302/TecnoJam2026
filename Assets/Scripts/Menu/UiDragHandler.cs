using UnityEngine;
using UnityEngine.EventSystems; // Requerido para detectar eventos de UI

public class UIDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform m_RectTransform;
    private CanvasGroup m_CanvasGroup;
    private Canvas m_Canvas;

    void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_CanvasGroup = GetComponent<CanvasGroup>();
        
        m_Canvas = GetComponentInParent<Canvas>();

        if (m_CanvasGroup == null) m_CanvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_CanvasGroup.alpha = 0.6f;
        m_CanvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_RectTransform.anchoredPosition += eventData.delta / m_Canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        m_CanvasGroup.alpha = 1f;
        m_CanvasGroup.blocksRaycasts = true;
    }
}