using System.Collections.Generic;
using UnityEngine;

public class RadioScript : MonoBehaviour
{
    public List<AudioClip> tracks;
    public AudioClip currentTrack;

    public void SwitchTrack()
    {
        int index = tracks.IndexOf(currentTrack);

        if (index < tracks.Count - 1)
            currentTrack = tracks[index + 1];
        else
            currentTrack = tracks[0];

        MusicManager.Instance.PlayNewTrack(currentTrack);
    }
}