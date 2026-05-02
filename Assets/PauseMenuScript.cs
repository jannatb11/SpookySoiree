using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{ //This script pauses and unpauses the game
    public GameObject pauseMenu;
    public GameObject pauseScreen;
    public bool isPaused;
    void Start()
    {
        isPaused = false;
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            if(isPaused){ //Unpause the game if it is paused.
                isPaused = false;
                Debug.Log("unpause");
                Destroy(pauseScreen);
            } else{ // Pause the game if it is unpaused.
                isPaused = true;
                Debug.Log("pause");
                pauseScreen = Instantiate(pauseMenu);
            }
        }
    }
}
