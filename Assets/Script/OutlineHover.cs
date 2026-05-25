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
      
        if (useSpecificNPCs && requiredNPCIDs != null && requiredNPCIDs.Length > 0)
        {
            for (int i = 0; i < requiredNPCIDs.Length; i++)
            {
                if (!GameState.triggeredIDs.Contains(requiredNPCIDs[i]))
                    return false;
            }
            return true;
        }

        // Puzzle door
        if (sceneName == "Hallway_Act2")
        {
            return GameState.AllPuzzlesCompleted();
        }

        // Front door lock
        if (sceneName == "Hallway" && !GameState.hasFrontDoorKey)
            return false;

        return true;
    }

    void SetAlpha(float a)
    {
        Color c = baseColor;
        c.a = a;
        outline.effectColor = c;
    }
}