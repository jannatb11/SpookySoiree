using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    [Header("UI")]
    public GameObject dialoguePanel;
    public GameObject backgroundBox;
    public Text nameText;
    public Text dialogueText;

    [Header("Choice UI")]
    public GameObject choicePanel;
    public Button yesButton;
    public Button noButton;

    [Header("Typing Settings")]
    public float typingSpeed = 0.03f;

    [Header("Scene Arrows")]
    public GameObject sceneArrows;

    [Header("Intro Gate")]
    public bool isIntroDialogue;
    public int introLastLineIndex;

    [Header("Scene Transition")]
    public bool switchSceneOnEnd;
    public string sceneToLoad;

    private string[] lines;
    private string[] speakers;
    private int index;

    private bool hasChoices;
    private int choiceLineIndex;
    private int yesStart, yesEnd;
    private int noStart, noEnd;

    private bool inBranch;
    private int branchEnd;

    private bool lineFullyDisplayed;
    private Coroutine typingCoroutine;

    private NPCInteraction currentNPC;
    private ItemInteractionUI currentItem;

    public bool IsDialogueActive { get; private set; }

    void Awake()
    {
        Instance = this;

        yesButton.onClick.AddListener(YesChoice);
        noButton.onClick.AddListener(NoChoice);

        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false);

        if (backgroundBox != null)
            backgroundBox.SetActive(false);
    }

    void Update()
    {
        if (!IsDialogueActive)
            return;

        if (Input.GetMouseButtonDown(0))
        {
            // Don't allow click-to-continue if choices are visible
            if (choicePanel.activeSelf)
                return;

            if (!lineFullyDisplayed)
            {
                StopCoroutine(typingCoroutine);
                dialogueText.text = lines[index];
                lineFullyDisplayed = true;
            }
            else
            {
                NextLine();
            }
        }
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
        ItemInteractionUI item,
        NPCInteraction npc
    )
    {
        IsDialogueActive = true;

        if (sceneArrows != null)
            sceneArrows.SetActive(false);

        dialoguePanel.SetActive(true);

        if (backgroundBox != null)
            backgroundBox.SetActive(true);

        choicePanel.SetActive(false);

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

        currentNPC = npc;
        currentItem = item;

        ShowLine();
    }

    void ShowLine()
    {
        dialogueText.text = "";
        lineFullyDisplayed = false;

        if (speakers != null &&
            index < speakers.Length &&
            !string.IsNullOrEmpty(speakers[index]))
        {
            nameText.text = speakers[index];
        }

        typingCoroutine = StartCoroutine(TypeLine(lines[index]));
    }

    IEnumerator TypeLine(string line)
    {
        foreach (char letter in line)
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        lineFullyDisplayed = true;

        //  Show choices when we reach the choice line
        if (!inBranch && hasChoices && index == choiceLineIndex)
        {
            choicePanel.SetActive(true);
        }
    }

    void NextLine()
    {
        index++;

        // End branch
        if (inBranch && index > branchEnd)
        {
            EndDialogue();
            return;
        }

        if (index >= lines.Length)
        {
            EndDialogue();
            return;
        }

        ShowLine();
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
        choicePanel.SetActive(false); //  HIDE choice panel properly

        inBranch = true;
        branchEnd = end;
        index = start;

        ShowLine();
    }

    void EndDialogue()
    {
        dialoguePanel.SetActive(false);
        choicePanel.SetActive(false); //  Ensure hidden

        if (backgroundBox != null)
            backgroundBox.SetActive(false);

        IsDialogueActive = false;

        if (sceneArrows != null)
            sceneArrows.SetActive(true);

        //  Intro gate unlock
        if (isIntroDialogue && index >= introLastLineIndex)
        {
            DialogueGate.introFinished = true;
            Debug.Log("Intro finished — movement unlocked");
        }

        //  Scene switch (Gurt)
        if (switchSceneOnEnd && !string.IsNullOrEmpty(sceneToLoad))
        {
            GameProgress.talkedToGurt = true;
            SceneManager.LoadScene(sceneToLoad);
            return;
        }

        if (currentNPC != null)
        {
            currentNPC.RemoveNPC();
        }

        currentNPC = null;
        currentItem = null;
    }
}
