using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenDoor : MonoBehaviour
{
    [SerializeField] private string kitchenSceneName = "Kitchen";

    public void TryEnterKitchen()
    {
        if (!GameProgress.talkedToGurt)
        {
            Debug.Log("Kitchen is locked. Talk to Gurt first.");
            return;
        }

        SceneManager.LoadScene(kitchenSceneName);
    }
}
