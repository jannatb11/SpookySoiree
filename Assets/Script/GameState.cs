using System.Collections.Generic;

public static class GameState
{
    public static HashSet<string> removedNPCs = new HashSet<string>();

    public static bool talkedToDaisy = false;
    public static bool openedDoor = false;

    public static HashSet<string> talkedToNPCs = new HashSet<string>();
    public static int requiredDoorNPCCount = 3;
    public static bool allDoorNPCsTalkedTo = false;

   
    public static string pendingSelfDialogueID = "";
}