using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ButtonSound : MonoBehaviour
{
    public int buttonID;
    public AudioSource audioSource;
    public AudioClip clip;

    private static int[] correctOrder = { 4, 6, 11 };
    private static int currentIndex = 0;
    private static bool puzzleSolved = false;
    private static bool retryShown = false;

    private GameObject retryButton;

    private void Awake()
    {
        retryButton = GameObject.Find("Retry"); 
        if (retryButton != null)
            retryButton.SetActive(false); 
    }

    public void Press()
    {
        if (audioSource != null && clip != null)
            audioSource.PlayOneShot(clip);

        if (puzzleSolved) return;

        if (buttonID == correctOrder[currentIndex])
        {
            currentIndex++;

            if (currentIndex >= correctOrder.Length)
            {
                puzzleSolved = true;
                if (retryButton != null)
                    retryButton.SetActive(false); 
                Debug.Log("Puzzle Solved!");
            }
        }
        else
        {
            currentIndex = 0;

            if (!retryShown && retryButton != null)
            {
                retryButton.SetActive(true); 
                retryShown = true;
                Debug.Log("Retry button activated!");
            }

            Debug.Log("Wrong! Try Again!");
        }

        Debug.Log("Pressed: " + buttonID);
    }

    public void Retry()
    {
        currentIndex = 0;
        puzzleSolved = false;
        retryShown = false;

        if (retryButton != null)
            retryButton.SetActive(false); 

        Debug.Log("Puzzle Reset");
    }
}
