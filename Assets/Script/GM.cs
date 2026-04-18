using System.Collections.Generic;
using UnityEngine;

public class GM : MonoBehaviour
{
    public static GM Instance;

    public Penny pennyPrefab;
    public int totalPennies = 15;
    public int respawnTimes = 5;

    private List<Penny> activePennies = new List<Penny>();
    private int roundsCompleted = 0;

    void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        SpawnPennies(totalPennies);
    }

    void SpawnPennies(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Penny p = Instantiate(pennyPrefab, RandomPosition(), Quaternion.identity);
            activePennies.Add(p);
            p.Init();
        }
    }

    Vector2 RandomPosition()
    {
        return new Vector2(Random.Range(-7f, 7f), Random.Range(-4f, 4f));
    }

    public void PennyClicked(Penny penny)
    {
        // Check if all pennies are clicked
        bool allClicked = true;
        foreach (var p in activePennies)
        {
            if (!p.IsClicked())
            {
                allClicked = false;
                break;
            }
        }

        if (allClicked)
        {
            roundsCompleted++;
            if (roundsCompleted >= respawnTimes)
            {
                Debug.Log("Puzzle Complete!");
            }
            else
            {
                RespawnPennies(Random.Range(5, 11));
            }
        }
    }

    void RespawnPennies(int count)
    {
        // Randomly pick pennies to reset
        List<Penny> unclicked = new List<Penny>(activePennies);
        for (int i = 0; i < count && unclicked.Count > 0; i++)
        {
            int index = Random.Range(0, unclicked.Count);
            unclicked[index].ResetPenny();
            unclicked.RemoveAt(index);
        }
    }

    public void GameOver()
    {
        Debug.Log("Game Over!");
        Time.timeScale = 0; // stop the game
    }
}