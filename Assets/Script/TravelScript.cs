using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelScript : MonoBehaviour
{
    [System.Serializable]
    public class SceneRequirement
    {
        [Header("Scene")]
        public string sceneName;

        [Header("Required NPC")]
        public string requiredNPCID;

        // =========================
        // BLOCKED DIALOGUE
        // =========================
        [Header("Blocked Dialogue")]

        [TextArea]
        public string[] blockedDialogueLines;

        // WHO SPEAKS EACH LINE
        public string[] blockedSpeakerNames;

        // TRUE = NPC
        // FALSE = PLAYER
        public bool[] blockedIsNPCSpeaking;

        public AudioClip[] blockedVoiceClips;
    }

    [Header("Scene Requirements")]
    public SceneRequirement[] sceneRequirements;

    public void Load(string sceneName)
    {
        // =========================
        // SCENE REQUIREMENT CHECK
        // =========================
        foreach (SceneRequirement requirement in sceneRequirements)
        {
            if (requirement.sceneName == sceneName)
            {
                bool talkedToRequiredNPC =
                    GameState.triggeredIDs.Contains(requirement.requiredNPCID);

                // PLAYER HAS NOT TALKED TO REQUIRED NPC
                if (!talkedToRequiredNPC)
                {
                    if (DialogueManager.Instance != null)
                    {
                        DialogueManager.Instance.StartDialogue(
                            "Blocked",
                            requirement.blockedDialogueLines,
                            requirement.blockedSpeakerNames,
                            requirement.blockedIsNPCSpeaking,
                            false,
                            0, 0, 0, 0, 0,
                            null,
                            null,
                            requirement.blockedVoiceClips,
                            null
                        );
                    }

                    return;
                }
            }
        }

        // =========================
        // FRONT DOOR LOCK
        // =========================
        if (sceneName == "Hallway" && !GameState.hasFrontDoorKey)
        {
            Debug.Log("The door is locked. Find the key.");
            return;
        }

        // =========================
        // CONNECT 4 LOCK
        // =========================
        if (sceneName == "Hallway_Act2" &&
            !GameState.completedConnect4Puzzle)
        {
            Debug.Log("The door is locked. Complete the puzzle first.");
            return;
        }

        // =========================
        // PUZZLE LOCK
        // =========================
        if (sceneName == "ConnectFourPuzzle" &&
            !(GlobalUnlocksScript.completedPianoPuzzle &&
              GlobalUnlocksScript.completedLockPuzzle))
        {
            Debug.Log("Complete the puzzles first.");
            return;
        }

        // =========================
        // MUSIC SYSTEM
        // =========================
        if (MusicManager.Instance != null)
        {
            int distance = GetDistanceForScene(sceneName);
            MusicManager.Instance.SetDistanceLevel(distance);
        }

        // =========================
        // LOAD SCENE
        // =========================
        SceneManager.LoadScene(sceneName);
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