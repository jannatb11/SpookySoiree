using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class SceneFadeIn : MonoBehaviour
{
    [Header("Fade")]
    public Image fadeImage;
    public float fadeDuration = 1f;

    [Header("Self Dialogue")]
    public string selfDialogueID;

    IEnumerator Start()
    {
        // Start black
        fadeImage.gameObject.SetActive(true);

        Color c = fadeImage.color;
        c.a = 1f;
        fadeImage.color = c;

        float t = 0f;

        while (t < fadeDuration)
        {
            t += Time.deltaTime;

            c.a = Mathf.Lerp(1f, 0f, t / fadeDuration);
            fadeImage.color = c;

            yield return null;
        }

        // Fully transparent
        c.a = 0f;
        fadeImage.color = c;

        // Hide fade image
        fadeImage.gameObject.SetActive(false);

        // Start self dialogue
        StartSelfDialogue();
    }

    void StartSelfDialogue()
    {
        if (string.IsNullOrEmpty(selfDialogueID))
            return;

        // Save ID just like the rest of your game
        GameState.pendingSelfDialogueID = selfDialogueID;

        NPCInteraction[] npcs = FindObjectsOfType<NPCInteraction>();

        foreach (NPCInteraction npc in npcs)
        {
            if (npc.npcID == selfDialogueID)
            {
                npc.Interact();
                break;
            }
        }
    }
}