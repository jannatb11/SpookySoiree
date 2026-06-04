using System.Collections;
using UnityEngine;

public class KeyPlay : MonoBehaviour
{
    public int buttonID;
    public AudioSource audioSource;
    public AudioClip clip;
    public GameObject pianoinv;

    [Header("UI References")]
    public GameObject winUI;
    public GameObject xUI;

    private int[] correctOrder = { 4, 6, 11, 11, 11, 6, 6, 4, 4 };

    private static int currentIndex = 0;
    private static bool puzzleSolved = false;

    private void Awake()
    {
        if (winUI != null)
            winUI.SetActive(false);

        if (xUI != null)
            xUI.SetActive(false);

        if (pianoinv != null)
            pianoinv.SetActive(GlobalUnlocksScript.completedPianoPuzzle);
    }

    private void OnEnable()
    {
        if (!GlobalUnlocksScript.completedPianoPuzzle)
        {
            currentIndex = 0;
            puzzleSolved = false;
        }
    }

    public void Press()
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);

        if (puzzleSolved || GlobalUnlocksScript.completedPianoPuzzle)
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

                if (pianoinv != null)
                    pianoinv.SetActive(true);

                GlobalUnlocksScript.completedPianoPuzzle = true;

                Debug.Log("Puzzle Solved!");
                winUI.SetActive(true);
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

        Debug.Log("Pressed: " + buttonID + " | Index: " + currentIndex);
    }

    private IEnumerator HideXAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (xUI != null)
            xUI.SetActive(false);
    }
}