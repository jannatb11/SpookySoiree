using UnityEngine;
using UnityEngine.SceneManagement;

public class KitchenDoor : MonoBehaviour
{
    [Header("Scene To Load")]
    [SerializeField] private string nextSceneName = "Hallway";

    [Header("Requirement")]
    [SerializeField] private int requiredNPCs = 2;

    [SerializeField] private bool requireAnyItem = true;

    // THIS is called by your UI Button
    public void TryLeaveKitchen()
    {
        // Check NPC requirement
        if (GameState.kitchenNPCsTalkedTo.Count < requiredNPCs)
        {
            Debug.Log(
                "Locked: You need to talk to " +
                (requiredNPCs - GameState.kitchenNPCsTalkedTo.Count) +
                " more NPC(s)."
            );
            return;
        }

        // Check item requirement
        if (requireAnyItem && !InventoryManagerHasAnyItem())
        {
            Debug.Log("Locked: You need to pick up an item first.");
            return;
        }

        Debug.Log("Kitchen unlocked. Leaving...");
        SceneManager.LoadScene(nextSceneName);
    }

    bool InventoryManagerHasAnyItem()
    {
        if (InventoryManager.Instance == null) return false;
        return InventoryManager.Instance.HasAnyItem();
    }
}