using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class HoverHighlight : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image targetImage;

    public Color normalColor = Color.white;
    public Color hoverColor = Color.yellow;

    void Start()
    {
        if (targetImage == null)
            targetImage = GetComponent<Image>();

        targetImage.color = normalColor;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        targetImage.color = hoverColor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        targetImage.color = normalColor;
    }
}