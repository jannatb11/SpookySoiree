using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;

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
    // SPAWN SYSTEM
    // =========================
    [Header("Spawn Trigger System")]
    public bool disableUntilTriggered;
    public string requiredTriggerID;

    // =========================
    // CUTSCENE SYSTEM
    // =========================
    [Header("Video Cutscene")]
    public bool playVideoAfterDialogue;
    public VideoPlayer videoPlayer;

    public GameObject cutscenePanel;

    [Header("Cutscene End Behavior")]
    public bool disappearAfterCutscene;

    [Header("Scene After Cutscene")]
    public bool loadSceneAfterCutscene;
    public string cutsceneSceneName;

    private bool videoPlaying;

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

        if (!string.IsNullOrEmpty(npcID))
            GameState.triggeredIDs.Add(npcID);

        if (countsForKitchenExitProgress)
            GameState.kitchenNPCsTalkedTo.Add(npcID);

        if (!string.IsNullOrEmpty(triggerSelfDialogueID))
            GameState.pendingSelfDialogueID = triggerSelfDialogueID;

        if (countsForDoorProgress && !GameState.talkedToNPCs.Contains(npcID))
            GameState.talkedToNPCs.Add(npcID);

        if (unlockDaisyProgress)
            GameState.talkedToDaisy = true;

        if (unlockIntro)
            GameProgress.introFinished = true;

        if (unlockKitchen)
            GameProgress.kitchenUnlocked = true;

        if (teleportAfterDialogue && sceneSwitcher != null)
            sceneSwitcher.TriggerSceneSwitch();

        // =========================
        // START CUTSCENE
        // =========================
        if (playVideoAfterDialogue && videoPlayer != null)
        {
            videoPlaying = true;

            if (cutscenePanel != null)
                cutscenePanel.SetActive(true);

            if (DialogueManager.Instance != null)
                DialogueManager.Instance.enabled = false;

            videoPlayer.Play();
            return;
        }

        if (disappearAfterDialogue)
        {
            GameState.removedNPCs.Add(npcID);
            Destroy(gameObject);
        }
    }

    // =========================
    // VIDEO FINISHED
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


        if (loadSceneAfterCutscene && !string.IsNullOrEmpty(cutsceneSceneName))
        {
            GameState.pendingSelfDialogueID = "Gurt_Cutscene";
            GameState.resetInventoryOnNextScene = true;
            SceneManager.LoadScene(cutsceneSceneName);
            return;
        }

        if (disappearAfterCutscene)
        {
            GameState.removedNPCs.Add(npcID);
            Destroy(gameObject);
        }
    }
}