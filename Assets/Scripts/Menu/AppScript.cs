using UnityEngine;

public class AppScript : MonoBehaviour
{
    [SerializeField] private float m_timeToDoubleClick = 1.5f;
    [SerializeField] private GameObject m_appWindow;
    private float m_clickCounter;
    private void Update()
    {
        m_clickCounter += Time.deltaTime;
    }
    public void AppClick()
    {
        if(m_clickCounter < m_timeToDoubleClick)
        {
            m_appWindow.SetActive(true);
        }
        m_clickCounter = 0;
    }
}
