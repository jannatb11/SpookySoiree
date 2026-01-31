using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanges : MonoBehaviour
{
    public void StartGame()
{
    SceneManager.LoadScene(1);
}

    //keep all puzzles and main menu after player movement scenes
    public int loopStartIndex = 0;
    public int loopEndIndex = 7;

    public void ChangeScene(int direction)
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int nextIndex = currentIndex + direction;

        if (nextIndex > loopEndIndex)
        {
            nextIndex = loopStartIndex;
        }
        else if (nextIndex < loopStartIndex)
        {
            nextIndex = loopEndIndex;
        }

        SceneManager.LoadScene(nextIndex);
    }
}
