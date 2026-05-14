using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneSwitcher : MonoBehaviour
{
    [Header("Fade Settings")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Scene")]
    public string targetSceneName = "Kitchen";

    // =========================
    // NEW: AUTO DIALOGUE SYSTEM
    // =========================
    [Header("Auto Dialogue On Scene Enter")]
    public string autoStartNPCID;

    private bool triggered;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Start fully transparent
        if (fadeImage != null)
        {
            Color c = fadeImage.color;
            c.a = 0f;
            fadeImage.color = c;
        }

        // Listen for scene load
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    public void TriggerSceneSwitch()
    {
        if (triggered) return;
        triggered = true;

        StartCoroutine(FadeAndSwitch());
    }

    IEnumerator FadeAndSwitch()
    {
        if (fadeImage != null)
            fadeImage.raycastTarget = false;

        // =========================
        // FADE OUT (0 -> 1)
        // =========================
        yield return StartCoroutine(Fade(0f, 1f));

        // =========================
        // GAME STATE UPDATE
        // =========================
        GameProgress.kitchenUnlocked = true;

        // =========================
        // NEW: SET AUTO DIALOGUE NPC
        // =========================
        if (!string.IsNullOrEmpty(autoStartNPCID))
        {
            GameState.pendingSelfDialogueID = autoStartNPCID;
        }

        // =========================
        // LOAD SCENE
        // =========================
        SceneManager.LoadScene(targetSceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // =========================
        // FADE BACK IN (1 -> 0)
        // =========================
        StartCoroutine(Fade(1f, 0f));

        triggered = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float t = 0f;
        Color c;

        if (fadeImage != null)
            c = fadeImage.color;
        else
            c = new Color(1f, 1f, 1f);

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, endAlpha, t / fadeDuration);

            if (fadeImage != null)
                fadeImage.color = c;

            yield return null;
        }

        c.a = endAlpha;

        if (fadeImage != null)
            fadeImage.color = c;
    }
}
