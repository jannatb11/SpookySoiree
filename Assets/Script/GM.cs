using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public static GM Instance;

    [SerializeField] private GameObject winUI;
    [SerializeField] private GameObject exitButton; // reference

    private List<Penny> pennies = new List<Penny>();

    private Penny currentPenny;
    private int clickedCount = 0;

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        if (winUI != null)
        {
            winUI.SetActive(false);
        }

        if (exitButton != null) // <-- HIDE AT START
        {
            exitButton.SetActive(false);
        }

        pennies.AddRange(FindObjectsOfType<Penny>());

        foreach (Penny p in pennies)
        {
            p.SetWhite();
        }

        ActivateRandomPenny();
    }

    private void ActivateRandomPenny()
    {
        List<Penny> available = new List<Penny>();

        foreach (Penny p in pennies)
        {
            if (p != currentPenny)
            {
                available.Add(p);
            }
        }

        if (available.Count == 0)
            return;

        currentPenny = available[Random.Range(0, available.Count)];

        currentPenny.SetGreen();
    }

    public void PennyClicked(Penny penny)
    {
        if (penny != currentPenny)
            return;

        clickedCount++;

        penny.SetWhite();

        if (clickedCount >= 15)
        {
            WinGame();
            return;
        }

        ActivateRandomPenny();
    }

    private void WinGame()
    {
        Debug.Log("YOU WIN!");
        GlobalUnlocksScript.completedLockPuzzle = true;
        foreach (Penny p in pennies)
        {
            p.gameObject.SetActive(false);
        }

        if (winUI != null)
        {
            winUI.SetActive(true);
        }

        if (exitButton != null) 
        {
            exitButton.SetActive(true);
        }


    }
}