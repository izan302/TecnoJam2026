using UnityEngine;
using UnityEngine.UI;

public class Background : MonoBehaviour
{
    [SerializeField] private Animation m_animator;
    [SerializeField] private AnimationClip m_zoomIn;
    [SerializeField] private AnimationClip m_zoomOut;
    [SerializeField] private GameObject m_windowsUi;
    bool m_zoomedIn = false;
    public void ZoomIn()
    {
        m_animator.Play(m_zoomIn.name);
        m_zoomedIn = true;
    }
    public void OpenWindowsUI()
    {
        m_windowsUi.SetActive(true);
    }
    public void ZoomOut()
    {
        m_animator.Play(m_zoomOut.name);
        m_zoomedIn = false;
    }
}
