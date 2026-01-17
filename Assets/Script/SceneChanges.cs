using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanges : MonoBehaviour
{
    public void ChangeScene(int direction)
    {
        int index = SceneManager.GetActiveScene().buildIndex;
        SceneManager.LoadScene(index + direction);
    }
}
