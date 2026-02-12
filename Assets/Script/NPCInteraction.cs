using UnityEngine;
using UnityEngine.UI;

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC Settings")]
    public string npcID; // MUST be unique for each NPC
    public string npcName = "Toot";

    [TextArea(2, 5)]
    public string[] dialogueLines;

    public string[] speakerNames;

    [Header("Choices")]
    public bool hasChoices;
    public int choiceLineIndex;
    public int yesJumpToLine, yesEndLine;
    public int noJumpToLine, noEndLine;

    [Header("Removal")]
    public bool removeAfterDialogue;

    private Button button;

    void Awake()
    {
        // If this NPC was already removed before, destroy it instantly
        if (GameState.removedNPCs.Contains(npcID))
        {
            Destroy(gameObject);
            return;
        }

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
            null,
            this // PASS THIS NPC
        );
    }

    public void RemoveNPC()
    {
        if (!removeAfterDialogue) return;

        GameState.removedNPCs.Add(npcID);
        Destroy(gameObject);
    }
}
