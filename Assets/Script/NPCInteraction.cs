using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcID;
    public string npcName;

    [Header("Animation")]
    public Animator animator;
    
    [Header("Dialogue")]
    public string[] dialogueLines;
    public string[] speakerNames;

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
        if (GameState.removedNPCs.Contains(npcID))
        {
            Destroy(gameObject);
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
            this
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
        {
            GameProgress.introFinished = true;
            Debug.Log("Intro unlocked by " + npcName);
        }

        if (unlockKitchen)
        {
            GameProgress.kitchenUnlocked = true;
            Debug.Log("Kitchen unlocked by " + npcName);
        }

        if (teleportAfterDialogue && sceneSwitcher != null)
        {
            sceneSwitcher.TriggerSceneSwitch();
        }

        if (disappearAfterDialogue)
        {
            GameState.removedNPCs.Add(npcID);
            Destroy(gameObject);
        }
    }
}
