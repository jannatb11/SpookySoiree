using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DastardlyEvilResetButtonOfDoomScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Lethe(){
        GameState.removedNPCs = new HashSet<string>();
        GameProgress.talkedToGurt = false;
        DialogueGate.introFinished = false; 
        DialogueGate.gurtUnlockedKitchen = false;
        GlobalUnlocksScript.completedLockPuzzle = false;
        GlobalUnlocksScript.completedPianoPuzzle = false;
        GlobalUnlocksScript.completedConnect4Puzzle = false;
        SceneManager.LoadScene("Hallway");
    }
}
