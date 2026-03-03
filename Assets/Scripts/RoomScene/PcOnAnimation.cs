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
    private bool m_IsClicked = false;

    void Start()
    {
        m_StartupVideo.source = VideoSource.Url;
        m_StartupVideo.url = Path.Combine(Application.streamingAssetsPath, m_VideoFileName);

        m_StartupVideo.playOnAwake = false;
        m_StartupVideo.loopPointReached += OnVideoFinished;
    }

    public void OnClick()
    {
        if (m_IsClicked) return;
        m_IsClicked = true;
        GabeNewell.Instance.LoadDesktop();

        m_Title.SetActive(false);
        m_PcOn.SetActive(true);
        m_PcStartupScreen.SetActive(true);

        m_StartupVideo.Play();

        StartCoroutine(WaitForVideoToStart());

        Invoke(nameof(SafetyNet), 5f);
    }

    IEnumerator WaitForVideoToStart()
    {
        yield return new WaitUntil(() => m_StartupVideo.isPlaying);

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

        GabeNewell.Instance.GoToCinematic();
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
        }
    }
}