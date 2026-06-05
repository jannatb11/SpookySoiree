using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutlineHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline outline;

    [Header("Scene Settings")]
    public string sceneName;

    [Header("Kitchen / NPC Requirements")]
    public bool useSpecificNPCs = false;
    public string[] requiredNPCIDs;

    [Header("Item Requirements")]
    public bool requireItem = false;
    public enum RequiredItem
    {
        Daisy,
        Piano,
        Mouse
    }
    public RequiredItem requiredItem;

    [Header("Puzzle Requirements")]
    public bool requireSpecificPuzzles = false;

    public PuzzleRequirement puzzle1;
    public PuzzleRequirement puzzle2;

    public enum PuzzleRequirement
    {
        None,
        Connect4,
        Piano,
        Lock,
        DodgeMinigame,
        FruitPiercer,
        PenniesPuzzle
    }


    [Header("Colors")]
    public Color validColor = Color.green;
    public Color lockedColor = Color.red;

    [Header("Glow Settings")]
    public float hoverAlpha = 1f;

    private Color baseColor;

    void Start()
    {
        outline = GetComponent<Outline>();
        outline.enabled = true;

        SetAlpha(0f);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        bool canUse = CanUse();
        baseColor = canUse ? validColor : lockedColor;
        SetAlpha(hoverAlpha);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        SetAlpha(0f);
    }

    public bool CanUse()
    {
        // Specific NPC requirements
        if (useSpecificNPCs && requiredNPCIDs != null && requiredNPCIDs.Length > 0)
        {
            for (int i = 0; i < requiredNPCIDs.Length; i++)
            {
                if (!GameState.triggeredIDs.Contains(requiredNPCIDs[i]))
                    return false;
            }
        }

        // Puzzle door
        if (sceneName == "Hallway_Act2")
        {
            if (!GameState.AllPuzzlesCompleted())
                return false;
        }

        // Front door lock
        if (sceneName == "Hallway" && !GameState.hasFrontDoorKey)
            return false;

        // Inventory item requirement
        if (requireItem)
        {
            if (InventoryManager.Instance == null)
                return false;

            switch (requiredItem)
            {
                case RequiredItem.Daisy:
                    if (!InventoryManager.Instance.HasDaisy())
                        return false;
                    break;

                case RequiredItem.Piano:
                    if (!InventoryManager.Instance.HasPiano())
                        return false;
                    break;

                case RequiredItem.Mouse:
                    if (!InventoryManager.Instance.HasMouse())
                        return false;
                    break;
            }
        }

        if (requireSpecificPuzzles)
        {
            if (!IsPuzzleCompleted(puzzle1))
                return false;

            if (!IsPuzzleCompleted(puzzle2))
                return false;
        }

        return true;
    }

    void SetAlpha(float a)
    {
        Color c = baseColor;
        c.a = a;
        outline.effectColor = c;
    }

    bool IsPuzzleCompleted(PuzzleRequirement puzzle)
    {
        switch (puzzle)
        {
            case PuzzleRequirement.Connect4:
                return GameState.completedConnect4Puzzle;

            case PuzzleRequirement.Piano:
                return GlobalUnlocksScript.completedPianoPuzzle;

            case PuzzleRequirement.Lock:
                return GlobalUnlocksScript.completedLockPuzzle;

            case PuzzleRequirement.DodgeMinigame:
                return GlobalUnlocksScript.completedDodgeMinigame;

            case PuzzleRequirement.FruitPiercer:
                return GlobalUnlocksScript.completedFruitPiercer;

            case PuzzleRequirement.PenniesPuzzle:
                return GlobalUnlocksScript.completedpennypuzzle;

            default:
                return true;
        }
    }
}