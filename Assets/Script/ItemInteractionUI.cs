using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class ItemInteractionUI : MonoBehaviour
{
    public string itemName = "Key"; // Unique ID for the item

    [TextArea(2, 5)]
    public string[] dialogueLines;

    public bool hasChoices = true;
    public int choiceLineIndex = 1;

    public int yesStart, yesEnd;
    public int noStart, noEnd;

    private Button button;
    private bool collected;

    // Static dictionary to keep track of collected items across scenes
    private static HashSet<string> collectedItems = new HashSet<string>();

    void Awake()
    {
        button = GetComponent<Button>();

        if (button != null)
            button.onClick.AddListener(OnItemClicked);

        // Check if this item was already collected
        if (collectedItems.Contains(itemName))
        {
            DisableItem();
        }
    }

    void OnItemClicked()
    {
        if (DialogueManager.Instance.IsDialogueActive) return;
        if (collected) return;

        DialogueManager.Instance.StartDialogue(
            itemName,
            dialogueLines,
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

        // Mark item as collected
        collectedItems.Add(itemName);

        // Disable the parent UI (image + button)
        DisableItem();
    }

    private void DisableItem()
    {
        if (transform.parent != null)
            transform.parent.gameObject.SetActive(false);
        else
            gameObject.SetActive(false);
    }
}
