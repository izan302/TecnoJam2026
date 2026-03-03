using UnityEngine;
using UnityEngine.Video;
using System.IO;

public class CinematicManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;

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

    void OnVideoPrepared(VideoPlayer vp)
    {
        vp.Play();
    }

    void OnVideoEnded(VideoPlayer vp)
    {
        if (GabeNewell.Instance != null)
            GabeNewell.Instance.GoToDesktop();
    }

    void OnErrorDetected(VideoPlayer vp, string message)
    {
        Debug.LogError("Error: " + message);
        OnVideoEnded(vp);
    }
}