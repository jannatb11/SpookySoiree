using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RadioScript : MonoBehaviour
{
    public List<AudioClip> tracks;
    public AudioClip currentTrack;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void SwitchTrack(){
        int index = tracks.IndexOf(currentTrack);
        if(index < tracks.Count - 1){
            currentTrack = tracks[index + 1];
        } else{
            currentTrack = tracks[0];
        }
        GameObject.Find("MusicManager").GetComponent<AudioSource>().clip = currentTrack;
        GameObject.Find("MusicManager").GetComponent<MusicManager>().PlayMusic();
    }
}
