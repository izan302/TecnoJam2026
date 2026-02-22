using UnityEngine;
using UnityEngine.EventSystems;

public class InteractiveObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool m_IsInteractable { get; set; } = true;
    private Vector2 hotspot = Vector2.zero;

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_IsInteractable)
        {
            CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.Clickable, hotspot);
        }else {
            CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.UnClickable, hotspot);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.Normal, Vector2.zero);
    }

    void OnDisable()
    {
        CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.Normal, Vector2.zero);
    }
}