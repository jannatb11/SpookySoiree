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

        //  Block leaving ONLY the Hallway until intro dialogue is finished
        if (currentScene.name == "Hallway" && !DialogueGate.introFinished)
        {
            Debug.Log("Scene change blocked: intro not finished");
            return;
        }

        int nextIndex = currentScene.buildIndex + direction;

        if (nextIndex > loopEndIndex)
            nextIndex = loopStartIndex;
        else if (nextIndex < loopStartIndex)
            nextIndex = loopEndIndex;

        SceneManager.LoadScene(nextIndex);
    }
}
