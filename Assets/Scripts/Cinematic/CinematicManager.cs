using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class CinematicManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;
    [Header("Configurar Salto")]
    [SerializeField] private float m_HoldTimeRequired = 2f;
    private float m_HoldTimer = 0f;
    private bool m_IsHolding = false;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.url = Path.Combine(Application.streamingAssetsPath, "cinematicaWEBGL2.mp4");

        videoPlayer.loopPointReached += OnVideoEnded;
        videoPlayer.errorReceived += OnErrorDetected;
        videoPlayer.prepareCompleted += OnVideoPrepared;
    }

    void Start()
    {
        videoPlayer.Prepare();
    }

    void Update()
    {
        HandleSkipInput();
    }

    void HandleSkipInput()
    {
        if (!videoPlayer.isPlaying || m_IsHolding) return;
        if (Input.GetMouseButton(0))
        {
            m_HoldTimer += Time.deltaTime;

            if (m_HoldTimer >= m_HoldTimeRequired)
            {
                m_IsHolding = true;
                OnVideoEnded(videoPlayer);
            }
        }else
        {
            m_HoldTimer = 0f;
        }
    }

    void OnVideoPrepared(VideoPlayer vp)
    {
        vp.Play();
    }

    void OnVideoEnded(VideoPlayer vp)
    {
        if (vp.isPlaying) vp.Stop();
        
        if (GabeNewell.Instance != null)
            GabeNewell.Instance.GoToDesktop();
    }

    void OnErrorDetected(VideoPlayer vp, string message)
    {
        Debug.LogError("Error: " + message);
        OnVideoEnded(vp);
    }
}