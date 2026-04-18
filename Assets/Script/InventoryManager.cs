using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private GameObject daisyinv;
    private GameObject daisyinv2;
    private GameObject pianoinv;
    private GameObject mouseinv;

    private bool hasDaisy;
    private bool hasPiano;
    private bool hasMouse;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SetupUI());
    }

    IEnumerator SetupUI()
    {
        yield return null;

        FindUI();
        RefreshUI();
    }

    void FindUI()
    {
        daisyinv = GameObject.Find("daisyinv");
        daisyinv2 = GameObject.Find("daisyinv2");
        pianoinv = GameObject.Find("pianoinv");
        mouseinv = GameObject.Find("mouseinv");
    }


    void RefreshUI()
    {
        if (daisyinv != null) daisyinv.SetActive(hasDaisy);
        if (daisyinv2 != null) daisyinv2.SetActive(!hasDaisy);

        if (pianoinv != null) pianoinv.SetActive(hasPiano);
        if (mouseinv != null) mouseinv.SetActive(hasMouse);
    }


    public void CollectDaisy()
    {
        hasDaisy = true;
        RefreshUI();
    }

    public void CollectPiano()
    {
        hasPiano = true;
        RefreshUI();
    }

    public void CollectMouse()
    {
        hasMouse = true;
        RefreshUI();
    }
}