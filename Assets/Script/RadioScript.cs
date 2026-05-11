using System.Collections.Generic;
using UnityEngine;

public class RadioScript : MonoBehaviour
{
    public List<AudioClip> tracks;
    public AudioClip currentTrack;

    public void SwitchTrack()
    {
        // If no tracks exist
        if (tracks.Count == 0)
            return;

        int index = tracks.IndexOf(currentTrack);

        // If nothing selected yet, start first track
        if (index == -1)
        {
            currentTrack = tracks[0];
            MusicManager.Instance.PlayNewTrack(currentTrack);
            return;
        }

        // Go to next track
        if (index < tracks.Count - 1)
        {
            currentTrack = tracks[index + 1];
            MusicManager.Instance.PlayNewTrack(currentTrack);
        }
        else
        {
            // End of playlist -> return to ambience
            currentTrack = null;
            MusicManager.Instance.ReturnToAmbience();
        }
    }
}