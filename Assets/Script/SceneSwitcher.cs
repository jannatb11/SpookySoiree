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

    public void TriggerSceneSwitch()
    {
        if (triggered) return;
        triggered = true;
        StartCoroutine(FadeAndSwitch());
    }

    IEnumerator FadeAndSwitch()
    {
        fadeImage.gameObject.SetActive(true);
        fadeImage.raycastTarget = false;

        Color c = fadeImage.color;
        c.a = 0f;
        fadeImage.color = c;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;
            c.a = Mathf.Lerp(0f, 1f, t / fadeDuration);
            fadeImage.color = c;
            yield return null;
        }

        SceneManager.LoadScene(targetSceneName);
    }

}
