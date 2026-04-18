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
        // Already removed check
        if (GameState.removedNPCs.Contains(npcID))
        {
            Destroy(gameObject);
            return;
        }

        // NEW: Hide until Daisy is talked to
        if (requireDaisyToAppear && !GameState.talkedToDaisy)
        {
            gameObject.SetActive(false);
            return;
        }
    }

    public void Interact()
    {
        if (DialogueManager.Instance.IsDialogueActive)
            return;

        // Start talking animation
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
        voiceClips //  NEW
 );
    }

    public void OnDialogueComplete()
    {
        // Stop talking animation
        if (animator != null)
            animator.SetBool("isTalking", false);

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
