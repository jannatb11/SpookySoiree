using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI References")]
    public GameObject dialoguePanel;
    public GameObject dialogueBackground;   // BLACK BOX
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
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        nextButton.onClick.AddListener(NextLine);
        yesButton.onClick.AddListener(YesChoice);
        noButton.onClick.AddListener(NoChoice);

        dialoguePanel.SetActive(false);
        dialogueBackground.SetActive(false);
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
        dialogueBackground.SetActive(true);
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
        // BRANCH MODE
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

        // CHOICE LINE
        if (hasChoices && index == choiceLineIndex)
        {
            dialogueText.text = lines[index];
            nextButton.gameObject.SetActive(false);
            choicePanel.SetActive(true);
            return;
        }

        // NORMAL FLOW
        index++;

        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = lines[index];
    }

    void YesChoice()
    {
        StartBranch(yesStart, yesEnd);

        // Only items get collected
        if (currentItem != null)
        {
            currentItem.CollectItem();
        }
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

        dialogueText.text = lines[index];
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        dialogueBackground.SetActive(false);
        choicePanel.SetActive(false);

        IsDialogueActive = false;
        currentItem = null;
    }
}
