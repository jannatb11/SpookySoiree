using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcID;
    public string npcName;

    [Header("Voice Lines")]
    public AudioClip[] voiceClips;

    [Header("Animation")]
    public Animator animator;

    [Header("Dialogue")]
    public string[] dialogueLines;
    public string[] speakerNames;

    [Header("Spawn Conditions")]
    public bool requireDaisyToAppear;
    public bool requireDoorToAppear;
    public bool requireAllDoorNPCs; //  FINAL NPC USES THIS

    [Header("Progress Tracking")]
    public bool countsForDoorProgress;

    [Header("Unlock Flags")]
    public bool unlockDaisyProgress;

    [Header("Choices")]
    public bool hasChoices;
    public int choiceLineIndex;
    public int yesJumpToLine;
    public int yesEndLine;
    public int noJumpToLine;
    public int noEndLine;

    [Header("Unlock Settings")]
    public bool unlockIntro;
    public bool unlockKitchen;

    [Header("After Dialogue")]
    public bool disappearAfterDialogue = false;

    [Header("Scene Teleport")]
    public bool teleportAfterDialogue = false;
    public SceneSwitcher sceneSwitcher;

    void Awake()
    {
        // Remove permanently if flagged
        if (GameState.removedNPCs.Contains(npcID))
        {
            Destroy(gameObject);
            return;
        }

        // Daisy condition
        if (requireDaisyToAppear && !GameState.talkedToDaisy)
        {
            gameObject.SetActive(false);
            return;
        }

        // Door condition
        if (requireDoorToAppear && !GameState.openedDoor)
        {
            gameObject.SetActive(false);
            return;
        }

        // Final NPC condition
        if (requireAllDoorNPCs && !GameState.allDoorNPCsTalkedTo)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    public void Interact()
    {
        if (DialogueManager.Instance.IsDialogueActive)
            return;

        if (animator != null)
            animator.SetBool("isTalking", true);

        DialogueManager.Instance.StartDialogue(
            npcName,
            dialogueLines,
            speakerNames,
            hasChoices,
            choiceLineIndex,
            yesJumpToLine,
            yesEndLine,
            noJumpToLine,
            noEndLine,
            null,
            this,
            voiceClips
        );
    }

    public void OnDialogueComplete()
    {
        if (animator != null)
            animator.SetBool("isTalking", false);

        //  Track NPC progress
        if (countsForDoorProgress)
        {
            GameState.talkedToNPCs.Add(npcID);
            Debug.Log("Talked to: " + npcID);

            //  Check if ALL required NPCs are done
            if (GameState.talkedToNPCs.Count >= GameState.requiredDoorNPCCount)
            {
                GameState.allDoorNPCsTalkedTo = true;
                Debug.Log("All door NPCs talked to!");

                //  INSTANTLY reveal final NPC
                GameObject finalNPC = GameObject.Find("FinalNPC");
                if (finalNPC != null)
                {
                    finalNPC.SetActive(true);
                }
                else
                {
                    Debug.LogWarning("FinalNPC not found in scene!");
                }
            }
        }

        ApplyUnlocks();
    }

    public void ApplyUnlocks()
    {
        if (unlockIntro)
            GameProgress.introFinished = true;

        if (unlockKitchen)
            GameProgress.kitchenUnlocked = true;

        if (unlockDaisyProgress)
        {
            GameState.talkedToDaisy = true;
            Debug.Log("Daisy progression unlocked!");
        }

        if (teleportAfterDialogue && sceneSwitcher != null)
            sceneSwitcher.TriggerSceneSwitch();

        if (disappearAfterDialogue)
        {
            GameState.removedNPCs.Add(npcID);
            Destroy(gameObject);
        }
    }
}