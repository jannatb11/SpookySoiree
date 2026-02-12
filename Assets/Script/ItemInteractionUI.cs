using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionUI : MonoBehaviour
{
    public string itemName = "Key";

    [TextArea(2, 5)]
    public string[] dialogueLines;

    public string[] speakerNames; // ADD THIS

    public bool hasChoices = true;
    public int choiceLineIndex = 1;

    public int yesStart, yesEnd;
    public int noStart, noEnd;

    private Button button;
    private bool collected;

    void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(OnItemClicked);
    }

    void OnItemClicked()
    {
        if (DialogueManager.Instance.IsDialogueActive) return;
        if (collected) return;

        DialogueManager.Instance.StartDialogue(
            itemName,
            dialogueLines,
            speakerNames,   // NEW
            hasChoices,
            choiceLineIndex,
            yesStart,
            yesEnd,
            noStart,
            noEnd,
            this,           // item reference
            null            // no NPC
        );
    }

    public void CollectItem()
    {
        collected = true;
        gameObject.SetActive(false);
    }
}
