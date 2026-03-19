using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChanges : MonoBehaviour
{
    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    // Scene loop limits
    public int loopStartIndex = 0;
    public int loopEndIndex = 7;

    public void ChangeScene(int direction)
    {
        Scene currentScene = SceneManager.GetActiveScene();
        int nextIndex = currentScene.buildIndex + direction;

        if (nextIndex > loopEndIndex)
            nextIndex = loopStartIndex;
        else if (nextIndex < loopStartIndex)
            nextIndex = loopEndIndex;

        Scene nextScene = SceneManager.GetSceneByBuildIndex(nextIndex);

        //  BLOCK Kitchen unless unlocked
        if (nextScene.name == "Kitchen" && !DialogueGate.gurtUnlockedKitchen)
        {
            Debug.Log("Kitchen locked. Talk to Gurt first.");
            return;
        }

        SceneManager.LoadScene(nextIndex);
    }
}
