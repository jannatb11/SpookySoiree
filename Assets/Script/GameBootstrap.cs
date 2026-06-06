using UnityEngine;

public static class GameBootstrap
{
    public static void ResetAllGameData()
    {
        Debug.Log("=== GAME RESET START ===");

        // Reset NPC/world state
        GameState.ResetGame();

        // Reset puzzle progress
        GlobalUnlocksScript.ResetProgress();

        // Reset story flags
        GameProgress.introFinished = false;
        GameProgress.kitchenUnlocked = false;

        // Reset Unity saved data (optional but common for full restart)
        PlayerPrefs.DeleteAll();

        Debug.Log("=== GAME RESET COMPLETE ===");
    }
}