using UnityEngine;

public class SpawnAfterElevator : MonoBehaviour
{
    [Header("Trigger Required")]
    public string requiredTriggerID = "Elevator";

    [Header("Spawn Mode")]
    public bool useSpawnInsteadOfEnable = false;

    public GameObject npcPrefab;   // optional
    public Transform spawnPoint;   // optional

    void Awake()
    {
        bool unlocked = GameState.triggeredIDs.Contains(requiredTriggerID);

        if (!unlocked)
        {
            gameObject.SetActive(false);
            return;
        }

        if (useSpawnInsteadOfEnable)
        {
            if (npcPrefab != null && spawnPoint != null)
            {
                Instantiate(npcPrefab, spawnPoint.position, spawnPoint.rotation);
            }

            Destroy(gameObject);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}