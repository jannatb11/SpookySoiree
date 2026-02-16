using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionUI : MonoBehaviour
{
    public string itemName;

    [TextArea(2, 5)]
    public string[] dialogueLines;

    public string[] speakerNames;

    public bool hasChoices;
    public int choiceLineIndex;
    public int yesStart, yesEnd;
    public int noStart, noEnd;

    private bool collected = false;

    void OnItemClicked()
    {
        if (DialogueManager.Instance.IsDialogueActive) return;
        if (collected) return;

        DialogueManager.Instance.StartDialogue(
            itemName,
            dialogueLines,
            speakerNames,
            hasChoices,
            choiceLineIndex,
            yesStart,
            yesEnd,
            noStart,
            noEnd,
            this,      // item reference
            null       // no NPC
        );
    }

    public void OnDialogueComplete()
    {
        collected = true;

        Button button = GetComponent<Button>();
        if (button != null)
        {
            button.interactable = false;
        }

        gameObject.SetActive(false); // remove item visually
    }
}
