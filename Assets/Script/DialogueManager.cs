using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public GameObject dialoguePanel;
    public GameObject backgroundBox;
    public Text nameText;
    public Text dialogueText;
    public Button nextButton;

    public SceneSwitcher sceneSwitcher;
    
    public GameObject choicePanel;
    public Button yesButton;
    public Button noButton;

    [Header("Intro Gate")]
    public bool isIntroDialogue;
    public int introLastLineIndex;

    private string[] lines;
    private string[] speakers;
    private int index;

    private bool hasChoices;
    private int choiceLineIndex;

    private int yesStart, yesEnd;
    private int noStart, noEnd;

    private bool inBranch;
    private int branchEnd;

    private ItemInteractionUI currentItem;

    public bool IsDialogueActive { get; private set; }

    void Awake()
    {
        Instance = this;

        nextButton.onClick.AddListener(NextLine);
        yesButton.onClick.AddListener(YesChoice);
        noButton.onClick.AddListener(NoChoice);

        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
        if (backgroundBox != null)
            backgroundBox.SetActive(false);
    }

    //  THIS MATCHES YOUR NPCInteraction CALL
    public void StartDialogue(
        string defaultSpeaker,
        string[] dialogue,
        string[] speakerNames,
        bool _hasChoices,
        int _choiceLineIndex,
        int _yesStart,
        int _yesEnd,
        int _noStart,
        int _noEnd,
        ItemInteractionUI item
    )
    {
        IsDialogueActive = true;

        dialoguePanel.SetActive(true);
        if (backgroundBox != null)
            backgroundBox.SetActive(true);

        choicePanel.SetActive(false);
        nextButton.gameObject.SetActive(true);

        lines = dialogue;
        speakers = speakerNames;
        index = 0;

        hasChoices = _hasChoices;
        choiceLineIndex = _choiceLineIndex;

        yesStart = _yesStart;
        yesEnd = _yesEnd;
        noStart = _noStart;
        noEnd = _noEnd;

        inBranch = false;
        currentItem = item;

        UpdateLine();
    }

    void NextLine()
    {
        index++;

        if (inBranch && index > branchEnd)
        {
            EndDialogue();
            return;
        }

        if (!inBranch && hasChoices && index == choiceLineIndex)
        {
            UpdateLine();
            nextButton.gameObject.SetActive(false);
            choicePanel.SetActive(true);
            return;
        }

        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }

        UpdateLine();
    }

    void UpdateLine()
    {
        dialogueText.text = lines[index];

        if (speakers != null && index < speakers.Length && !string.IsNullOrEmpty(speakers[index]))
            nameText.text = speakers[index];
    }

    void YesChoice()
    {
        StartBranch(yesStart, yesEnd);

        if (currentItem != null)
            currentItem.CollectItem();
    }

    void NoChoice()
    {
        StartBranch(noStart, noEnd);
    }

    void StartBranch(int start, int end)
    {
        choicePanel.SetActive(false);
        nextButton.gameObject.SetActive(true);

        inBranch = true;
        branchEnd = end;
        index = start;

        UpdateLine();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
        if (backgroundBox != null)
            backgroundBox.SetActive(false);

        IsDialogueActive = false;
        currentItem = null;

        if (isIntroDialogue)
        {
            DialogueGate.introFinished = true;
            Debug.Log("Intro finished — movement unlocked");
        }

        if (sceneSwitcher != null)
        {
            sceneSwitcher.TriggerSceneSwitch();
        }
    }
}
