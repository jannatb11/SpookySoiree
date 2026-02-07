using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject daisyinv;
    public GameObject pianoinv;
    public GameObject mouseinv;

    void Start()
    {
        daisyinv.SetActive(PlayerPrefs.GetInt("DaisyCollected", 0) == 1);
    }


    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

}
   