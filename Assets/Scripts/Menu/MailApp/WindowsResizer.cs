using UnityEngine;
using UnityEngine.EventSystems;

public class WindowsResizer : MonoBehaviour
{
    public enum Torientation
    {
        Top,
        Bottom,
        Left,
        Right,
    }
    public Torientation m_orientation;
    public RectTransform m_rectTransform;
    public EmailAppSize m_emailAppSize;
    private Vector2 m_lastPointPosition;
    public void OnMouseDragStart(BaseEventData data)
    {
        PointerEventData l_pointerEventData = (PointerEventData)data;
        m_lastPointPosition = l_pointerEventData.position;
    }
    public void DragManager(BaseEventData data)
    {
        PointerEventData l_pointerEventData = (PointerEventData)data;
        Vector2 l_positionDifference = l_pointerEventData.position - m_lastPointPosition;
        Debug.Log(m_orientation);
        switch(m_orientation)
        {
            case Torientation.Top:
                l_positionDifference = new Vector2(0, l_positionDifference.y);
                m_rectTransform.offsetMax += l_positionDifference;
                break;
            case Torientation.Bottom:
                l_positionDifference = new Vector2(0, l_positionDifference.y);
                m_rectTransform.offsetMin += l_positionDifference;
                Debug.Log(l_positionDifference);
                break;
            case Torientation.Right:

                l_positionDifference = new Vector2 (l_positionDifference.x, 0);
                /*if((m_rectTransform.offsetMax.x + l_positionDifference.x) - m_rectTransform.offsetMin.x > m_emailAppSize.m_MaxLeft2Right)
                {
                    break;
                }
                else if ((m_rectTransform.offsetMax.x + l_positionDifference.x) - m_rectTransform.offsetMin.x < m_emailAppSize.m_MinLeft2Right)
                {
                    break;
                }*/
                m_rectTransform.offsetMax += l_positionDifference;
                break;
            case Torientation.Left:
                l_positionDifference = new Vector2(l_positionDifference.x, 0);
                /*
                if ((m_rectTransform.offsetMax.x + l_positionDifference.x) - m_rectTransform.offsetMin.x > m_emailAppSize.m_MaxLeft2Right)
                {
                    break;
                }
                else if ((m_rectTransform.offsetMax.x + l_positionDifference.x) - m_rectTransform.offsetMin.x < m_emailAppSize.m_MinLeft2Right)
                {
                    break;
                }*/
                m_rectTransform.offsetMin += l_positionDifference;
                break;

        }
        m_lastPointPosition = l_pointerEventData.position;
    }
    public void OnMouseDragEnd(BaseEventData data)
    {
        PointerEventData l_pointerEventData = (PointerEventData)data;

    }
}
