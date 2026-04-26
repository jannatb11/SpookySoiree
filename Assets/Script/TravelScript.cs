using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelScript : MonoBehaviour
{
    public void Load(string sceneName)
    {
        //  FRONT DOOR LOCK
        if (sceneName == "Hallway"  && !GameState.hasFrontDoorKey)
        {
            Debug.Log("The door is locked. Find the key.");
            return;
        }

        // EXISTING PUZZLE LOCK
        if (sceneName == "ConnectFourPuzzle" &&
            !(GlobalUnlocksScript.completedPianoPuzzle &&
              GlobalUnlocksScript.completedLockPuzzle))
        {
            Debug.Log("Complete the puzzles first.");
            return;
        }

        // MUSIC SYSTEM
        if (MusicManager.Instance != null)
        {
            int distance = GetDistanceForScene(sceneName);
            MusicManager.Instance.SetDistanceLevel(distance);
        }

        SceneManager.LoadScene(sceneName);
    }

    int GetDistanceForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Dining": return 3;
            case "LRS1": return 2;
            case "Storage": return 2;
            case "LRS3": return 1;
            case "Kitchen": return 1;
            case "LRS2": return 1;
            default: return 0;
        }
    }
}