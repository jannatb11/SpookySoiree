using UnityEngine;

public class UIButtonTravelWithRequirement : MonoBehaviour
{
    [Header("Travel")]
    public TravelScript travelScript;
    public string sceneName;

    [Header("Blocked Dialogue (Optional Custom)")]
    [TextArea] public string[] blockedLines;
    public string[] blockedSpeakers;
    public bool[] blockedIsNPC;
    public AudioClip[] blockedVoiceClips;

    private OutlineHover outlineHover;

    void Start()
    {
        outlineHover = GetComponent<OutlineHover>();
    }

    public void OnClick()
    {
        if (DialogueManager.Instance != null &&
            DialogueManager.Instance.IsDialogueActive)
            return;

        if (outlineHover != null && !outlineHover.CanUse())
        {
            ShowBlockedDialogue();
            return;
        }

        travelScript.Load(sceneName);
    }

    void ShowBlockedDialogue()
    {
        if (DialogueManager.Instance == null) return;

        string[] lines;
        string[] speakers;
        bool[] isNPC;

        if (blockedLines != null && blockedLines.Length > 0)
        {
            lines = blockedLines;

            speakers = (blockedSpeakers != null && blockedSpeakers.Length == lines.Length)
                ? blockedSpeakers
                : new string[] { "Player" };

            isNPC = (blockedIsNPC != null && blockedIsNPC.Length == lines.Length)
                ? blockedIsNPC
                : new bool[] { false };
        }
        else
        {
            lines = new string[] { GetBlockedMessage() };
            speakers = new string[] { "Player" };
            isNPC = new bool[] { false };
        }

        DialogueManager.Instance.StartDialogue(
            "Blocked",
            lines,
            speakers,
            isNPC,
            false,
            0, 0, 0, 0, 0,
            null,
            null,
            blockedVoiceClips,
            null
        );
    }

    string GetBlockedMessage()
    {
        if (outlineHover == null || outlineHover.requiredNPCIDs == null)
            return "I can't go there yet...";

        string missing = "";

        for (int i = 0; i < outlineHover.requiredNPCIDs.Length; i++)
        {
            if (!GameState.triggeredIDs.Contains(outlineHover.requiredNPCIDs[i]))
            {
                missing += outlineHover.requiredNPCIDs[i] + ", ";
            }
        }

        if (missing.Length > 2)
            missing = missing.Substring(0, missing.Length - 2); // remove last comma

        return "I should talk to " + missing + " first...";
    }
}