using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MouseIndicatorScript : MonoBehaviour
{
    public Sprite neutral;
    public Sprite leftClick;
    public Sprite rightClick;
    public RectTransform canvas;
    public float canvasWidth;
    public float canvasHeight;
    public float mouseWidth;
    public float mouseHeight;
    public Vector3 gotoPos;
    public AudioSource click;
    public bool moving;
    // Start is called before the first frame update
    void Start()
    {
        moving = false;
        canvas = GameObject.Find("Canvas").GetComponent<RectTransform>();
        canvasWidth = canvas.rect.width;
        canvasHeight = canvas.rect.height;
        mouseWidth = GetComponent<RectTransform>().rect.width;
        mouseHeight = GetComponent<RectTransform>().rect.height;
        transform.position = new Vector3(canvasWidth/2 , canvasHeight/2);
        gotoPos = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        canvasWidth = canvas.rect.width;
        canvasHeight = canvas.rect.height;
        mouseWidth = GetComponent<RectTransform>().rect.width;
        mouseHeight = GetComponent<RectTransform>().rect.height;
        if(Mathf.Round(transform.position.x) == Mathf.Round(gotoPos.x) && Mathf.Round(transform.position.y) == Mathf.Round(gotoPos.y)){
            moving = false;
        } else{
            moving = true;
        }
        transform.position = Vector3.Lerp(transform.position, gotoPos, Time.deltaTime * 10);
    }
    public void LeftClick(){
        StartCoroutine(Click(true));
    }
    public void RightClick(){
        StartCoroutine(Click(false));
    }
    public IEnumerator Click(bool left){
        Move();
        yield return new WaitWhile(() => moving);
        click.Play();
        if(left == true){
            GetComponent<Image>().sprite = leftClick;
        }else{
            GetComponent<Image>().sprite = rightClick;
        }
        yield return new WaitForSeconds(0.125f);
        GetComponent<Image>().sprite = neutral;
    }
    public void Move(){
        Vector3 targetPos = new Vector3(Random.Range(mouseWidth/2, canvasWidth-mouseWidth/2), Random.Range(mouseHeight/2, canvasHeight - mouseHeight/2));
        gotoPos = targetPos;
    }
}
