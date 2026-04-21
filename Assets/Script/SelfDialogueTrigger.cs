using UnityEngine;

public class SelfDialogueTrigger : MonoBehaviour
{
    [Header("ID")]
    public string triggerID;

    [Header("Dialogue")]
    [TextArea(2, 5)] public string[] lines;

    void Start()
    {
        if (GameState.pendingSelfDialogueID == triggerID)
        {
            GameState.pendingSelfDialogueID = ""; // reset so it doesn't repeat

            StartCoroutine(PlayDialogue());
        }
    }

    System.Collections.IEnumerator PlayDialogue()
    {
        yield return new WaitForSeconds(0.5f);

        int len = lines.Length;

        string[] speakers = new string[len];
        bool[] isNPCSpeaking = new bool[len];

        for (int i = 0; i < len; i++)
        {
            speakers[i] = "Player";      
            isNPCSpeaking[i] = false; // player speaking
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