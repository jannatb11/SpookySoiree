using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string itemID;

    [Header("Spawn Trigger System")]
    public bool disableUntilTriggered;
    public string requiredTriggerID;

    void Awake()
    {
        if (disableUntilTriggered && !string.IsNullOrEmpty(requiredTriggerID))
        {
            if (!GameState.triggeredIDs.Contains(requiredTriggerID))
            {
                gameObject.SetActive(false);
                return;
            }
        }
    }

    public void Collect()
    {
        GameState.hasFrontDoorKey = true;

        Debug.Log("Picked up front door key!");

        Destroy(gameObject);
    }
}