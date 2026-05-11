using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class OutlineHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private Outline outline;

    [Header("Scene Settings")]
    public string sceneName;

    [Header("Kitchen Settings")]
    public bool isKitchenDoor = false;
    public int requiredNPCs = 2;

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

        //  START COMPLETELY INVISIBLE
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
        SetAlpha(0f); // hide again
    }

    bool CanUse()
    {
        // Kitchen logic
        if (isKitchenDoor)
        {
            return GameState.kitchenNPCsTalkedTo.Count >= requiredNPCs;
        }

        // Front door lock
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