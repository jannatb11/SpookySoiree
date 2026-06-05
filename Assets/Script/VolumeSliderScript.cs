using UnityEngine;
using UnityEngine.UI;

public class VolumeSliderScript : MonoBehaviour
{
    public Slider bar;

    void Start()
    {
        bar = GetComponent<Slider>();

        if (MusicManager.Instance != null)
        {
            bar.value = MusicManager.Instance.volumeMultiplier;
        }
    }

    // Hook this to the slider OnValueChanged event
    public void UpdateValue()
    {
        if (MusicManager.Instance == null)
            return;

        MusicManager.Instance.ChangeVolume(bar.value);
    }
}