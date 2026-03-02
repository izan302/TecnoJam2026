using FMODUnity;
using UnityEngine;
using UnityEngine.EventSystems;
public class InteractiveObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool m_IsInteractable { get; set; } = true;
    private Vector2 hotspot = Vector2.zero;
    private bool m_check = false;
    private bool m_fail = false;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_IsInteractable)
        {
            m_check = true;
            CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.Clickable, hotspot);
        }else {
            m_fail = true;
            CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.UnClickable, hotspot);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_check = false;
        m_fail = false;
        CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.Normal, Vector2.zero);
    }

    void OnDisable()
    {
        CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.Normal, Vector2.zero);
    }


    
    public void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (m_check)
            {
                AudioManager.instance.PlayClick();
            }
            if (m_fail)
            {
                AudioManager.instance.Error();
            }
        }
    }
    
}