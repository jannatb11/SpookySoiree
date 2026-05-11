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

    public float volumeMultiplier;

    void Awake()
    {
        volumeMultiplier = 1f;

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
        // Start ambience immediately
        ambienceSource.loop = true;
        ambienceSource.Play();

        // Music prepared but NOT playing yet
        musicSource.loop = true;
        musicSource.volume = 0f;
    }

    public void StartMusic()
    {
        musicStarted = true;
        currentStep = 3;

        // Stop ambience
        if (ambienceSource.isPlaying)
            ambienceSource.Stop();

        ApplyVolume();
    }

    public void PlayNewTrack(AudioClip clip)
    {
        if (clip == null)
            return;

        if (!musicStarted)
            StartMusic();

        // Prevent restarting same track
        if (musicSource.clip == clip && musicSource.isPlaying)
            return;

        musicSource.clip = clip;
        musicSource.Play();

        ApplyVolume();
    }

    public void ReturnToAmbience()
    {
        musicStarted = false;

        musicSource.Stop();
        musicSource.clip = null;

        if (!ambienceSource.isPlaying)
            ambienceSource.Play();
    }

    public void ApplyVolume()
    {
        float volume = volumeSteps[currentStep];

        musicSource.volume = volume * volumeMultiplier;

        // If very far away
        if (volume == 0f)
        {
            if (!ambienceSource.isPlaying)
                ambienceSource.Play();
        }
        else
        {
            if (ambienceSource.isPlaying)
                ambienceSource.Stop();
        }
    }

    public void ChangeVolume()
    {
        if (ambienceSource.isPlaying)
        {
            ambienceSource.volume =
                volumeSteps[currentStep] * volumeMultiplier;
        }
        else
        {
            musicSource.volume =
                volumeSteps[currentStep] * volumeMultiplier;
        }
    }

    public void SetDistanceLevel(int level)
    {
        if (!musicStarted)
            return;

        currentStep = Mathf.Clamp(level, 0, volumeSteps.Length - 1);

        ApplyVolume();
    }
}