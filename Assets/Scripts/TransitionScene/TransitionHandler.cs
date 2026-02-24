using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TransitionHandler : MonoBehaviour
{
    [SerializeField] private TypewriterEffect m_TypewriterEffect;
    [SerializeField] private string[] m_KeysToPlay;
    private int m_CurrentIndex = 0;
    void Start()
    {
        if (m_KeysToPlay.Length > 0)
        {
            m_TypewriterEffect.PlayText(m_KeysToPlay[m_CurrentIndex]);
        }
        m_TypewriterEffect.OnTextFinished += OnTextFinished;
    }

    void OnTextFinished()
    {
        m_CurrentIndex++;
        if (m_CurrentIndex < m_KeysToPlay.Length)
        {
            m_TypewriterEffect.PlayText(m_KeysToPlay[m_CurrentIndex]);
        }else {
            StartCoroutine(WaitAndStartNextScene(1f));
        }
    }

    void OnDestroy()
    {
        m_TypewriterEffect.OnTextFinished -= OnTextFinished;
    }
    IEnumerator WaitForNextText()
    {
        yield return new WaitForSeconds(1f);
        m_TypewriterEffect.PlayText(m_KeysToPlay[m_CurrentIndex]);
    }
    IEnumerator WaitAndStartNextScene(float _waitTime)
    {
        yield return new WaitForSeconds(_waitTime);
        GabeNewell.Instance.GoToDesktop();
    }
}
