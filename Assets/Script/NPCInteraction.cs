using UnityEngine;
using UnityEngine.UI;

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

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(OnNPCClicked);
        else
            Debug.LogError("NPCInteraction: No Button found on NPC UI Image");
    }

    void OnNPCClicked()
    {
        if (DialogueManager.Instance.IsDialogueActive)
            return;

        DialogueManager.Instance.StartDialogue(
            npcName,
            dialogueLines,
            hasChoices,
            choiceLineIndex,
            yesJumpToLine,
            yesEndLine,
            noJumpToLine,
            noEndLine,
            null   // NPCs are not items
        );
    }
}
