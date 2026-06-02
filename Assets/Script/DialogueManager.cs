using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public TMP_Text dialogueText;
    public TMP_Text speakerText;
    public GameObject choicePanel;
    public Button yesButton;
    public Button noButton;

    [Header("Room Navigation UI (ARROWS)")]
    public GameObject roomNavigationUI;

    [Header("Names")]
    public string playerName = "Player";

    [Header("Typing")]
    public float typingSpeed = 0.03f;

    [Header("Audio")]
    public AudioSource audioSource;

    private string[] lines;
    private string[] speakerNames;
    private bool[] isNPCSpeaking;
    private AudioClip[] voiceClips;
    private string[] animationStates;

    private int index;
    private bool isTyping;
    private bool canContinue;
    private bool isDialogueActive;

    private bool hasChoices;
    private int choiceLineIndex;
    private int yesJumpToLine;
    private int noJumpToLine;

    private NPCInteraction currentNPC;
    private ItemInteractionUI currentItem;

    private Coroutine typingCoroutine;

    public bool IsDialogueActive => isDialogueActive;

    [System.Serializable]
    public class CharacterColor
    {
        public string characterName;
        public Color textColor;
    }

    [Header("Character Colors")]
    public CharacterColor[] characterColors;
    public Color defaultColor = Color.white;

    void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    void Update()
    {
        if (!isDialogueActive) return;

        if (Input.GetMouseButtonDown(0))
        {
            if (isTyping)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = lines[index];
                isTyping = false;
                canContinue = true;

                if (audioSource != null)
                    audioSource.Stop();
            }
            else if (canContinue && !choicePanel.activeSelf)
            {
                NextLine();
            }
        }
    }

    public void StartDialogue(
        string npcName,
        string[] dialogueLines,
        string[] speakerNames,
        bool[] isNPCSpeaking,
        bool hasChoices,
        int choiceLineIndex,
        int yesJumpToLine,
        int yesEndLine,
        int noJumpToLine,
        int noEndLine,
        ItemInteractionUI itemReference,
        NPCInteraction npcReference,
        AudioClip[] voiceClips,
        string[] animationStates
    )
    {
        if (isDialogueActive)
            return;

        this.lines = dialogueLines;
        this.speakerNames = speakerNames;
        this.isNPCSpeaking = isNPCSpeaking;
        this.voiceClips = voiceClips;
        this.animationStates = animationStates;

        this.hasChoices = hasChoices;
        this.choiceLineIndex = choiceLineIndex;
        this.yesJumpToLine = yesJumpToLine;
        this.noJumpToLine = noJumpToLine;

        currentNPC = npcReference;
        currentItem = itemReference;

        index = 0;
        isDialogueActive = true;

        dialoguePanel.SetActive(true);
        choicePanel.SetActive(false);

        DisableRoomUI();  

        StartTyping();
    }

    void StartTyping()
    {
        if (currentNPC != null)
        {
            currentNPC.CheckDialogueEvents(index);
        }

        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        canContinue = false;
        dialogueText.text = "";

        string speaker = "Unknown";

        if (speakerNames != null && index < speakerNames.Length)
            speaker = speakerNames[index];

        speakerText.text = speaker;
        dialogueText.color = GetColorForSpeaker(speaker);

        if (currentNPC != null && currentNPC.animator != null)
        {
            if (animationStates != null && index < animationStates.Length)
            {
                currentNPC.animator.Play(animationStates[index]);
            }
        }

        if (audioSource != null &&
            voiceClips != null &&
            index < voiceClips.Length &&
            voiceClips[index] != null)
        {
            audioSource.Stop();
            audioSource.clip = voiceClips[index];
            audioSource.Play();
        }
       
        if (currentNPC != null)
        {
            currentNPC.CheckDialogueEvents(index);
        }

        foreach (char c in lines[index])
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        canContinue = true;

        if (hasChoices && index == choiceLineIndex)
            ShowChoice();
    }

    void NextLine()
    {
        if (index < lines.Length - 1)
        {
            index++;
            StartTyping();
        }
        else
        {
            EndDialogue();
        }
    }

    void ShowChoice()
    {
        canContinue = false;
        choicePanel.SetActive(true);

        dialogueText.text += "\n\n> Yes\n> No";

        yesButton.onClick.RemoveAllListeners();
        noButton.onClick.RemoveAllListeners();

        yesButton.onClick.AddListener(() =>
        {
            choicePanel.SetActive(false);
            index = yesJumpToLine - 1;
            NextLine();
        });

        noButton.onClick.AddListener(() =>
        {
            choicePanel.SetActive(false);
            index = noJumpToLine - 1;
            NextLine();
        });
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
        isDialogueActive = false;

        if (audioSource != null)
            audioSource.Stop();

        if (currentNPC != null)
        {
            currentNPC.OnDialogueComplete();
            currentNPC = null;
        }

        if (currentItem != null)
        {
            currentItem.OnDialogueComplete();
            currentItem = null;
        }

        EnableRoomUI();   
    }

    Color GetColorForSpeaker(string speaker)
    {
        if (characterColors != null)
        {
            foreach (var c in characterColors)
            {
                if (c.characterName == speaker)
                    return c.textColor;
            }
        }

        return defaultColor;
    }

    // =========================
    // ROOM UI CONTROL
    // =========================

    void DisableRoomUI()
    {
        if (roomNavigationUI != null)
            roomNavigationUI.SetActive(false);
    }

    void EnableRoomUI()
    {
        if (roomNavigationUI != null)
            roomNavigationUI.SetActive(true);
    }


}