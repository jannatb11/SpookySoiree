using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            if(isPaused){ //Unpause the game if it is paused.
                isPaused = false;
                Debug.Log("unpause");
            } else{ // Pause the game if it is unpaused.
                isPaused = true;
                Debug.Log("pause");
            }
        }
    }
}
