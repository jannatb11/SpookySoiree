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

    public bool hasChoices = true;
    public int choiceLineIndex = 2;

    public int yesJumpToLine = 3;
    public int yesEndLine = 4;

    public int noJumpToLine = 5;
    public int noEndLine = 6;

    private void OnMouseDown()
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
            hasChoices,
            choiceLineIndex,
            yesJumpToLine,
            yesEndLine,
            noJumpToLine,
            noEndLine
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
