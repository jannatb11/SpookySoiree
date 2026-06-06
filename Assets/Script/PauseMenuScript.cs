using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    public GameObject currentPauseMenu;

    void Start()
    {
        isPaused = false;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Pause()
    {
        isPaused = true;

        currentPauseMenu = Instantiate(pauseMenu);
        currentPauseMenu.GetComponent<PauseCanvasScript>().PMS = this;

        Debug.Log("Game Paused");
    }

    public void Resume()
    {
        isPaused = false;

        if (currentPauseMenu != null)
            Destroy(currentPauseMenu);

        Debug.Log("Game Resumed");
    }

    public void RestartGame()
    {
        Debug.Log("Restarting Game...");

        // 1. Reset EVERYTHING
        GameBootstrap.ResetAllGameData();

        // 2. Make sure time is normal
        Time.timeScale = 1f;

        // 3. Reload first scene
        SceneManager.LoadScene(0);
    }
}