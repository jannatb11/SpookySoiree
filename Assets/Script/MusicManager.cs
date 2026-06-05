using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance;

    [Header("Audio Sources")]
    public AudioSource menuSource;
    public AudioSource ambienceSource;
    public AudioSource radioSource;

    [Header("Settings")]
    public float volumeMultiplier = 1f;

    [Header("Scene Settings")]
    public string mainMenuSceneName = "MainMenu";

    // Radio distance steps
    private float[] volumeSteps = { 0f, 0.2f, 0.5f, 1f };
    private int currentStep = 3;

    private bool isMenuScene = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void Start()
    {
        menuSource.Stop();
        ambienceSource.Stop();
        radioSource.Stop();

        Scene currentScene = SceneManager.GetActiveScene();
        OnSceneLoaded(currentScene, LoadSceneMode.Single);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == mainMenuSceneName)
        {
            isMenuScene = true;

            menuSource.volume = volumeMultiplier;
            menuSource.loop = true;
            menuSource.Play();

            ambienceSource.Stop();

            
        }
        else
        {
            isMenuScene = false;

            menuSource.Stop();

            
            if (!radioSource.isPlaying)
            {
                ambienceSource.volume = volumeMultiplier;
                ambienceSource.loop = true;
                ambienceSource.Play();
            }

           

            StartCoroutine(ApplyDistanceNextFrame(scene.name));
        }
    }

   
    public void PlayRadioTrack(AudioClip clip)
    {
        if (clip == null || isMenuScene)
            return;

        radioSource.clip = clip;
        radioSource.loop = true;

        ApplyRadioVolume();
        radioSource.Play();
    }

    public void StopRadio()
    {
        if (isMenuScene)
            return;

        radioSource.Stop();
        radioSource.clip = null;

        if (!ambienceSource.isPlaying)
        {
            ambienceSource.volume = volumeMultiplier;
            ambienceSource.Play();
        }
    }

   
    public void SetDistanceLevel(int level)
    {
        currentStep = Mathf.Clamp(level, 0, volumeSteps.Length - 1);
        ApplyRadioVolume();
    }

    void ApplyRadioVolume()
    {
        if (radioSource.clip == null)
        {
            if (!isMenuScene && !ambienceSource.isPlaying)
                ambienceSource.Play();

            return;
        }

        float volume = volumeSteps[currentStep] * volumeMultiplier;
        radioSource.volume = volume;

        if (volume > 0f)
        {
            if (!radioSource.isPlaying)
                radioSource.Play();

            if (ambienceSource.isPlaying)
                ambienceSource.Stop();
        }
        else
        {
            if (radioSource.isPlaying)
                radioSource.Stop();

            if (!isMenuScene && !ambienceSource.isPlaying)
                ambienceSource.Play();
        }
    }

   
    public void ChangeVolume(float value)
    {
        volumeMultiplier = value;

        menuSource.volume = value;
        ambienceSource.volume = value;

        if (radioSource.clip != null)
            ApplyRadioVolume();
    }

    
    public int GetDistanceForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Dining": return 3;
            case "LRS1": return 2;
            case "Storage": return 2;
            case "LRS3": return 1;
            case "Kitchen": return 1;
            case "LRS2": return 1;
            default: return 0;
        }
    }

    
    IEnumerator ApplyDistanceNextFrame(string sceneName)
    {
        yield return null;
        SetDistanceLevel(GetDistanceForScene(sceneName));
    }
}