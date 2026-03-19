using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string npcName = "Bob";

    [TextArea(2, 5)]
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
}
