using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Connect4GameScript : MonoBehaviour
{
    public string currentPiece;
    public GameObject yellowPiece;
    public GameObject redPiece;
    public GameObject piece;
    public Color yellow;
    public Color red;
    public Color BG;
    public int instructions = 0;
    // Start is called before the first frame update
    void Start()
    {
        currentPiece = "";
        GameObject IM = GameObject.Find("InventoryManager");
        IM.SetActive(false);
        IM.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
        CheckWin();
        
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
        if(instructions < 2){
            Instructions();
        }
        if(currentPiece != "yellow"){
            ClearPiece();
            currentPiece = "yellow";
            piece = Instantiate(yellowPiece, transform.position, transform.rotation, transform);
        } else{
            ClearPiece();
        }
        
    }
    public void ClickRed(){
        if(instructions < 2){
            Instructions();
        }
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
    public void CheckWin(){
        foreach(GameObject boardPiece in GameObject.FindGameObjectsWithTag("Connect4BoardPiece")){
            if(boardPiece.GetComponent<BoardPieceScript>().rotation == 0 || boardPiece.GetComponent<BoardPieceScript>().rotation == 360){

            } else{
                return;
            }
        }
        bool win = true;
        foreach(GameObject slot in GameObject.FindGameObjectsWithTag("Connect4Slot")){
            win = (win && slot.GetComponent<SlotWinScript>().correct);
            if(win == false){
                break;
            }
        }
        if(win){
            Win();
        }else{
            
        }
    }
    public void Win()
    {
        transform.Find("Victory Text").gameObject.SetActive(true);

        GameState.completedConnect4Puzzle = true;
    }
    public void Instructions(){
        TextMeshProUGUI readout = GameObject.Find("Instructions").GetComponent<TextMeshProUGUI>();
        instructions += 1;
        if(instructions < 1){
            readout.text = "Instructions: \nClick on a piece of the board to rotate it.";
        } else if(instructions < 2){
            readout.text = "Instructions: \nClick on a piece of the board to rotate it.\nClick on a colored chip to select it and place it in a slot.\nYou can only place chips on a rotated piece.";
        } else{
            readout.text = "Instructions: \nClick on a piece of the board to rotate it.\nClick on a colored chip to select it and place it in a slot.\nYou can only place chips on a rotated piece.\nMatch the pattern on the left.";
        }
    }
}
