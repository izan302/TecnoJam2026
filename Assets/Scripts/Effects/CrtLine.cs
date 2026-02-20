using UnityEngine;

public class CrtLine : MonoBehaviour
{
    private float m_Speed;
    private Transform m_DestroyPoint;

    public void Initialize(float _Speed, Transform _DestroyPoint)
    {
        m_Speed = _Speed;
        m_DestroyPoint = _DestroyPoint;

        RectTransform rt = GetComponent<RectTransform>();
        
        rt.localScale = Vector3.one;
        rt.localRotation = Quaternion.identity;
        rt.localPosition = new Vector3(rt.localPosition.x, rt.localPosition.y, 0f);
    }

    void Update()
    {
        transform.localPosition -= new Vector3(0, m_Speed * Time.deltaTime, 0);

        if (m_DestroyPoint != null && transform.localPosition.y <= m_DestroyPoint.localPosition.y)
        {
            Destroy(gameObject);
        }
    }
}