using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MinesweeperClickHandler : MonoBehaviour, IPointerDownHandler
{
    [Header("Referencias")]
    public Camera m_WorldCamera;
    public RawImage m_DisplayImage;

    [HideInInspector]
    public Minesweeper m_GameLogic;

    public void OnPointerDown(PointerEventData _EventData)
    {
        if (m_WorldCamera == null || m_DisplayImage == null || m_GameLogic == null)
        {
            return;
        }

        RectTransformUtility.ScreenPointToLocalPointInRectangle(
            m_DisplayImage.rectTransform,
            _EventData.position,
            _EventData.pressEventCamera,
            out Vector2 localPoint);

        Rect l_Rect = m_DisplayImage.rectTransform.rect;
        float l_NormalizedX = (localPoint.x - l_Rect.x) / l_Rect.width;
        float l_NormalizedY = (localPoint.y - l_Rect.y) / l_Rect.height;

        Ray ray = m_WorldCamera.ViewportPointToRay(new Vector3(l_NormalizedX, l_NormalizedY, 0));

        Debug.DrawRay(ray.origin, ray.direction * 100, Color.red, 2f);

        RaycastHit2D l_Hit = Physics2D.GetRayIntersection(ray);

        if (l_Hit.collider != null)
        {
            if (_EventData.button == PointerEventData.InputButton.Left)
            {
                m_GameLogic.HandleClickFromWindow(l_Hit.point, false);
            }
            else if (_EventData.button == PointerEventData.InputButton.Right)
            {
                m_GameLogic.HandleClickFromWindow(l_Hit.point, true);
            }
        }
    }
}