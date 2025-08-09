using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    [Header("UI refs")]
    [SerializeField]
    private Image portraitImage;

    [SerializeField]
    private TMP_Text nameText;

    [SerializeField]
    private TMP_Text dialogueText;

    [Header("Choices UI")]
    [SerializeField]
    private Transform choicesRoot; // Panel chứa các nút lựa chọn

    [SerializeField]
    private Button choiceButtonPrefab; // Prefab 1 nút có TMP_Text con

    [Header("Next")]
    [SerializeField]
    private GameObject nextButton; // GameObject chứa nút "Tiếp theo"

    [SerializeField]
    private Button nextButtonComponent; // (tuỳ) gán trực tiếp Button nếu muốn

    [Header("Typewriter Settings")]
    [SerializeField]
    private float typeSpeed = 0.03f; // thời gian giữa mỗi ký tự

    [SerializeField]
    private bool allowNextOnChoice = false; // bấm Next ở Choice sẽ tự chọn option 1

    // State
    public bool IsActive { get; private set; }

    // Sự kiện cho GameManager bám
    public Action OnDialogueStarted;
    public Action OnDialogueEnded;

    private DialogueSequenceSO seq;
    private DialogueEntry current;

    // Typewriter control
    private Coroutine typingCoroutine;
    private bool isTyping;
    private string fullLineText;

    private void Awake()
    {
        // Tự gán onClick cho nút Next nếu có
        if (nextButtonComponent == null && nextButton != null)
            nextButtonComponent = nextButton.GetComponentInChildren<Button>(true);

        if (nextButtonComponent != null)
        {
            nextButtonComponent.onClick.RemoveAllListeners();
            nextButtonComponent.onClick.AddListener(OnClickNext);
        }

        if (portraitImage != null)
            portraitImage.gameObject.SetActive(false);
    }

    // Bắt đầu theo asset (từ startId)
    public void StartDialogue(DialogueSequenceSO sequence) => StartDialogueAtId(sequence, null);

    // Bắt đầu theo id cụ thể (null/empty -> startId)
    public void StartDialogueAtId(DialogueSequenceSO sequence, string entryId)
    {
        if (sequence == null)
            return;
        seq = sequence;
        seq.BuildIndex();

        if (string.IsNullOrEmpty(entryId))
            entryId = seq.startId;

        current = seq.GetById(entryId.Trim());
        IsActive = true;
        OnDialogueStarted?.Invoke();
        ShowCurrent();
    }

    // Nhảy tới id trong sequence hiện tại
    public void JumpTo(string entryId)
    {
        if (seq == null)
            return;
        if (string.IsNullOrEmpty(entryId))
            entryId = seq.startId;
        current = seq.GetById(entryId.Trim());
        ShowCurrent();
    }

    // Next/Skip
    public void OnClickNext()
    {
        if (!IsActive || seq == null || current == null)
            return;

        if (isTyping)
        {
            StopTypingAndShowFull();
            return;
        }

        if (current.type == EntryType.Choice)
        {
            if (!allowNextOnChoice)
                return;
            var list = current.choice?.options;
            if (list != null && list.Count > 0)
            {
                var opt = list[0];
                if (string.IsNullOrEmpty(opt.nextId))
                {
                    EndDialogue();
                    return;
                }
                current = seq.GetById(opt.nextId.Trim());
                ShowCurrent();
            }
            return;
        }

        if (current.type != EntryType.Line)
            return;

        string nextId = current.line.nextId?.Trim();

        // Nếu nextId rỗng -> kết thúc luôn
        if (string.IsNullOrEmpty(nextId))
        {
            EndDialogue();
            return;
        }

        current = seq.GetById(nextId);
        if (current == null)
        {
            EndDialogue();
            return;
        }
        ShowCurrent();
    }

    private void ShowCurrent()
    {
        ClearChoices();

        if (current.type == EntryType.Line)
        {
            var line = current.line;

            if (line.speaker != null)
            {
                nameText.text = line.speaker.DisplayName;
                nameText.color = line.speaker.NameColor;

                if (line.speaker.DefaultIcon != null)
                {
                    portraitImage.sprite = line.speaker.DefaultIcon;
                    portraitImage.gameObject.SetActive(true);
                }
                else
                {
                    portraitImage.gameObject.SetActive(false);
                }
            }
            else
            {
                nameText.text = "";
                portraitImage.gameObject.SetActive(false);
            }

            fullLineText = line.text ?? "";
            StartTyping(fullLineText);

            if (nextButton != null)
                nextButton.SetActive(true);
            if (nextButtonComponent != null)
                nextButtonComponent.interactable = true;
        }
        else if (current.type == EntryType.Choice)
        {
            if (nextButton != null)
                nextButton.SetActive(false);
            RenderChoices();
        }
    }

    private void StartTyping(string textToType)
    {
        if (typingCoroutine != null)
            StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeTextCoroutine(textToType));
    }

    private IEnumerator TypeTextCoroutine(string textToType)
    {
        isTyping = true;
        dialogueText.text = "";

        foreach (char c in textToType)
        {
            dialogueText.text += c;
            yield return new WaitForSeconds(typeSpeed);
        }

        isTyping = false;
        typingCoroutine = null;
    }

    private void StopTypingAndShowFull()
    {
        if (typingCoroutine != null)
        {
            StopCoroutine(typingCoroutine);
            typingCoroutine = null;
        }
        dialogueText.text = fullLineText;
        isTyping = false;
    }

    private void RenderChoices()
    {
        if (
            current.choice == null
            || current.choice.options == null
            || current.choice.options.Count == 0
        )
        {
            EndDialogue();
            return;
        }

        foreach (var opt in current.choice.options)
        {
            var btn = Instantiate(choiceButtonPrefab, choicesRoot);
            var label = btn.GetComponentInChildren<TMP_Text>();
            if (label != null)
                label.text = opt.text;

            btn.onClick.AddListener(() =>
            {
                string jump = opt.nextId?.Trim();
                current = !string.IsNullOrEmpty(jump)
                    ? seq.GetById(jump)
                    : seq.GetNextLinear(current);
                ShowCurrent();
            });
        }
    }

    private void ClearChoices()
    {
        if (choicesRoot == null)
            return;
        for (int i = choicesRoot.childCount - 1; i >= 0; i--)
            Destroy(choicesRoot.GetChild(i).gameObject);
    }

    private void EndDialogue()
    {
        ClearChoices();
        if (nextButton != null)
            nextButton.SetActive(false);
        IsActive = false;
        OnDialogueEnded?.Invoke();
        Debug.Log("Dialogue ended.");
    }
}
