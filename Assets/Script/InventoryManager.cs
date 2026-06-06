using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    [Header("Inventory UI")]
    private GameObject daisyinv;
    private GameObject daisyinv2;
    private GameObject pianoinv;
    private GameObject mouseinv;

    [Header("Inventory State")]
    private bool hasDaisy;
    private bool hasPiano;
    private bool hasMouse;

    [Header("Daisy FIRST Dialogue")]
    public string[] daisyDialogueLines;
    public bool[] daisyIsNPCSpeaking;
    public AudioClip[] daisyVoiceClips;
    public string[] daisySpeakerNames;

    [Header("Extra Daisy Dialogue Sets")]
    public DaisyDialogueSet afterGurtCutscene;
    public DaisyDialogueSet afterSarahAct2;

    public bool HasAnyItem()
    {
        return hasDaisy || hasPiano || hasMouse;
    }

    [System.Serializable]
    public class DaisyConversation
    {
        public string[] lines;
        public bool[] isNPCSpeaking;
        public AudioClip[] voiceClips;
        public string[] speakerNames;
    }

    [System.Serializable]
    public class DaisyDialogueSet
    {
        public string stateName;
        public DaisyConversation[] conversations;
    }

    [Header("Daisy Dialogue Sets")]
    public DaisyDialogueSet beforeDoor;
    public DaisyDialogueSet afterDoor;
    public DaisyDialogueSet afterAllNPCs;

    private bool hasTalkedToDaisy = false;

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            string existingScene = Instance.gameObject.scene.name;

            if (existingScene == "DontDestroyOnLoad")
            {
                Debug.Log("Replacing old InventoryManager with scene version.");

                Destroy(Instance.gameObject);

                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
                return;
            }
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(SetupUI());

        if (GameState.resetInventoryOnNextScene)
        {
            ClearInventory();
            GameState.resetInventoryOnNextScene = false;
        }
    }


    IEnumerator SetupUI()
    {
        yield return null;
        yield return null;

        FindUI();
        SetupButtons();
        RefreshUI();
    }
    IEnumerator DelayedRefresh()
    {
        yield return null;
        yield return null;

        RefreshUI();
    }

    void FindUI()
    {
        GameObject[] allObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        foreach (GameObject obj in allObjects)
        {
            if (obj.name == "daisyinv")
                daisyinv = obj;

            if (obj.name == "daisyinv2")
                daisyinv2 = obj;

            if (obj.name == "pianoinv")
                pianoinv = obj;

            if (obj.name == "mouseinv")
                mouseinv = obj;
        }

        Debug.Log($"UI FOUND -> daisyinv:{daisyinv}, daisyinv2:{daisyinv2}");
    }

    void SetupButtons()
    {
        if (daisyinv)
        {
            Button btn = daisyinv.GetComponent<Button>();

            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(TalkToDaisy);
            }
        }

        if (daisyinv2)
        {
            Button btn = daisyinv2.GetComponent<Button>();

            if (btn != null)
            {
                btn.onClick.RemoveAllListeners();
                btn.onClick.AddListener(CollectDaisy);
            }
        }
    }

    void RefreshUI()
    {
        if (daisyinv == null || daisyinv2 == null ||
            pianoinv == null || mouseinv == null)
        {
            FindUI();
        }

        Debug.Log("hasDaisy = " + hasDaisy);

        if (daisyinv != null)
            daisyinv.SetActive(hasDaisy);

        if (daisyinv2 != null)
            daisyinv2.SetActive(!hasDaisy);

        if (pianoinv != null)
            pianoinv.SetActive(hasPiano);

        if (mouseinv != null)
            mouseinv.SetActive(hasMouse);
    }
    public void CollectDaisy()
    {
        hasDaisy = true;

        Debug.Log("Collected Daisy");

        GameObject daisyUI = GameObject.Find("daisyinv");

        if (daisyUI != null)
        {
            daisyUI.SetActive(true);
            Debug.Log("Forced daisyinv visible");
        }
        else
        {
            Debug.LogError("Could not find daisyinv!");
        }

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

    public void ClearInventory()
    {
        hasDaisy = false;
        hasPiano = false;
        hasMouse = false;

        RefreshUI();
    }

    public void TalkToDaisy()
    {
        if (!hasDaisy) return;

        if (DialogueManager.Instance == null) return;
        if (DialogueManager.Instance.IsDialogueActive) return;

        if (!hasTalkedToDaisy)
        {
            hasTalkedToDaisy = true;

            DialogueManager.Instance.StartDialogue(
                "Daisy",
                daisyDialogueLines,
                daisySpeakerNames,
                daisyIsNPCSpeaking,
                false,
                0, 0, 0, 0, 0,
                null,
                null,
                daisyVoiceClips,
                null
            );
            return;
        }

        DaisyDialogueSet currentSet;

       
        if (GameState.triggeredIDs.Contains("Sarah_Act2_1"))
        {
            currentSet = afterSarahAct2;
        }
        else if (GameState.triggeredIDs.Contains("Gurt_Cutscene"))
        {
            currentSet = afterGurtCutscene;
        }
        else if (GameState.allDoorNPCsTalkedTo)
        {
            currentSet = afterAllNPCs;
        }
        else if (GameState.openedDoor)
        {
            currentSet = afterDoor;
        }
        else
        {
            currentSet = beforeDoor;
        }

        if (currentSet == null || currentSet.conversations.Length == 0)
            return;

        int rand = Random.Range(0, currentSet.conversations.Length);
        DaisyConversation convo = currentSet.conversations[rand];

        if (convo.lines == null || convo.lines.Length == 0)
            return;

        DialogueManager.Instance.StartDialogue(
            "Daisy",
            convo.lines,
            convo.speakerNames,
            convo.isNPCSpeaking,
            false,
            0, 0, 0, 0, 0,
            null,
            null,
            convo.voiceClips,
            null
        );
    }

    public bool HasDaisy()
    {
        return hasDaisy;
    }

    public bool HasPiano()
    {
        return hasPiano;
    }

    public bool HasMouse()
    {
        return hasMouse;
    }

    IEnumerator LateRefresh()
    {
        yield return new WaitForSeconds(0.1f);
        RefreshUI();
    }
}