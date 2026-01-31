using UnityEngine;

public class ItemInteraction : MonoBehaviour
{
    [Header("Item Info")]
    public string itemID; // MUST be unique
    public string itemName = "Item";

    [TextArea(2, 5)]
    public string[] dialogueLines;

    public bool hasChoices = true;
    public int choiceLineIndex = 1;

    public int yesJumpToLine = 2;
    public int yesEndLine = 3;

    public int noJumpToLine = 4;
    public int noEndLine = 5;

    private void Start()
    {
        // Check if item was already picked up
        if (PlayerPrefs.GetInt(itemID, 0) == 1)
        {
            Destroy(gameObject);
        }
    }

    private void OnMouseDown()
    {
        if (DialogueManager.DialogueActive)
            return;

        DialogueManager.Instance.StartDialogue(
            itemName,
            dialogueLines,
            hasChoices,
            choiceLineIndex,
            yesJumpToLine,
            yesEndLine,
            noJumpToLine,
            noEndLine,
            gameObject,
            true
        );
    }

    // Called by DialogueManager when item is picked up
    public void MarkAsCollected()
    {
        PlayerPrefs.SetInt(itemID, 1);
        PlayerPrefs.Save();
    }
}