using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionUI : MonoBehaviour
{
    public string itemName = "Plant";

    [TextArea(2, 5)]
    public string[] dialogueLines;

    public string[] speakerNames; // optional, same length as dialogueLines

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
            itemName,          // fallback speaker
            dialogueLines,
            speakerNames,     
            hasChoices,
            choiceLineIndex,
            yesStart,
            yesEnd,
            noStart,
            noEnd,
            this
        );
    }

    public void CollectItem()
    {
        collected = true;

        // Disable the whole UI image (parent)
        gameObject.transform.parent.gameObject.SetActive(false);
    }
}
