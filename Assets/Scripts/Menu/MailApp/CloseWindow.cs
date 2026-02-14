using UnityEngine;

public class CloseWindow : MonoBehaviour
{
    [SerializeField] private GameObject m_WindowToClose;
    public void Close()
    {
        if (m_WindowToClose != null)
        {
            m_WindowToClose.SetActive(false);
        }
    }
}
