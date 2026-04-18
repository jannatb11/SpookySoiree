using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    private GameObject daisyinv;
    private GameObject daisyinv2;
    private GameObject pianoinv;
    private GameObject mouseinv;

    private bool hasDaisy = false;
    private bool hasPiano = false;
    private bool hasMouse = false;

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

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        ReconnectUI();
        UpdateUI();
    }

    void ReconnectUI()
    {
        daisyinv = GameObject.Find("daisyinv");
        daisyinv2 = GameObject.Find("daisyinv2");
        pianoinv = GameObject.Find("pianoinv");
        mouseinv = GameObject.Find("mouseinv");

        if (daisyinv2 != null && !hasDaisy)
            daisyinv2.SetActive(true);

        if (daisyinv != null && !hasDaisy)
            daisyinv.SetActive(false);

        if (pianoinv != null && !hasPiano)
            pianoinv.SetActive(false);

        if (mouseinv != null && !hasMouse)
            mouseinv.SetActive(false);
    }

    public void CollectDaisy()
    {
        if (!hasDaisy)
        {
            hasDaisy = true;
            if (daisyinv2 != null) daisyinv2.SetActive(false);
            if (daisyinv != null) daisyinv.SetActive(true);
        }
    }

    public void CollectPiano()
    {
        if (!hasPiano)
        {
            hasPiano = true;
            if (pianoinv != null) pianoinv.SetActive(true);
        }
    }

    public void CollectMouse()
    {
        if (!hasMouse)
        {
            hasMouse = true;
            if (mouseinv != null) mouseinv.SetActive(true);
        }
    }

    void UpdateUI()
    {
        if (daisyinv != null) daisyinv.SetActive(hasDaisy);
        if (daisyinv2 != null) daisyinv2.SetActive(!hasDaisy);
        if (pianoinv != null) pianoinv.SetActive(hasPiano);
        if (mouseinv != null) mouseinv.SetActive(hasMouse);
    }
}