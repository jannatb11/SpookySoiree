using UnityEngine;
using UnityEngine.Video;
using UnityEngine.SceneManagement;
using System.Collections;
using System.Collections.Generic;

public class NPCInteraction : MonoBehaviour
{
    public EndDemoTransition endDemoTransition;
    public bool triggerEndDemo;


    [System.Serializable]
    public class DialogueSpawnEvent
    {
        [Tooltip("The dialogue line index that triggers this event.")]
        public int lineNumber;

        [Tooltip("Objects to activate when this line is reached.")]
        public GameObject[] objectsToSpawn;

    }

    [Header("Interaction Requirement")]
    public string requiredNPCToTalkTo;

    [Header("Dialogue Spawn Events")]
    public DialogueSpawnEvent[] dialogueSpawnEvents;

    private HashSet<int> triggeredDialogueEvents = new HashSet<int>();

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

    [Header("Act Transition")]
    public bool playActTransition;
    public SceneTransition sceneTransition;

    [Header("Door Transition")]
    public bool playDoorTransition;
    public string doorSceneName;
    public SceneTransition doorTransition;

    [Header("Demo Ending")]
    public bool showEndDemoScreen;

    public CanvasGroup fadeScreen;
    public CanvasGroup endDemoText;
    public float endDemoFadeTime = 2f;

    void Start()
    {
        // Auto-trigger self dialogue after scene load
        if (!string.IsNullOrEmpty(npcID) &&
            GameState.pendingSelfDialogueID == npcID)
        {
            GameState.pendingSelfDialogueID = null;
            Interact();
        }
    }
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
       
        if (!string.IsNullOrEmpty(requiredNPCToTalkTo) &&
            !GameState.triggeredIDs.Contains(requiredNPCToTalkTo))
        {
            Debug.Log("I should talk to someone else first...");
            return;
        }

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
        Debug.Log("OnDialogueComplete called for: " + npcName);

        Debug.Log("triggerEndDemo = " + triggerEndDemo);

        if (endDemoTransition == null)
            Debug.LogError("endDemoTransition is NULL!");
        else
            Debug.Log("endDemoTransition found.");

        if (triggerEndDemo && endDemoTransition != null)
        {
            Debug.Log("STARTING END DEMO");

            endDemoTransition.StartEndDemo();
            return;
        }
        if (triggerEndDemo && endDemoTransition != null)
        {
            endDemoTransition.StartEndDemo();
            return;
        }

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

        if (teleportAfterDialogue && sceneSwitcher != null && !playDoorTransition)
        {
            sceneSwitcher.TriggerSceneSwitch();
        }

        if (playDoorTransition && !string.IsNullOrEmpty(doorSceneName))
        {
            if (doorTransition != null)
            {
                doorTransition.StartTransition(doorSceneName);
            }
            else
            {
                SceneManager.LoadScene(doorSceneName);
            }

            return;
        }

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

            if (playActTransition && sceneTransition != null)
            {
                sceneTransition.StartTransition(cutsceneSceneName);
            }
            else
            {
                SceneManager.LoadScene(cutsceneSceneName);
            }

            return;
        }

        

        if (disappearAfterCutscene)
        {
            GameState.removedNPCs.Add(npcID);
            Destroy(gameObject);
        }
    }

    public void CheckDialogueEvents(int currentLine)
{
    if (dialogueSpawnEvents == null)
        return;

    if (triggeredDialogueEvents.Contains(currentLine))
        return;

    foreach (DialogueSpawnEvent evt in dialogueSpawnEvents)
    {
        if (evt.lineNumber == currentLine)
        {
            triggeredDialogueEvents.Add(currentLine);

            foreach (GameObject obj in evt.objectsToSpawn)
            {
                if (obj == null)
                    continue;

                CanvasGroup cg = obj.GetComponent<CanvasGroup>();

                if (cg != null)
                {
                    StartCoroutine(FadeIn(cg, 1f));
                }
                else
                {
                    Debug.LogWarning(
                        obj.name +
                        " does not have a CanvasGroup component."
                    );
                }
            }
        }
    }



}



    IEnumerator FadeIn(CanvasGroup canvasGroup, float duration)
    {
        GameObject obj = canvasGroup.gameObject;

        obj.SetActive(true);

        float time = 0f;
        canvasGroup.alpha = 0f;

        while (time < duration)
        {
            time += Time.deltaTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1f, time / duration);
            yield return null;
        }

        canvasGroup.alpha = 1f;

        
        Animator anim = obj.GetComponent<Animator>();

        if (anim != null)
        {
            anim.SetTrigger("Show");
        }
    }

    public void TestDoor()
    {
        doorTransition.StartTransition("Character");
    }

    

}