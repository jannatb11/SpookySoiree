using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public System.Action OnDialogueFinished;


    public GameObject dialoguePanel;
    public GameObject dialogueBackground;

    public Text nameText;
    public Text dialogueText;
    public Button nextButton;

    public GameObject choicePanel;
    public Button yesButton;
    public Button noButton;

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
        dialogueBackground.SetActive(false);
    }

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
        dialogueBackground.SetActive(true);
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

        ShowLine(defaultSpeaker);
    }

    void ShowLine(string fallbackSpeaker)
    {
        dialogueText.text = lines[index];

        if (speakers != null && index < speakers.Length && !string.IsNullOrEmpty(speakers[index]))
            nameText.text = speakers[index];
        else
            nameText.text = fallbackSpeaker;
    }

    public void NextLine()
    {
        index++;

        if (inBranch && index > branchEnd)
        {
            EndDialogue();
            return;
        }

        if (!inBranch && hasChoices && index == choiceLineIndex)
        {
            ShowLine(nameText.text);
            nextButton.gameObject.SetActive(false);
            choicePanel.SetActive(true);
            return;
        }

        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }

        ShowLine(nameText.text);
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

        ShowLine(nameText.text);
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueBackground.SetActive(false);
        choicePanel.SetActive(false);

        IsDialogueActive = false;

        // Unlock movement after intro dialogue
        DialogueGate.introFinished = true;

        currentItem = null;
    }


}

