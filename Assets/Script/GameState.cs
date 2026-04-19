using System.Collections.Generic;

public static class GameState
{
    public static HashSet<string> removedNPCs = new HashSet<string>();

    public static bool talkedToDaisy = false;
    public static bool openedDoor = false;

    //  Track which NPCs were talked to
    public static HashSet<string> talkedToNPCs = new HashSet<string>();

    //  How many are required
    public static int requiredDoorNPCCount = 3; // change this

    //  Final unlock flag
    public static bool allDoorNPCsTalkedTo = false;
}