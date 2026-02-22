using UnityEngine;
using UnityEngine.UI;
public class ScrollButton : MonoBehaviour
{
    [SerializeField] private Scrollable m_ScrollScript;
    [SerializeField] private Button m_UpButton;
    [SerializeField] private Button m_DownButton;

    void Update()
    {
        if (m_ScrollScript == null || m_UpButton == null || m_DownButton == null) return;

        m_UpButton.interactable = !m_ScrollScript.AtTop;
        m_UpButton.GetComponent<InteractiveObject>().m_IsInteractable = !m_ScrollScript.AtTop;
        m_DownButton.interactable = !m_ScrollScript.AtBottom;
        m_DownButton.GetComponent<InteractiveObject>().m_IsInteractable = !m_ScrollScript.AtBottom;
    }
}
