using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventory UI")]
    private GameObject inventoryBar;

    private GameObject daisyinv;
    private GameObject daisyinv2;
    private GameObject pianoinv;
    private GameObject mouseinv;

    [Header("Inventory State")]
    private bool hasDaisy;
    private bool hasPiano;
    private bool hasMouse;

    private bool inventoryOpened = false;

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

    void Start()
    {
        // DISABLE INVENTORY ON GAME START
        inventoryBar = GameObject.Find("inventorymanager");

        if (inventoryBar)
            inventoryBar.SetActive(false);
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
        SetupButtons();

        // KEEP INVENTORY DISABLED IN EVERY SCENE
        if (!inventoryOpened && inventoryBar)
            inventoryBar.SetActive(false);

        RefreshUI();
    }

    void FindUI()
    {
        inventoryBar = GameObject.Find("inventorymanager");

        daisyinv = GameObject.Find("daisyinv");
        daisyinv2 = GameObject.Find("daisyinv2");
        pianoinv = GameObject.Find("pianoinv");
        mouseinv = GameObject.Find("mouseinv");
    }

    void SetupButtons()
    {
        // OPEN INVENTORY BUTTON
        if (daisyinv2)
        {
            Button openBtn = daisyinv2.GetComponent<Button>();

            if (openBtn != null)
            {
                openBtn.onClick.RemoveAllListeners();
                openBtn.onClick.AddListener(OpenInventory);
            }
        }

        // DAISY BUTTON
        if (daisyinv)
        {
            Button daisyBtn = daisyinv.GetComponent<Button>();

            if (daisyBtn != null)
            {
                daisyBtn.onClick.RemoveAllListeners();
                daisyBtn.onClick.AddListener(TalkToDaisy);
            }
        }
    }

    void RefreshUI()
    {
        if (inventoryBar)
            inventoryBar.SetActive(inventoryOpened);

        if (!inventoryOpened)
            return;

        if (daisyinv)
            daisyinv.SetActive(hasDaisy);

        if (pianoinv)
            pianoinv.SetActive(hasPiano);

        if (mouseinv)
            mouseinv.SetActive(hasMouse);
    }

    void OpenInventory()
    {
        inventoryOpened = true;
        RefreshUI();
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

    public void TalkToDaisy()
    {
        Debug.Log("Talk to Daisy");
    }
}