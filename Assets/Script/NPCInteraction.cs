using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    public string npcName = "Toot";

    [TextArea(2, 5)]
    public string[] dialogueLines;

    public string[] speakerNames; // SAME length as dialogueLines

    public bool hasChoices;
    public int choiceLineIndex;

    public int yesJumpToLine, yesEndLine;
    public int noJumpToLine, noEndLine;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnNPCClicked);
    }

    void OnNPCClicked()
    {
        if (DialogueManager.Instance.IsDialogueActive) return;

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
            null
        );
    }
}
