using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelScript : MonoBehaviour
{
    public void Load(string sceneName)
    {
        // Prevent leaving hallway before intro is finished
        if (!GameProgress.introFinished
            && sceneName == "LRS3"
            && SceneManager.GetActiveScene().name == "Hallway")
        {
            Debug.Log("Finish talking to Toot first.");
            return;
        }

        // Kitchen locked until Gurt unlocks it
        if (sceneName == "Kitchen" && !GameProgress.kitchenUnlocked)
        {
            Debug.Log("Kitchen locked.");
            return;
        }

        // Puzzle gate
        if (sceneName == "ConnectFourPuzzle" &&
            !(GlobalUnlocksScript.completedPianoPuzzle &&
              GlobalUnlocksScript.completedLockPuzzle))
        {
            Debug.Log("Complete the Lockpick and Piano puzzle first.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
