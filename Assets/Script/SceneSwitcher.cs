using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class SceneSwitcher : MonoBehaviour
{
    public Image fadeImage;
    public float fadeDuration = 1f;
    public string targetSceneName = "Kitchen";

    private bool triggered;

    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        // Start fully transparent
        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        // When a new scene loads, fade back in
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
        fadeImage.raycastTarget = false;

        // Fade OUT (0 -> 1)
        yield return StartCoroutine(Fade(0f, 1f));

        // Unlock kitchen using NEW system
        GameProgress.kitchenUnlocked = true;

        SceneManager.LoadScene(targetSceneName);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Fade back IN (1 -> 0)
        StartCoroutine(Fade(1f, 0f));

        triggered = false;
    }

    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float t = 0f;
        Color c;
        if(fadeImage != null){
            c = fadeImage.color;
        } else{
            c = new Color(255, 255, 255);
        }

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, endAlpha, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = endAlpha;
        if(fadeImage != null){
            fadeImage.color = c;
        }
    }
}
