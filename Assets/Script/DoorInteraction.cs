using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorHotspotUI : MonoBehaviour
{
    [Header("Dialogue")]
    public string npcName;
    public string[] dialogueLines;
    public string[] speakerNames;
    public AudioClip[] voiceClips;

    [Header("Choices")]
    public bool hasChoices;
    public int choiceLineIndex;
    public int yesJumpToLine;
    public int yesEndLine;
    public int noJumpToLine;
    public int noEndLine;

    [Header("NPC To Reveal")]
    public GameObject npcToReveal;

    private Button button;
    private bool hasTriggered = false;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(OnClick);
    }

    void OnClick()
    {
        if (DialogueManager.Instance.IsDialogueActive || hasTriggered)
            return;

        hasTriggered = true;

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
            null,
            voiceClips
        );

        StartCoroutine(WaitForDialogueEnd());
    }

    IEnumerator WaitForDialogueEnd()
    {
        while (DialogueManager.Instance.IsDialogueActive)
            yield return null;

        // unlock door
        GameState.openedDoor = true;

        //  reveal first NPC
        if (npcToReveal != null)
        {
            npcToReveal.SetActive(true);
        }

        // disable button
        button.interactable = false;

        if (button.image != null)
            button.image.enabled = false;
    }
}