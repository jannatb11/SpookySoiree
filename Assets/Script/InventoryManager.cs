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
        daisyinv = GameObject.Find("daisyinv");
        daisyinv2 = GameObject.Find("daisyinv2");
        pianoinv = GameObject.Find("pianoinv");
        mouseinv = GameObject.Find("mouseinv");

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
        if (daisyinv == null || daisyinv2 == null || pianoinv == null || mouseinv == null)
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

        StartCoroutine(DelayedRefresh());
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

        if (GameState.allDoorNPCsTalkedTo)
            currentSet = afterAllNPCs;
        else if (GameState.openedDoor)
            currentSet = afterDoor;
        else
            currentSet = beforeDoor;

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
}