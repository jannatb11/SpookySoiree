using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Connect4GameScript : MonoBehaviour
{
    public string currentPiece;
    public GameObject yellowPiece;
    public GameObject redPiece;
    public GameObject piece;
    public Color yellow;
    public Color red;
    public Color BG;
    // Start is called before the first frame update
    void Start()
    {
        currentPiece = "";
    }

    // Update is called once per frame
    void Update()
    {
        if(piece != null){
            piece.transform.position = new Vector3(Input.mousePosition.x - 5, Input.mousePosition.y - 5, 0);
        }
        switch(currentPiece){
            case "yellow":
                
                break;
            case "red":
                
                break;
            default:
                
                break;
        }
    }
    public void ClickYellow(){
        if(currentPiece != "yellow"){
            ClearPiece();
            currentPiece = "yellow";
            piece = Instantiate(yellowPiece, transform.position, transform.rotation, transform);
        } else{
            ClearPiece();
        }
        
    }
    public void ClickRed(){
        if(currentPiece != "red"){
            ClearPiece();
            currentPiece = "red";
            piece = Instantiate(redPiece, transform.position, transform.rotation, transform);
        } else{
            ClearPiece();
        }
    }
    public void ClearPiece(){
        Destroy(piece);
        currentPiece = "";
    }
}
