using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    public AudioSource musicSource;
    public AudioSource ambienceSource;

    // 0 = far, 3 = close
    private float[] volumeSteps = { 0f, 0.2f, 0.5f, 1f };
    private int currentStep = 3;

    private bool musicStarted = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        // Start with ambience only
        ambienceSource.loop = true;
        ambienceSource.Play();

        musicSource.loop = true;
        musicSource.volume = 0f;
        musicSource.Play(); //  ALWAYS PLAYING (just silent)
    }

    public void StartMusic()
    {
        musicStarted = true;
        currentStep = 3;

        ambienceSource.Stop();
        ApplyVolume();
    }

    public void PlayNewTrack(AudioClip clip)
    {
        if (!musicStarted)
            StartMusic();

        if (musicSource.clip == clip) return;

        musicSource.clip = clip;
        musicSource.Play();
    }

    // Going farther
    

    void ApplyVolume()
    {
        float volume = volumeSteps[currentStep];
        musicSource.volume = volume;

        if (volume == 0f)
        {
            // Only ambience when VERY far
            if (!ambienceSource.isPlaying)
                ambienceSource.Play();
        }
        else
        {
            // Coming back -> stop ambience
            if (ambienceSource.isPlaying)
                ambienceSource.Stop();
        }
    }

    public void SetDistanceLevel(int level)
    {
        if (!musicStarted) return;

        currentStep = Mathf.Clamp(level, 0, volumeSteps.Length - 1);
        ApplyVolume();
    }
}