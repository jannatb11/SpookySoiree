using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterShotDoor : MonoBehaviour
{
    [SerializeField] private string sceneName = "CharacterShot";

    public void TryEnterCharacterShot()
    {
        //  If player already talked to Gurt -> lock room
        if (GameProgress.talkedToGurt)
        {
            Debug.Log("Character Shot room is now locked.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
