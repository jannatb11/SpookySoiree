using UnityEngine;

public class NPCInteraction : MonoBehaviour
{
    public string npcName = "Bob";

    [TextArea(2, 5)]
    public string[] dialogueLines;

    public bool hasChoices = true;
    public int choiceLineIndex = 2;

    public int yesJumpToLine = 3;
    public int yesEndLine = 4;

    public int noJumpToLine = 5;
    public int noEndLine = 6;

    private void OnMouseDown()
    {
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
}
