using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelScript : MonoBehaviour
{
    public void Load(string sceneName)
    {
        // Locks (keep your existing ones)

        

        if (sceneName == "ConnectFourPuzzle" &&
            !(GlobalUnlocksScript.completedPianoPuzzle &&
              GlobalUnlocksScript.completedLockPuzzle))
        {
            Debug.Log("Complete the puzzles first.");
            return;
        }

        //  SET DISTANCE BASED ON SCENE
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
            case "Dining": return 3;     // full volume
            case "LRS1": return 2;    // medium
            case "Storage": return 2;
            case "LRS3": return 1;
            case "Kitchen": return 1;
            case "LRS2": return 1;       // quiet
            default: return 0;              // far -> ambience
        }
    }
}