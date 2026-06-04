using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VolumeSliderScript : MonoBehaviour
{
    public Slider bar;// The slider
    public float value;// Value of slider
    // Start is called before the first frame update
    void Start()
    {
        bar = gameObject.GetComponent<Slider>();
        value = GameObject.Find("MusicManager").GetComponent<MusicManager>().volumeMultiplier;
        bar.value = value;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void UpdateValue(){
        value = bar.value;
        GameObject.Find("MusicManager").GetComponent<MusicManager>().volumeMultiplier = value;
        GameObject.Find("MusicManager").GetComponent<MusicManager>().ChangeVolume();
    }
}
