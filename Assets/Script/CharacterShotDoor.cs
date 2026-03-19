using UnityEngine;
using UnityEngine.SceneManagement;

public class CharacterShotDoor : MonoBehaviour
{
    [SerializeField] private string sceneName = "CharacterShot";

    public void TryEnterCharacterShot()
    {
        if (GameProgress.kitchenUnlocked)
        {
            Debug.Log("Character Shot room is now locked.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }
}
