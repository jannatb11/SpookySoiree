using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcID;
    public string npcName;

    [Header("Dialogue")]
    public string[] dialogueLines;
    public string[] speakerNames;
    public bool[] isNPCSpeaking;

    [Header("Voice")]
    public AudioClip[] voiceClips;

    [Header("Animation")]
    public Animator animator;

   
    [Header("Animation Per Line")]
    public string[] animationStates;

    [Header("Spawn Conditions")]
    public bool requireDaisyToAppear;
    public bool requireDoorToAppear;
    public bool requireAllDoorNPCs;

    [Header("Progress Tracking")]
    public bool countsForDoorProgress;

    [Header("Self Dialogue Trigger")]
    public string triggerSelfDialogueID;

    [Header("Unlock Flags")]
    public bool unlockDaisyProgress;
    public bool unlockIntro;
    public bool unlockKitchen;

    [Header("Scene / State")]
    public bool teleportAfterDialogue;
    public SceneSwitcher sceneSwitcher;

    public bool disappearAfterDialogue;

    void Awake()
    {
        if (GameState.removedNPCs.Contains(npcID))
        {
            Destroy(gameObject);
            return;
        }

        if (requireDaisyToAppear && !GameState.talkedToDaisy)
        {
            gameObject.SetActive(false);
            return;
        }

        if (requireDoorToAppear && !GameState.openedDoor)
        {
            gameObject.SetActive(false);
            return;
        }

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

        DialogueManager.Instance.StartDialogue(
            npcName,
            dialogueLines,
            speakerNames,
            isNPCSpeaking,
            false,
            0, 0, 0, 0, 0,
            null,
            this,
            voiceClips,
            animationStates 
        );
    }

    public void OnDialogueComplete()
    {
        if (animator != null)
            animator.SetBool("isTalking", false);

        
        if (!string.IsNullOrEmpty(triggerSelfDialogueID))
        {
            GameState.pendingSelfDialogueID = triggerSelfDialogueID;
        }

        if (countsForDoorProgress)
        {
            GameState.talkedToNPCs.Add(npcID);

            if (GameState.talkedToNPCs.Count >= GameState.requiredDoorNPCCount)
            {
                GameState.allDoorNPCsTalkedTo = true;

                GameObject finalNPC = GameObject.Find("FinalNPC");
                if (finalNPC != null)
                    finalNPC.SetActive(true);
            }
        }

        if (unlockDaisyProgress)
            GameState.talkedToDaisy = true;

        if (unlockIntro)
            GameProgress.introFinished = true;

        if (unlockKitchen)
            GameProgress.kitchenUnlocked = true;

        if (teleportAfterDialogue && sceneSwitcher != null)
            sceneSwitcher.TriggerSceneSwitch();

        if (disappearAfterDialogue)
        {
            GameState.removedNPCs.Add(npcID);
            Destroy(gameObject);
        }
    }
}