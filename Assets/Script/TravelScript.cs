using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class TravelScript : MonoBehaviour
{
    [System.Serializable]
    public class SceneRequirement
    {
        [Header("Scene")]
        public string sceneName;

        

        [Header("Required NPC")]
        public string requiredNPCID;

        [Header("Arrow Visibility Requirement")]
        public string requiredNPCToShowArrow;

        [Header("Blocked Dialogue")]
        [TextArea] public string[] blockedDialogueLines;
        public string[] blockedSpeakerNames;
        public bool[] blockedIsNPCSpeaking;
        public AudioClip[] blockedVoiceClips;

        [Header("Enter Dialogue")]
        public bool playDialogueBeforeEntering;

        [TextArea] public string[] enterDialogueLines;
        public string[] enterSpeakerNames;
        public bool[] enterIsNPCSpeaking;
        public AudioClip[] enterVoiceClips;

        [Header("Auto Dialogue On Scene Enter")]
        public string autoStartNPCID;
    }


    [Header("Scene Requirements")]
    public SceneRequirement[] sceneRequirements;

    private bool isLoading;

    private void Awake()
    {
        if (sceneRequirements == null)
            sceneRequirements = new SceneRequirement[0];
    }

    public void Load(string sceneName)
    {
        // Prevent spam clicking
        if (isLoading) return;
        isLoading = true;

        if (sceneRequirements == null)
            sceneRequirements = new SceneRequirement[0];

        SceneRequirement matchingRequirement = null;

       
        for (int i = 0; i < sceneRequirements.Length; i++)
        {
            SceneRequirement requirement = sceneRequirements[i];

            if (requirement == null)
                continue;

            if (requirement.sceneName == sceneName)
            {
                matchingRequirement = requirement;
                break;
            }
        }

       
        if (matchingRequirement != null)
        {
            bool talkedToRequiredNPC =
                GameState.triggeredIDs.Contains(matchingRequirement.requiredNPCID);

            // BLOCKED
            if (!talkedToRequiredNPC)
            {
                if (DialogueManager.Instance != null)
                {
                    DialogueManager.Instance.StartDialogue(
                        "Blocked",
                        matchingRequirement.blockedDialogueLines,
                        matchingRequirement.blockedSpeakerNames,
                        matchingRequirement.blockedIsNPCSpeaking,
                        false,
                        0, 0, 0, 0, 0,
                        null,
                        null,
                        matchingRequirement.blockedVoiceClips,
                        null
                    );
                }

                isLoading = false;
                return;
            }

            // ENTER DIALOGUE FIRST
            if (matchingRequirement.playDialogueBeforeEntering)
            {
                StartCoroutine(PlayDialogueThenLoad(matchingRequirement, sceneName));
                return;
            }
        }

       
        if (sceneName == "Hallway" && !GameState.hasFrontDoorKey)
        {
            Debug.Log("The door is locked. Find the key.");
            isLoading = false;
            return;
        }

        if (sceneName == "Hallway_Act2" && !GameState.completedConnect4Puzzle)
        {
            Debug.Log("The door is locked. Complete the puzzle first.");
            isLoading = false;
            return;
        }

        if (sceneName == "ConnectFourPuzzle" &&
            !(GlobalUnlocksScript.completedPianoPuzzle &&
              GlobalUnlocksScript.completedLockPuzzle))
        {
            Debug.Log("Complete the puzzles first.");
            isLoading = false;
            return;
        }

        if(sceneName == "FruitPiercer" &&
            !(GlobalUnlocksScript.completedDodgeMinigame &&
            GlobalUnlocksScript.completedpennypuzzle))
        {
            Debug.Log("Complete the puzzles first.");
            isLoading = false;
            return;
        }


        

      
        if (matchingRequirement != null &&
            !string.IsNullOrEmpty(matchingRequirement.autoStartNPCID))
        {
            GameState.pendingSelfDialogueID = matchingRequirement.autoStartNPCID;
        }

        SceneManager.LoadScene(sceneName);
    }

    IEnumerator PlayDialogueThenLoad(SceneRequirement req, string sceneName)
    {
        if (DialogueManager.Instance != null)
        {
            DialogueManager.Instance.StartDialogue(
                "Enter",
                req.enterDialogueLines,
                req.enterSpeakerNames,
                req.enterIsNPCSpeaking,
                false,
                0, 0, 0, 0, 0,
                null,
                null,
                req.enterVoiceClips,
                null
            );

            while (DialogueManager.Instance.IsDialogueActive)
            {
                yield return null;
            }
        }

        
        if (!string.IsNullOrEmpty(req.autoStartNPCID))
        {
            GameState.pendingSelfDialogueID = req.autoStartNPCID;
        }

        SceneManager.LoadScene(sceneName);
    }

    public bool CanShowArrow(string sceneName)
    {
        if (sceneRequirements == null)
            return true;

        for (int i = 0; i < sceneRequirements.Length; i++)
        {
            var req = sceneRequirements[i];
            if (req == null) continue;

            if (req.sceneName == sceneName)
            {
                // If no requirement, always show
                if (string.IsNullOrEmpty(req.requiredNPCToShowArrow))
                    return true;

                // Only show if NPC was talked to
                return GameState.triggeredIDs.Contains(req.requiredNPCToShowArrow);
            }
        }

        return true;
    }

    int GetDistanceForScene(string sceneName)
    {
        switch (sceneName)
        {
            case "Dining": return 3;
            case "LRS1": return 2;
            case "Storage": return 2;
            case "LRS3": return 1;
            case "Kitchen": return 1;
            case "LRS2": return 1;
            default: return 0;
        }
    }
}