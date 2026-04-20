using UnityEngine;
using UnityEngine.UI;

public class ItemInteractionUI : MonoBehaviour
{
    public string itemName;

    [TextArea]
    public string[] dialogueLines;

    public string[] speakerNames;   //  NEW
    public bool[] isNPCSpeaking;

    public AudioClip[] voiceClips;

    private bool collected;

    public void OnItemClicked()
    {
        if (DialogueManager.Instance.IsDialogueActive || collected)
            return;

        DialogueManager.Instance.StartDialogue(
            itemName,
            dialogueLines,
            speakerNames,
            isNPCSpeaking,
            false,
            0, 0, 0, 0, 0,
            this,
            null,
            voiceClips
        );
    }

    public void OnDialogueComplete()
    {
        collected = true;
        gameObject.SetActive(false);
    }
}