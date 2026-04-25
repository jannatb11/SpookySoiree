using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenDoor : MonoBehaviour
{
    [Header("Scene To Load")]
    [SerializeField] private string nextSceneName = "Hallway";

    [Header("Requirement")]
    [SerializeField] private int requiredNPCs = 2;

    // THIS is called by your UI Button
    public void TryLeaveKitchen()
    {
        if (GameState.kitchenNPCsTalkedTo.Count < requiredNPCs)
        {
            Debug.Log(
                "Locked: You need to talk to " +
                (requiredNPCs - GameState.kitchenNPCsTalkedTo.Count) +
                " more NPC(s)."
            );
            return;
        }

        Debug.Log("Kitchen unlocked. Leaving...");
        SceneManager.LoadScene(nextSceneName);
    }
}