using UnityEngine;

public class PuzzleTriggerHide : MonoBehaviour
{
    public enum PuzzleType
    {
        Connect4,
        Piano,
        Lock,
        DodgeMinigame,
        FruitPiercer
    }

    public PuzzleType puzzle;

    void Update()
    {
        CheckIfCompleted();
    }

    void CheckIfCompleted()
    {
        bool completed = false;

        switch (puzzle)
        {
            case PuzzleType.Connect4:
                completed = GameState.completedConnect4Puzzle;
                break;

            case PuzzleType.Piano:
                completed = GlobalUnlocksScript.completedPianoPuzzle;
                break;

            case PuzzleType.Lock:
                completed = GlobalUnlocksScript.completedLockPuzzle;
                break;

            case PuzzleType.DodgeMinigame:
                completed = GlobalUnlocksScript.completedDodgeMinigame;
                break;

            case PuzzleType.FruitPiercer:
                completed = GlobalUnlocksScript.completedFruitPiercer;
                break;
        }

        if (completed)
        {
            gameObject.SetActive(false);
        }
    }
}