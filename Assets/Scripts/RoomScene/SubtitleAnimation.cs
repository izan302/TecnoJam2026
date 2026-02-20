using System.Collections;
using UnityEngine;

public class SubtitleAnimation : MonoBehaviour
{
    [SerializeField] private GameObject m_Subtitle;
    private Coroutine m_SubtitleAnimation;

    void Start()
    {
        if (m_Subtitle != null)
        {
            m_SubtitleAnimation = StartCoroutine(PlaySubtitleAnimation());
        }
    }

    IEnumerator PlaySubtitleAnimation()
    {
        while (true) 
        {
            m_Subtitle.SetActive(true);
            yield return new WaitForSeconds(0.5f);
            
            m_Subtitle.SetActive(false);
            yield return new WaitForSeconds(0.5f);
        }
    }
}