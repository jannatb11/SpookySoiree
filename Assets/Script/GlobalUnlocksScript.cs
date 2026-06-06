using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalUnlocksScript
{// these booleans start out as false but become true when you win a puzzle. It's used for unlocking areas
    public static bool completedLockPuzzle = false;
    public static bool completedPianoPuzzle = false;
    public static bool completedConnect4Puzzle = false;
    public static bool completedFruitPiercer = false;
    public static bool completedDodgeMinigame = false;
    public static bool completedpennypuzzle = false;


    public static void ResetProgress()
    {
        completedLockPuzzle = false;
        completedPianoPuzzle = false;
        completedConnect4Puzzle = false;

        // Add any other unlocks here
    }
}


