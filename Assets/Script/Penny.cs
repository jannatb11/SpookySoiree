using UnityEngine;

public class Penny : MonoBehaviour
{
    private SpriteRenderer sr;

    private void Awake()
    {
        sr = GetComponent<SpriteRenderer>();
    }

    public void SetWhite()
    {
        sr.color = Color.white;
    }

    public void SetGreen()
    {
        sr.color = Color.green;
    }

    private void OnMouseDown()
    {
        GM.Instance.PennyClicked(this);
    }
}
