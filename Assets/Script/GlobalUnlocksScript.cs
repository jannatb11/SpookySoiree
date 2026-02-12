using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalUnlocksScript
{// these booleans start out as false but become true when you win a puzzle. It's used for unlocking areas
    public static bool completedLockPuzzle = false;
    public static bool completedPianoPuzzle = false;
    public static bool completedConnect4Puzzle = false;
}
