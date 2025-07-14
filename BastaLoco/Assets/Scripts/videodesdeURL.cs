using UnityEngine;
using UnityEngine.Video;

public class videodesdeURL : MonoBehaviour
{
    public string videoURL = "https://tuusuario.github.io/elnombrerepo/video.mp4";
    private VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer = GetComponent<VideoPlayer>();
        videoPlayer.source = VideoSource.Url;
        videoPlayer.url = videoURL;

        videoPlayer.Prepare();

        videoPlayer.prepareCompleted += (vp) =>
        {
            vp.Play();
        };
    }
}
