using UnityEngine;
using System.Collections;

public class SelfDialogueTrigger : MonoBehaviour
{
    [Header("ID")]
    public string triggerID;

    [Header("Dialogue")]
    [TextArea(2, 5)]
    public string[] lines;

    [Header("Audio")]
    public AudioClip dialogueSound;
    public AudioSource audioSource;

    void Start()
    {
        if (GameState.pendingSelfDialogueID == triggerID)
        {
            GameState.pendingSelfDialogueID = null;

            StartCoroutine(PlayDialogue());
        }
    }

    IEnumerator PlayDialogue()
    {
        yield return new WaitForSeconds(0.5f);

        // Play sound before dialogue starts
        if (dialogueSound != null)
        {
            if (audioSource != null)
                audioSource.PlayOneShot(dialogueSound);
            else
                AudioSource.PlayClipAtPoint(dialogueSound, transform.position);
        }

        if (DialogueManager.Instance == null)
            yield break;

        int len = lines.Length;

        string[] speakers = new string[len];
        bool[] isNPCSpeaking = new bool[len];

        for (int i = 0; i < len; i++)
        {
            speakers[i] = "Player";
            isNPCSpeaking[i] = false;
        }

        DialogueManager.Instance.StartDialogue(
            "Player",
            lines,
            speakers,
            isNPCSpeaking,
            false,
            0, 0, 0, 0, 0,
            null,
            null,
            null,
            null
        );
    }
}