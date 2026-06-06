using System.Collections.Generic;
using UnityEngine;

public static class GameState
{

    public static bool inventoryManagerInitialized = false;



    // =========================
    // NPC REMOVAL SYSTEM
    // =========================
    public static HashSet<string> removedNPCs = new HashSet<string>();

    // =========================
    // MAIN STORY FLAGS
    // =========================
    public static bool talkedToDaisy = false;
    public static bool openedDoor = false;
    public static bool allDoorNPCsTalkedTo = false;

    // =========================
    // NPC PROGRESSION TRACKING
    // =========================
    public static HashSet<string> talkedToNPCs = new HashSet<string>();

    public static bool resetInventoryOnNextScene;

    public static bool completedConnect4Puzzle = false;

    public static HashSet<string> playedVideoCutscenes = new HashSet<string>();
    public static bool CanLeaveKitchen(int requiredNPCs)
    {
        return kitchenNPCsTalkedTo.Count >= requiredNPCs;
    }

   

    public static HashSet<string> kitchenNPCsTalkedTo = new HashSet<string>();

    // =========================
    // GLOBAL EVENT SYSTEM
    // (Elevator, switches, triggers, etc.)
    // =========================
    public static HashSet<string> triggeredIDs = new HashSet<string>();

    // =========================
    // SELF DIALOGUE SYSTEM
    // =========================
    public static string pendingSelfDialogueID = "";

    public static bool AllPuzzlesCompleted()
    {
        return completedConnect4Puzzle
            && GlobalUnlocksScript.completedPianoPuzzle
            && GlobalUnlocksScript.completedLockPuzzle;
    }

    public static bool hasFrontDoorKey = false;


    public static void ResetGame()
    {
        Debug.Log("RESET CALLED - BEFORE: " + removedNPCs.Count);

        removedNPCs.Clear();
        talkedToNPCs.Clear();
        kitchenNPCsTalkedTo.Clear();
        triggeredIDs.Clear();
        playedVideoCutscenes.Clear();

        talkedToDaisy = false;
        openedDoor = false;
        allDoorNPCsTalkedTo = false;

        resetInventoryOnNextScene = false;
        hasFrontDoorKey = false;
        completedConnect4Puzzle = false;

        pendingSelfDialogueID = "";

        Debug.Log("RESET CALLED - AFTER: " + removedNPCs.Count);
    }
}