using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public Text nameText;
    public Text dialogueText;
    public Button nextButton;

    public GameObject choicePanel;
    public Button yesButton;
    public Button noButton;

    [Header("Dialogue State")]
    private string[] lines;
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
    }

    public void StartDialogue(
        string speaker,
        string[] dialogue,
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
        choicePanel.SetActive(false);
        nextButton.gameObject.SetActive(true);

        nameText.text = speaker;
        lines = dialogue;
        index = 0;

        hasChoices = _hasChoices;
        choiceLineIndex = _choiceLineIndex;

        yesStart = _yesStart;
        yesEnd = _yesEnd;
        noStart = _noStart;
        noEnd = _noEnd;

        inBranch = false;
        currentItem = item;

        dialogueText.text = lines[index];
    }

    public void NextLine()
    {
        // If currently in a branch
        if (inBranch)
        {
            index++;

            if (index > branchEnd)
            {
                EndDialogue();
                return;
            }

            dialogueText.text = lines[index];
            return;
        }

        // If at choice line
        if (hasChoices && index == choiceLineIndex)
        {
            dialogueText.text = lines[index];
            nextButton.gameObject.SetActive(false);
            choicePanel.SetActive(true);
            return;
        }

        // Normal line
        index++;

        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = lines[index];
    }

    private void YesChoice()
    {
        // Start Yes branch
        StartBranch(yesStart, yesEnd);

        // Collect the item if this dialogue is for an item
        if (currentItem != null)
        {
            currentItem.CollectItem();
        }
    }

    private void NoChoice()
    {
        // Start No branch
        StartBranch(noStart, noEnd);
    }

    private void StartBranch(int start, int end)
    {
        choicePanel.SetActive(false);
        nextButton.gameObject.SetActive(true);

        inBranch = true;
        branchEnd = end;
        index = start;

        dialogueText.text = lines[index];
    }

    private void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
        IsDialogueActive = false;
        currentItem = null;
    }
}
