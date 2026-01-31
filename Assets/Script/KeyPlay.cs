using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPlay : MonoBehaviour
{
    public int buttonID;
    public AudioSource audioSource;
    public AudioClip clip;

    public GameObject winUI;
    public GameObject xUI;

    private static int[] correctOrder = { 4, 6, 11 };
    private static int currentIndex = 0;
    private static bool puzzleSolved = false;

    private void Awake()
    {
        if (winUI != null)
            winUI.SetActive(false);

        if (xUI != null)
            xUI.SetActive(false);
    }

    public void Press()
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);

        if (puzzleSolved) return;

        if (currentIndex < correctOrder.Length &&
            buttonID == correctOrder[currentIndex])
        {
            currentIndex++;

            if (xUI != null)
                xUI.SetActive(false);

            if (currentIndex >= correctOrder.Length)
            {
                puzzleSolved = true;
                winUI.SetActive(true);
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
        xUI.SetActive(false);
    }
}
