using UnityEngine;
using UnityEngine.Video;

public class VideoCutsceneEndHandler : MonoBehaviour
{
    public NPCInteraction npc;
    public VideoPlayer videoPlayer;

    void Start()
    {
        videoPlayer.loopPointReached += OnVideoEnd;
    }

    void OnVideoEnd(VideoPlayer vp)
    {
        if (npc != null)
            npc.OnVideoFinished();
    }
}