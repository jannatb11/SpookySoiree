using System.Collections;
using UnityEngine;

public class EndDemoTransition : MonoBehaviour
{
    [Header("UI")]
    public GameObject endDemoPanel;

    public CanvasGroup fadeScreen;
    public CanvasGroup endDemoText;

    [Header("Hide When Fade Starts")]
    public GameObject[] objectsToHide;

    [Header("Hide CanvasGroups When Fade Starts")]
    public CanvasGroup[] canvasGroupsToHide;

    [Header("Timing")]
    public float fadeDuration = 1f;
    public float textFadeDuration = 1f;

    [Header("Options")]
    public bool freezeTime = true;

    public void StartEndDemo()
    {
        Debug.Log("EndDemoTransition.StartEndDemo() CALLED");

        StartCoroutine(EndDemoRoutine());
    }

    IEnumerator EndDemoRoutine()
    {
        // Enable panel
        if (endDemoPanel != null)
            endDemoPanel.SetActive(true);

        // Hide objects
        foreach (GameObject obj in objectsToHide)
        {
            if (obj != null)
                obj.SetActive(false);
        }

        // Hide canvas groups
        foreach (CanvasGroup cg in canvasGroupsToHide)
        {
            if (cg != null)
            {
                cg.alpha = 0f;
                cg.interactable = false;
                cg.blocksRaycasts = false;
            }
        }

        // Freeze game if desired
        if (freezeTime)
            Time.timeScale = 0f;

        // Reset alphas
        if (fadeScreen != null)
            fadeScreen.alpha = 0f;

        if (endDemoText != null)
            endDemoText.alpha = 0f;

        // Fade screen to black
        if (fadeScreen != null)
        {
            yield return StartCoroutine(
                FadeCanvas(fadeScreen, 0f, 1f, fadeDuration)
            );
        }

        // Small pause
        yield return new WaitForSecondsRealtime(0.3f);

        // Fade text in
        if (endDemoText != null)
        {
            yield return StartCoroutine(
                FadeCanvas(endDemoText, 0f, 1f, textFadeDuration)
            );
        }
    }

    IEnumerator FadeCanvas(
        CanvasGroup cg,
        float start,
        float end,
        float duration)
    {
        float t = 0f;

        cg.alpha = start;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;

            cg.alpha = Mathf.Lerp(
                start,
                end,
                t / duration
            );

            yield return null;
        }

        cg.alpha = end;
    }
}