using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TravelScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void Load(string sceneName)
    {
        if (!DialogueGate.introFinished && sceneName == "LRS3" && SceneManager.GetActiveScene().name == "Hallway")
            return;
        
        if (sceneName == "Kitchen" && !DialogueGate.gurtUnlockedKitchen)
        {
            Debug.Log("Kitchen locked.");
            return;
        }

        SceneManager.LoadScene(sceneName);
    }

}
