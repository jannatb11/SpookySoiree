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

        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0, 1, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        //  Unlock Kitchen here
        DialogueGate.gurtUnlockedKitchen = true;

        SceneManager.LoadScene(targetSceneName);
    }


    IEnumerator Fade(float startAlpha, float endAlpha)
    {
        float t = 0f;
        Color c = fadeImage.color;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(startAlpha, endAlpha, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        c.a = endAlpha;
        fadeImage.color = c;
    }
}
