using System.Collections;
using UnityEngine;

public class EndDemoTransition : MonoBehaviour
{
    [Header("UI")]
    public CanvasGroup fadeScreen;     // black full-screen image
    public CanvasGroup endDemoText;    // "END OF DEMO"

    [Header("Timing")]
    public float fadeDuration = 1f;
    public float textFadeDuration = 1f;

    [Header("Options")]
    public bool freezeTime = true;

    private void Awake()
    {
        // SAFETY: make sure nothing blocks clicks in the world
        if (fadeScreen != null)
            fadeScreen.blocksRaycasts = false;

        if (endDemoText != null)
            endDemoText.blocksRaycasts = false;
    }

    public void StartEndDemo()
    {
        Debug.Log("END DEMO STARTED");  
        StartCoroutine(EndDemoRoutine());
    }

    IEnumerator EndDemoRoutine()
    {
        // Freeze gameplay if needed
        if (freezeTime)
            Time.timeScale = 0f;

        // Ensure objects are active
        fadeScreen.gameObject.SetActive(true);
        endDemoText.gameObject.SetActive(true);

        // Reset alpha
        fadeScreen.alpha = 0f;
        endDemoText.alpha = 0f;

        
        yield return StartCoroutine(FadeCanvas(fadeScreen, 0f, 1f, fadeDuration));

        // Small pause (unscaled so it works even when timeScale = 0)
        yield return new WaitForSecondsRealtime(0.3f);

        
        yield return StartCoroutine(FadeCanvas(endDemoText, 0f, 1f, textFadeDuration));

       
    }

    IEnumerator FadeCanvas(CanvasGroup cg, float start, float end, float duration)
    {
        float t = 0f;
        cg.alpha = start;

      
        cg.blocksRaycasts = false;

        while (t < duration)
        {
            t += Time.unscaledDeltaTime;
            cg.alpha = Mathf.Lerp(start, end, t / duration);
            yield return null;
        }

        cg.alpha = end;
    }
}