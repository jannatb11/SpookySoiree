using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class PauseCanvasScript : MonoBehaviour
{
    public PauseMenuScript PMS;
    // Start is called before the first frame update
    void Start()
    {
            
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Resume(){
        PMS.Resume();
    }
    public void Restart(){
        SceneManager.LoadScene(0);
    }
}
