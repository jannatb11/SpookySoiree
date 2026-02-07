using System.Collections;
using UnityEngine;

public class KeyPlay : MonoBehaviour
{
    public int buttonID;
    public AudioSource audioSource;
    public AudioClip clip;
    public GameObject pianoinv;

    public static GameObject winUI;
    public static GameObject xUI;

    [Header("Assign UI ONCE (any button)")]
    public GameObject winUIRef;
    public GameObject xUIRef;
    private static int[] correctOrder = { 4, 6, 11, 11, 11, 6, 6, 4, 4};
    private static int currentIndex = 0;
    private static bool puzzleSolved = false;
    private static bool uiInitialized = false;

    private void Awake()
    {
        if (!uiInitialized)
        {
            winUI = winUIRef;
            xUI = xUIRef;

            if (winUI != null)
                winUI.SetActive(false);

            if (xUI != null)
                xUI.SetActive(false);

            uiInitialized = true;


            if (pianoinv != null)
                pianoinv.SetActive(false);
            else
                Debug.LogWarning("No inv ");
        }
    }


    public void Press()
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);

        if (puzzleSolved)
            return;

        if (currentIndex < correctOrder.Length &&
            buttonID == correctOrder[currentIndex])
        {
            currentIndex++;

            if (xUI != null)
                xUI.SetActive(false);

            if (currentIndex >= correctOrder.Length)
            {
                puzzleSolved = true;

                if (winUI != null)
                    winUI.SetActive(true);

                Debug.Log("Puzzle Solved!");
                pianoinv.SetActive(true);

            }
        }
        else
        {
            currentIndex = 0;

            if (xUI != null)
            {
                xUI.SetActive(true);
                StopAllCoroutines();
                StartCoroutine(HideXAfterDelay());
            }

            Debug.Log("Wrong move!");
        }

        Debug.Log("Pressed: " + buttonID);
    }

    private IEnumerator HideXAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (xUI != null)
            xUI.SetActive(false);
    }
}
