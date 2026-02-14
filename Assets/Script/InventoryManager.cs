using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public GameObject daisyinv;
    public GameObject pianoinv;
    public GameObject mouseinv;
    public GameObject daisyinv2;

    void Start()
    {
        //daisyinv.SetActive(PlayerPrefs.GetInt("DaisyCollected", 0) == 1);
        daisyinv.SetActive(false);
        daisyinv2.SetActive(true);
    }


    void Awake()
    {
        //DontDestroyOnLoad(gameObject);
        
        
    }

    public void SwitchUI()
    {
        daisyinv2.SetActive(false);
        daisyinv.SetActive(true);
    }

}
