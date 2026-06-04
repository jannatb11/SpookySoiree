using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public CanvasGroup fadeScreen;
    public CanvasGroup actText;

    public float fadeDuration = 1f;
    public float textDuration = 2f;

    public void StartTransition(string sceneName)
    {
        StartCoroutine(Transition(sceneName));
    }

    IEnumerator Transition(string sceneName)
    {
        // Fade to black
        yield return StartCoroutine(FadeCanvas(fadeScreen, 0f, 1f, fadeDuration));

        // Show ACT 2
        yield return StartCoroutine(FadeCanvas(actText, 0f, 1f, 0.5f));

        yield return new WaitForSeconds(textDuration);

        yield return StartCoroutine(FadeCanvas(actText, 1f, 0f, 0.5f));

        // Load new scene
        SceneManager.LoadScene(sceneName);

        // (Optional) fade back in after load
    }

    IEnumerator FadeCanvas(CanvasGroup cg, float start, float end, float duration)
    {
        float time = 0f;
        cg.alpha = start;

        while (time < duration)
        {
            time += Time.deltaTime;
            cg.alpha = Mathf.Lerp(start, end, time / duration);
            yield return null;
        }

        cg.alpha = end;
    }
}