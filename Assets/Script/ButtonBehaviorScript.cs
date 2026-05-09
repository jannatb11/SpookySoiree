using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonBehaviorScript : MonoBehaviour
{
    public bool hovered;
    // Start is called before the first frame update
    void Start()
    {
        hovered = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(hovered){
            transform.localScale = new Vector3(1.25f, 1.25f, 1f);
        } else{
            transform.localScale = new Vector3(1, 1, 1);
        }
    }
    public void MouseOver(){
        hovered = true;
    }
    public void MouseOff(){
        hovered = false;
    }
}
