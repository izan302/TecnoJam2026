using System.Collections;
using UnityEngine;

public class AppScript : MonoBehaviour
{
    [SerializeField] private float m_timeToDoubleClick = 1.5f;
    [SerializeField] private GameObject m_appWindow;
    [SerializeField] private Animation m_appAnimation;
    [SerializeField] private AnimationClip m_appAnimationOpen;
    [SerializeField] private AnimationClip m_appAnimationClose;
    [SerializeField] private bool m_IsDoubleClickToOpen = true;
    private float m_clickCounter;
    private void Update()
    {
        m_clickCounter += Time.deltaTime;
    }
    public void AppClick()
    {
        if(m_clickCounter < m_timeToDoubleClick || !m_IsDoubleClickToOpen)
        {
            m_appWindow.SetActive(true);
            if(m_appAnimationOpen != null)
            {
                m_appAnimation.clip = m_appAnimationOpen;
                m_appAnimation.Play();
            }
        }
        m_clickCounter = 0;
    }
    public void AppClose()
    {
        if(m_appAnimationClose != null)
        {
            m_appAnimation.clip = m_appAnimationClose;
            m_appAnimation.Play();
            StartCoroutine(WaitForAnimation());
        }
        else
        {
            m_appWindow.SetActive(false);
        }
    }

    private IEnumerator WaitForAnimation()
    {

        yield return new WaitForSeconds(m_appAnimation.clip.length);
        m_appWindow.SetActive(false);
    }
}
