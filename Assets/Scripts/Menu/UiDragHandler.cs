using UnityEngine;
using UnityEngine.EventSystems;

public class UIDragHandler : MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform m_RectTransform;
    private CanvasGroup m_CanvasGroup;
    private Canvas m_Canvas;
    private RectTransform m_ParentRectTransform;
    void Awake()
    {
        m_RectTransform = GetComponent<RectTransform>();
        m_CanvasGroup = GetComponent<CanvasGroup>();
        m_Canvas = GetComponentInParent<Canvas>();
        m_ParentRectTransform = transform.parent as RectTransform;

        if (m_CanvasGroup == null) m_CanvasGroup = gameObject.AddComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        m_CanvasGroup.alpha = 0.6f;
        m_CanvasGroup.blocksRaycasts = false;
        CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.Drag, Vector2.zero);
    }

    public void OnDrag(PointerEventData eventData)
    {
        m_RectTransform.anchoredPosition += eventData.delta / m_Canvas.scaleFactor;
        
        KeepWithinParent();
    }

    private void KeepWithinParent()
    {
        if (m_ParentRectTransform == null) return;

        Vector2 pos = m_RectTransform.anchoredPosition;

        float minX = (m_ParentRectTransform.rect.width - m_RectTransform.rect.width) / -2f;
        float maxX = (m_ParentRectTransform.rect.width - m_RectTransform.rect.width) / 2f;
        float minY = (m_ParentRectTransform.rect.height - m_RectTransform.rect.height) / -2f;
        float maxY = (m_ParentRectTransform.rect.height - m_RectTransform.rect.height) / 2f;

        pos.x = Mathf.Clamp(pos.x, minX, maxX);
        pos.y = Mathf.Clamp(pos.y, minY, maxY);

        m_RectTransform.anchoredPosition = pos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.Normal, Vector2.zero);
        m_CanvasGroup.alpha = 1f;
        m_CanvasGroup.blocksRaycasts = true;
    }
}