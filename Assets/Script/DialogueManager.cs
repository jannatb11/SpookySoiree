using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;
    public static bool DialogueActive;

    [Header("UI")]
    public GameObject dialoguePanel;
    public Text nameText;
    public Text dialogueText;
    public Button nextButton;

    public GameObject choicePanel;
    public Button choiceButton1;
    public Button choiceButton2;

    private string[] lines;
    private int index;

    private bool hasChoices;
    private int choiceLineIndex;

    private int yesStart, yesEnd;
    private int noStart, noEnd;

    private bool inBranch = false;
    private int branchEndIndex;

    private GameObject currentSource;
    private bool destroySourceOnYes;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DialogueActive = false;

        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);

        nextButton.onClick.AddListener(NextLine);
        choiceButton1.onClick.AddListener(ChoiceYes);
        choiceButton2.onClick.AddListener(ChoiceNo);
    }

    public void StartDialogue(
        string speakerName,
        string[] dialogue,
        bool _hasChoices,
        int _choiceLineIndex,
        int _yesStart,
        int _yesEnd,
        int _noStart,
        int _noEnd,
        GameObject sourceObject = null,
        bool destroyOnYes = false
    )
    {
        if (DialogueActive)
            return;

        DialogueActive = true;

        dialoguePanel.SetActive(true);
        choicePanel.SetActive(false);
        nextButton.gameObject.SetActive(true);

        nameText.text = speakerName;
        lines = dialogue;
        index = 0;

        hasChoices = _hasChoices;
        choiceLineIndex = _choiceLineIndex;

        yesStart = _yesStart;
        yesEnd = _yesEnd;
        noStart = _noStart;
        noEnd = _noEnd;

        inBranch = false;

        currentSource = sourceObject;
        destroySourceOnYes = destroyOnYes;

        dialogueText.text = lines[index];
    }

    public void NextLine()
    {
        index++;

        if (inBranch && index > branchEndIndex)
        {
            EndDialogue();
            return;
        }

        if (!inBranch && hasChoices && index == choiceLineIndex)
        {
            dialogueText.text = lines[index];
            ShowChoices();
            return;
        }

        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }

        dialogueText.text = lines[index];
    }

    void ShowChoices()
    {
        nextButton.gameObject.SetActive(false);
        choicePanel.SetActive(true);
    }

    void ChoiceYes()
    {
        StartBranch(yesStart, yesEnd);

        if (destroySourceOnYes && currentSource != null)
        {
            ItemInteraction item = currentSource.GetComponent<ItemInteraction>();
            if (item != null)
            {
                item.MarkAsCollected(); 
            }

            Destroy(currentSource);
            currentSource = null;
        }
    }

    void ChoiceNo()
    {
        StartBranch(noStart, noEnd);
    }

    void StartBranch(int start, int end)
    {
        if (start < 0 || start >= lines.Length || end < start)
        {
            EndDialogue();
            return;
        }

        choicePanel.SetActive(false);
        nextButton.gameObject.SetActive(true);

        inBranch = true;
        branchEndIndex = end;

        index = start;
        dialogueText.text = lines[index];
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);
        nextButton.gameObject.SetActive(true);

        inBranch = false;
        currentSource = null;
        destroySourceOnYes = false;

        DialogueActive = false;
    }
}
