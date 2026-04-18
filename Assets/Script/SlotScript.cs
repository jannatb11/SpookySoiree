using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SlotScript : MonoBehaviour
{
    public string currentColor;
    public Connect4GameScript gms;
    public Image img;
    public GameObject above;
    public GameObject below;
    // Start is called before the first frame update
    void Start()
    {
        gms = GameObject.Find("Canvas").GetComponent<Connect4GameScript>();
        currentColor = "";
        img = GetComponent<Image>();
    }

    // Update is called once per frame
    void Update()
    {
        if(above){
            SlotScript ss = above.GetComponent<SlotScript>();
            if(ss.currentColor != "" && transform.parent.GetComponent<BoardPieceScript>().rotating == false && above.transform.parent.GetComponent<BoardPieceScript>().rotating == false && currentColor == ""){
                if(transform.parent == above.transform.parent || ((transform.parent.GetComponent<BoardPieceScript>().rotation == 0 || transform.parent.GetComponent<BoardPieceScript>().rotation == 360) && (above.transform.parent.GetComponent<BoardPieceScript>().rotation == 0 || above.transform.parent.GetComponent<BoardPieceScript>().rotation == 360))){
                    currentColor = ss.currentColor;
                    ss.currentColor = "";
                }

            }
        }
        switch(currentColor){
            case "yellow":
                img.color = gms.yellow;
                break;
            case "red":
                img.color = gms.red;
                break;
            default:
                img.color = gms.BG;
                break;
        }

    }
    public void AddPiece(){
        if((transform.parent.GetComponent<BoardPieceScript>().rotation == 0 || transform.parent.GetComponent<BoardPieceScript>().rotation == 360) || ((currentColor == "yellow" || currentColor == "red") && (gms.currentPiece == "yellow" || gms.currentPiece == "red"))){
            return;
        }
        currentColor = gms.currentPiece;
        gms.ClearPiece();
    }
    public void OnTriggerStay2D(Collider2D other){
        TriggerLogic(other);
    }
    public void OnTriggerEnter2D(Collider2D other){
        TriggerLogic(other);
    }
    public void TriggerLogic(Collider2D other){
        if(!other.isTrigger){
            other.gameObject.GetComponent<SlotScript>().above = gameObject;
        } else{
            //above = other.gameObject;
        }
    }
    public void OnTriggerExit2D(Collider2D other){
        above = null;
    }

    
}
