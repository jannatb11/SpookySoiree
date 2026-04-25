using UnityEngine;
using UnityEngine.Video;

public class NPCInteraction : MonoBehaviour
{
    [Header("NPC Info")]
    public string npcID;
    public string npcName;

    [Header("Dialogue")]
    public string[] dialogueLines;
    public string[] speakerNames;
    public bool[] isNPCSpeaking;

    [Header("Voice")]
    public AudioClip[] voiceClips;

    [Header("Animation")]
    public Animator animator;

    [Header("Animation Per Line")]
    public string[] animationStates;

    [Header("Spawn Conditions")]
    public bool requireDaisyToAppear;
    public bool requireDoorToAppear;
    public bool requireAllDoorNPCs;

    [Header("Progress Tracking")]
    public bool countsForDoorProgress;

    [Header("Self Dialogue Trigger")]
    public string triggerSelfDialogueID;

    [Header("Unlock Flags")]
    public bool unlockDaisyProgress;
    public bool unlockIntro;
    public bool unlockKitchen;

    [Header("Kitchen Progress")]
    public bool countsForKitchenExitProgress;

    [Header("Scene / State")]
    public bool teleportAfterDialogue;
    public SceneSwitcher sceneSwitcher;

    public bool disappearAfterDialogue;

    // =========================
    // SPAWN SYSTEM (ELEVATOR ETC.)
    // =========================
    [Header("Spawn Trigger System")]
    public bool disableUntilTriggered;
    public string requiredTriggerID;

    // =========================
    //  VIDEO CUTSCENE (AFTER DIALOGUE)
    // =========================
    [Header("Video Cutscene")]
    public bool playVideoAfterDialogue;
    public VideoPlayer videoPlayer;

    [Header("Cutscene End Behavior")]
    public bool disappearAfterCutscene;

    private bool videoPlaying;

    public GameObject cutscenePanel;

    void Awake()
    {
        if (GameState.removedNPCs.Contains(npcID))
        {
            Destroy(gameObject);
            return;
        }

        if (requireDaisyToAppear && !GameState.talkedToDaisy)
        {
            gameObject.SetActive(false);
            return;
        }

        if (requireDoorToAppear && !GameState.openedDoor)
        {
            gameObject.SetActive(false);
            return;
        }

        if (requireAllDoorNPCs && !GameState.allDoorNPCsTalkedTo)
        {
            gameObject.SetActive(false);
            return;
        }

        // =========================
        // SPAWN LOCK (ELEVATOR SYSTEM)
        // =========================
        if (disableUntilTriggered && !string.IsNullOrEmpty(requiredTriggerID))
        {
            if (!GameState.triggeredIDs.Contains(requiredTriggerID))
            {
                gameObject.SetActive(false);
                return;
            }
        }
    }

    public void Interact()
    {
        if (DialogueManager.Instance.IsDialogueActive || videoPlaying)
            return;

        DialogueManager.Instance.StartDialogue(
            npcName,
            dialogueLines,
            speakerNames,
            isNPCSpeaking,
            false,
            0, 0, 0, 0, 0,
            null,
            this,
            voiceClips,
            animationStates
        );
    }

    public void OnDialogueComplete()
    {
        if (animator != null)
            animator.SetBool("isTalking", false);

        // =========================
        // GLOBAL TRIGGER SYSTEM (ELEVATOR ETC.)
        // =========================
        if (!string.IsNullOrEmpty(npcID))
        {
            GameState.triggeredIDs.Add(npcID);
        }

        if (countsForKitchenExitProgress)
        {
            GameState.kitchenNPCsTalkedTo.Add(npcID);
        }

        if (!string.IsNullOrEmpty(triggerSelfDialogueID))
        {
            GameState.pendingSelfDialogueID = triggerSelfDialogueID;
        }

        if (countsForDoorProgress)
        {
            GameState.talkedToNPCs.Add(npcID);

            if (GameState.talkedToNPCs.Count >= GameState.requiredDoorNPCCount)
            {
                GameState.allDoorNPCsTalkedTo = true;

                GameObject finalNPC = GameObject.Find("FinalNPC");
                if (finalNPC != null)
                    finalNPC.SetActive(true);
            }
        }

        if (unlockDaisyProgress)
            GameState.talkedToDaisy = true;

        if (unlockIntro)
            GameProgress.introFinished = true;

        if (unlockKitchen)
            GameProgress.kitchenUnlocked = true;

        if (teleportAfterDialogue && sceneSwitcher != null)
            sceneSwitcher.TriggerSceneSwitch();

        // =========================
        //  PLAY VIDEO AFTER DIALOGUE (YOUR REQUEST)
        // =========================
        if (playVideoAfterDialogue && videoPlayer != null)
        {
            videoPlaying = true;

            if (cutscenePanel != null)
                cutscenePanel.SetActive(true);

            videoPlayer.Play();

            if (DialogueManager.Instance != null)
                DialogueManager.Instance.enabled = false;

            return;
        }

        if (disappearAfterDialogue)
        {
            GameState.removedNPCs.Add(npcID);
            Destroy(gameObject);
        }
    }

    // =========================
    // CALLED WHEN VIDEO ENDS
    // =========================
    public void OnVideoFinished()
    {
        videoPlaying = false;

        if (videoPlayer != null)
            videoPlayer.Stop();

        if (cutscenePanel != null)
            cutscenePanel.SetActive(false);

        if (DialogueManager.Instance != null)
            DialogueManager.Instance.enabled = true;

        // =========================
        //  REMOVE GURT AFTER CUTSCENE
        // =========================
        if (disappearAfterCutscene)
        {
            GameState.removedNPCs.Add(npcID);
            Destroy(gameObject);
            return;
        }
    }
}