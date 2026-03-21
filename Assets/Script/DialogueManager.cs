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


    [System.Serializable]
    public class CharacterColor
    {
        public string characterName;
        public Color textColor;
    }

    [Header("Character Colors")]
    public CharacterColor[] characterColors;
    public Color defaultColor = Color.white;

    [Header("Text Colors")]
    public Color npcTextColor = Color.yellow;
    public Color playerTextColor = Color.white;
    public string playerName = "Player"; // MUST match speaker name

    [Header("UI To Disable During Dialogue")]
    public GameObject[] uiToHide;
    public Button[] buttonsToDisable;


    [Header("Typing")]
    public float typingSpeed = 0.03f;

    private string[] lines;
    private string[] speakerNames;

    private int index;
    private bool isTyping;
    private bool canContinue;
    private bool isDialogueActive;
    private bool hasChoices;
    private int choiceLineIndex;

    private int yesJumpToLine;
    private int noJumpToLine;

    private NPCInteraction currentNPC;

    private Coroutine typingCoroutine;

    public bool IsDialogueActive => isDialogueActive;

    

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
    bool hasChoices,
    int choiceLineIndex,
    int yesJumpToLine,
    int yesEndLine,
    int noJumpToLine,
    int noEndLine,
    ItemInteractionUI itemReference,
    NPCInteraction npcReference
)
    {
        //  NEW SAFETY CHECK
        if (isDialogueActive)
            return;

        this.lines = dialogueLines;
        this.speakerNames = speakerNames;
        this.hasChoices = hasChoices;
        this.choiceLineIndex = choiceLineIndex;
        this.yesJumpToLine = yesJumpToLine;
        this.noJumpToLine = noJumpToLine;

        currentNPC = npcReference;

        index = 0;
        isDialogueActive = true;

        dialoguePanel.SetActive(true);
        choicePanel.SetActive(false);

        StartTyping();

        // Hide UI objects
        foreach (GameObject obj in uiToHide)
        {
            obj.SetActive(false);
        }

        // Disable buttons
        foreach (Button btn in buttonsToDisable)
        {
            btn.interactable = false;
        }
    }

    void StartTyping()
    {
        typingCoroutine = StartCoroutine(TypeLine());
    }

    IEnumerator TypeLine()
    {
        isTyping = true;
        canContinue = false;
        dialogueText.text = "";

        if (speakerNames != null && speakerNames.Length > index)
        {
            string speaker = speakerNames[index];
            speakerText.text = speaker;

            //  APPLY CHARACTER COLOR
            dialogueText.color = GetColorForSpeaker(speaker);
        }

        foreach (char letter in lines[index])
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }

        isTyping = false;
        canContinue = true;

        if (hasChoices && index == choiceLineIndex)
        {
            ShowChoice();
        }
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

        if (currentNPC != null)
        {
            currentNPC.OnDialogueComplete();
            currentNPC = null;
        }

        // Show UI objects again
        foreach (GameObject obj in uiToHide)
        {
            obj.SetActive(true);
        }

        // Re-enable buttons
        foreach (Button btn in buttonsToDisable)
        {
            btn.interactable = true;
        }
    }

    Color GetColorForSpeaker(string speaker)
    {
        foreach (var c in characterColors)
        {
            if (c.characterName == speaker)
                return c.textColor;
        }

        return defaultColor;
    }
}
