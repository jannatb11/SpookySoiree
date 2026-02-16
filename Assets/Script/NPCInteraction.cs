using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcName;

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



    public void Interact()
    {
        if (DialogueManager.Instance.IsDialogueActive)
            return;

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
            gameObject.SetActive(false);
    }
}

