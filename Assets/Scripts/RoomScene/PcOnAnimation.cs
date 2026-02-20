using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Video;

public class PcOnAnimation : MonoBehaviour
{
    [SerializeField] private VideoPlayer m_StartupVideo;
    [SerializeField] private GameObject m_PcOn;
    [SerializeField] private GameObject m_PcOff;
    [SerializeField] private GameObject m_PcDesktop;
    [SerializeField] private GameObject m_PcStartupScreen;
    [SerializeField] private GameObject m_Title;

    void Start()
    {
        m_StartupVideo.loopPointReached += OnVideoFinished;
        m_StartupVideo.started += OnVideoStarted;
    }

    public void OnClick()
    {
        SceneManager.LoadScene("JanScene", LoadSceneMode.Additive);
        m_Title.SetActive(false);
        m_PcOn.SetActive(true);
        m_PcStartupScreen.SetActive(true);
        m_StartupVideo.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(EnterOnPC());
    }
    void OnVideoStarted(VideoPlayer vp)
    {
        m_PcOff.SetActive(false);
    }

    IEnumerator EnterOnPC()
    {
        m_PcDesktop.SetActive(true);
        m_PcStartupScreen.SetActive(false);
        yield return new WaitForSeconds(0.5f);
        SceneManager.LoadScene("JanScene");
    } 

    void OnDestroy()
    {
        m_StartupVideo.loopPointReached -= OnVideoFinished;
    }
}