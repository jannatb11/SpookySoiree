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
    private int currentIndex = 0;
    private bool puzzleSolved = false;

    private void Awake()
    {
        if (winUI != null)
            winUI.SetActive(false);

        if (xUI != null)
            xUI.SetActive(false);

        if (pianoinv != null)
            pianoinv.SetActive(GlobalUnlocksScript.completedPianoPuzzle);
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