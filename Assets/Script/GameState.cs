using System.Collections.Generic;

public static class GameState
{
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



    public static bool hasFrontDoorKey = false;
}