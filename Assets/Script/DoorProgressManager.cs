using UnityEngine;
using System.Collections.Generic;

public class DoorProgressManager : MonoBehaviour
{
    [Header("NPCs Required")]
    public List<string> requiredNPCIDs;

    [Header("Final NPC")]
    public GameObject npcToReveal;

    private bool hasTriggered = false;

    void Start()
    {
        // Make sure the NPC starts OFF
        if (npcToReveal != null)
            npcToReveal.SetActive(false);
    }

    void Update()
    {
        if (hasTriggered) return;

        foreach (string id in requiredNPCIDs)
        {
            if (!GameState.talkedToNPCs.Contains(id))
            {
                Debug.Log(" Missing NPC: " + id);
                return;
            }
        }

        Debug.Log(" ALL NPCs FOUND");
        RevealFinalNPC();
    }

    void RevealFinalNPC()
    {
        hasTriggered = true;

        if (npcToReveal != null)
        {
            npcToReveal.SetActive(true);
            Debug.Log("Final NPC unlocked!");
        }
    }
}