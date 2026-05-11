using UnityEngine;
using UnityEngine.EventSystems;

public class UIHoverGlow : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public float hoverScale = 1.15f;
    private Vector3 startScale;

    void Start()
    {
        startScale = transform.localScale;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = startScale * hoverScale;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = startScale;
    }
}