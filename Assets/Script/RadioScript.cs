using System.Collections.Generic;
using UnityEngine;

public class RadioScript : MonoBehaviour
{
    public List<AudioClip> tracks;
    public AudioClip currentTrack;

    public void SwitchTrack()
    {
        if (tracks.Count == 0)
            return;

        int index = tracks.IndexOf(currentTrack);

        if (index == -1)
        {
            currentTrack = tracks[0];
            MusicManager.Instance.PlayRadioTrack(currentTrack);
            return;
        }

        if (index < tracks.Count - 1)
        {
            currentTrack = tracks[index + 1];
            MusicManager.Instance.PlayRadioTrack(currentTrack);
        }
        else
        {
            currentTrack = null;
            MusicManager.Instance.StopRadio();
        }
    }
}