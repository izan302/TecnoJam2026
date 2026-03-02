using UnityEngine;
using UnityEngine.EventSystems;
using FMODUnity;

public class InteractiveObject : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public bool m_IsInteractable { get; set; } = true;
    private Vector2 hotspot = Vector2.zero;
    private bool m_check = false;
    public EventReference m_click = new EventReference();



    public void Start()
    {

    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (m_IsInteractable)
        {
            m_check = true;
            CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.Clickable, hotspot);
        }else {
            CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.UnClickable, hotspot);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        m_check = false;
        CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.Normal, Vector2.zero);
    }

    void OnDisable()
    {
        CursorManager.Instance.SetInteractorCursor(CursorManager.CursorImage.Normal, Vector2.zero);
    }


    public void Update()
    {
        if (Input.GetMouseButton (0))
        {
            if (m_check)
            {
                RuntimeManager.PlayOneShot(m_click, transform.position);

            }
        }
    }

}