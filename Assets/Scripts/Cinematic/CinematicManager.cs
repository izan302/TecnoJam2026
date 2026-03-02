using UnityEngine;
using UnityEngine.Video;
public class CinematicManager : MonoBehaviour
{
    private VideoPlayer videoPlayer;

    void Awake()
    {
        videoPlayer = GetComponent<VideoPlayer>();

        videoPlayer.loopPointReached += OnVideoEnded;

        videoPlayer.errorReceived += OnErrorDetected;
    }

    void OnVideoEnded(VideoPlayer vp)
    {
        GabeNewell.Instance.GoToDesktop();
    }

    void OnErrorDetected(VideoPlayer vp, string message)
    {
        Debug.LogError("Eror:: " + message);
        OnVideoEnded(vp);
    }
}
