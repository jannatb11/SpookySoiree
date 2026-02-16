using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenDoor : MonoBehaviour
{
    [SerializeField] private string kitchenSceneName = "Kitchen";

    public void TryEnterKitchen()
    {
        if (!GameProgress.kitchenUnlocked)
        {
            Debug.Log("Kitchen is locked. Talk to Gurt first.");
            return;
        }

        Debug.Log("Entering Kitchen...");
        SceneManager.LoadScene(kitchenSceneName);
    }
}
