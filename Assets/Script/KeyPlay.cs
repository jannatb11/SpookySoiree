using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Collections;
using UnityEngine;

public class ButtonSound : MonoBehaviour
{
    public AudioSource audioSource; 
    public AudioClip clip;           

    public void Play()
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);
    }
}
