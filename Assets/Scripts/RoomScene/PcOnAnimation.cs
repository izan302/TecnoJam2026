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

    void Start()
    {
        m_StartupVideo.loopPointReached += OnVideoFinished;
    }

    public void OnClick()
    {
        SceneManager.LoadScene("JanScene", LoadSceneMode.Additive);
        m_PcOn.SetActive(true);
        m_PcStartupScreen.SetActive(true);
        m_PcOff.SetActive(false);
        m_PcDesktop.SetActive(false);

        m_StartupVideo.Play();
    }

    void OnVideoFinished(VideoPlayer vp)
    {
        StartCoroutine(EnterOnPC());
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