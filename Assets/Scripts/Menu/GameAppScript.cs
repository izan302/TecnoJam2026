using UnityEngine;

public class GameAppScript : MonoBehaviour
{
    [SerializeField] private string m_appName;
    [SerializeField] private float m_timeToDoubleClick = 1.5f;
    private float m_clickCounter;

    [Header("Animations")]
    [SerializeField] private Animation m_animation;
    [SerializeField] private AnimationClip m_animationClip;

    private void Update()
    {
        m_clickCounter += Time.deltaTime;
        if (GabeNewell.Instance.m_MailsAreRead) {
            GetComponent<InteractiveObject>().m_IsInteractable = true;
        } else {
            GetComponent<InteractiveObject>().m_IsInteractable = false;
        }
    }
    public void AppClick()
    {
        if (m_clickCounter < m_timeToDoubleClick && GabeNewell.Instance.m_MailsAreRead)
        {
            GabeNewell.Instance.GoToGameplay();
        } else if(GabeNewell.Instance.m_MailsAreRead == false)
        {
            m_animation.clip = m_animationClip;
            m_animation.Play();
        }
            m_clickCounter = 0;
    }
}
