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

    // ========================
    //  NEW CONVERSATION SYSTEM
    // ========================

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
    private bool buttonsSetup = false;

    // ========================
    // UNITY SETUP
    // ========================

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

        if (!buttonsSetup)
        {
            SetupButtons();
            buttonsSetup = true;
        }
    }

    void FindUI()
    {
        daisyinv = GameObject.Find("daisyinv");
        daisyinv2 = GameObject.Find("daisyinv2");
        pianoinv = GameObject.Find("pianoinv");
        mouseinv = GameObject.Find("mouseinv");
    }

    void SetupButtons()
    {
        if (daisyinv)
        {
            Button btn = daisyinv.GetComponent<Button>();

            if (btn != null)
            {
                btn.onClick.RemoveListener(TalkToDaisy);
                btn.onClick.AddListener(TalkToDaisy);
            }
        }
    }

    void RefreshUI()
    {
        if (daisyinv)
            daisyinv.SetActive(hasDaisy);

        if (daisyinv2)
            daisyinv2.SetActive(!hasDaisy);

        if (pianoinv)
            pianoinv.SetActive(hasPiano);

        if (mouseinv)
            mouseinv.SetActive(hasMouse);
    }

    // ========================
    // COLLECT ITEMS
    // ========================

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

    // ========================
    //  DAISY INTERACTION
    // ========================

    public void TalkToDaisy()
    {
        if (!hasDaisy) return;

        if (DialogueManager.Instance == null) return;
        if (DialogueManager.Instance.IsDialogueActive) return;

        // ========================
        // FIRST TIME
        // ========================
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

        // ========================
        // PICK STATE
        // ========================

        DaisyDialogueSet currentSet;

        if (GameState.allDoorNPCsTalkedTo)
            currentSet = afterAllNPCs;
        else if (GameState.openedDoor)
            currentSet = afterDoor;
        else
            currentSet = beforeDoor;

        if (currentSet == null || currentSet.conversations.Length == 0)
            return;

        // ========================
        // RANDOM CONVERSATION
        // ========================

        int rand = Random.Range(0, currentSet.conversations.Length);
        DaisyConversation convo = currentSet.conversations[rand];

        if (convo.lines == null || convo.lines.Length == 0)
            return;

        // ========================
        // START CONVERSATION
        // ========================

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