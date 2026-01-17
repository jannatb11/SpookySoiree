using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanges : MonoBehaviour
{
    public void ChangeScene(int direction)
    {
        int currentIndex = SceneManager.GetActiveScene().buildIndex;
        int sceneCount = SceneManager.sceneCountInBuildSettings;

        int newIndex = currentIndex + direction;

        if (newIndex >= sceneCount)
        {
            newIndex = 0;
        }
        else if (newIndex < 0)
        {
            newIndex = sceneCount - 1;
        }

        SceneManager.LoadScene(newIndex);
    }
}
