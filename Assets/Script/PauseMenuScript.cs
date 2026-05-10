using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuScript : MonoBehaviour
{
    public GameObject pauseMenu;
    public bool isPaused;
    public GameObject currentPauseMenu;
    // Start is called before the first frame update
    void Start()
    {
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.P)){
            if(isPaused){ //Resume the game if it is paused.
                Resume();
            } else{ // Pause the game if it is playing.
                Pause();
            }
        }
    }
    public void Pause(){
        isPaused = true;
        currentPauseMenu = Instantiate(pauseMenu);
        currentPauseMenu.GetComponent<PauseCanvasScript>().PMS = GetComponent<PauseMenuScript>();
        Debug.Log("pause");
    }
    public void Resume(){
        isPaused = false;
        Debug.Log("unpause");
        Destroy(currentPauseMenu);
    }
}
