using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DoorHotspotUI : MonoBehaviour
{
    public string npcName;
    public string[] dialogueLines;
    public string[] speakerNames;
    public bool[] isNPCSpeaking;
    public AudioClip[] voiceClips;

    public GameObject npcToReveal;

    private Button button;
    private bool hasTriggered;

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
            isNPCSpeaking,
            false,
            0, 0, 0, 0, 0,
            null,
            null,
            voiceClips,
            null 
        );

        StartCoroutine(Wait());
    }

    IEnumerator Wait()
    {
        while (DialogueManager.Instance.IsDialogueActive)
            yield return null;

        GameState.openedDoor = true;

        if (npcToReveal != null)
            npcToReveal.SetActive(true);

        button.interactable = false;
        if (button.image != null)
            button.image.enabled = false;
    }
}