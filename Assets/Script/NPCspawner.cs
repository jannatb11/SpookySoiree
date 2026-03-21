using UnityEngine;

public class NPCSpawner : MonoBehaviour
{
    public GameObject[] npcsToEnable;

    void Start()
    {
        if (GameState.talkedToDaisy)
        {
            foreach (GameObject npc in npcsToEnable)
            {
                npc.SetActive(true);
            }
        }
    }
}