using UnityEngine;

public class Penny : MonoBehaviour
{
    private bool clicked = false;
    private float spawnTime;
    public float lifetime = 10f; 

    private SpriteRenderer sr;

    public void Init()
    {
        clicked = false;
        spawnTime = Time.time;
        sr = GetComponent<SpriteRenderer>();
        sr.color = Color.green; 
        gameObject.SetActive(true);
    }

    void Update()
    {
        if (!clicked && Time.time - spawnTime > lifetime)
        {
            GM.Instance.GameOver();
        }
    }

    void OnMouseDown()
    {
        if (!clicked)
        {
            clicked = true;
            sr.color = Color.gray; 
            GM.Instance.PennyClicked(this);
        }
    }

    public bool IsClicked() => clicked;

    public void ResetPenny()
    {
        Init();
    }
}