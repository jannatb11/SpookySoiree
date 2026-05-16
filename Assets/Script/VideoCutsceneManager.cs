using UnityEngine;
using UnityEngine.Video;
using System.Collections;

public class VideoCutsceneManager : MonoBehaviour
{
    public static VideoCutsceneManager Instance;

    public GameObject cutscenePanel;
    public VideoPlayer videoPlayer;

    void Awake()
    {
        Instance = this;
    }

    public IEnumerator PlayVideoCutscene(VideoClip clip)
    {
        cutscenePanel.SetActive(true);

        videoPlayer.clip = clip;
        videoPlayer.Play();

        while (videoPlayer.isPlaying)
        {
            yield return null;
        }

        cutscenePanel.SetActive(false);
    }
}