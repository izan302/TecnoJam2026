using System.Collections;
using System.IO;
using UnityEngine;
using UnityEngine.Video;

public class PcOnAnimation : MonoBehaviour
{
    [Header("Components")]
    [SerializeField] private VideoPlayer m_StartupVideo;
    [SerializeField] private string m_VideoFileName = "intro.mp4";

    [Header("UI Elements")]
    [SerializeField] private GameObject m_PcOn;
    [SerializeField] private GameObject m_PcOff;
    [SerializeField] private GameObject m_PcDesktop;
    [SerializeField] private GameObject m_PcStartupScreen;
    [SerializeField] private GameObject m_Title;

    void Start()
    {
        m_StartupVideo.source = VideoSource.Url;
        m_StartupVideo.url = Path.Combine(Application.streamingAssetsPath, m_VideoFileName);
        
        m_StartupVideo.loopPointReached += OnVideoFinished;
        m_StartupVideo.started += OnVideoStarted;
        
        m_StartupVideo.Prepare();
    }

    public void OnClick()
    {
        GabeNewell.Instance.LoadDesktop();
        
        m_Title.SetActive(false);
        m_PcOn.SetActive(true);
        m_PcStartupScreen.SetActive(true);

        if (m_StartupVideo.isPrepared)
        {
            m_StartupVideo.Play();
        }
        else
        {
            m_StartupVideo.prepareCompleted += (vp) => vp.Play();
        }

        Invoke(nameof(SafetyNet), 5.0f);
    }

    void OnVideoStarted(VideoPlayer vp)
    {
        CancelInvoke(nameof(SafetyNet));
        m_PcOff.SetActive(false);
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
        
        GabeNewell.Instance.GoToDesktop();
    }

    void SafetyNet()
    {
        if (!m_StartupVideo.isPlaying)
        {
            StartCoroutine(EnterOnPC());
        }
    }

    void OnDestroy()
    {
        if (m_StartupVideo != null)
        {
            m_StartupVideo.loopPointReached -= OnVideoFinished;
            m_StartupVideo.started -= OnVideoStarted;
        }
    }
}