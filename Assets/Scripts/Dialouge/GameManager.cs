using UnityEngine;

public class GameManager : MonoBehaviour
{
    [Header("Dialogue")]
    [SerializeField]
    private DialogueManager dialogueManager; // gán trong Inspector

    [SerializeField]
    private DialogueSequenceSO sequence; // asset CH1_RungQuyDi...

    [SerializeField]
    private GameObject dialoguePanel; // panel tổng của UI thoại (Canvas/Panel)

    [SerializeField]
    private string startEntryId = ""; // để trống = dùng startId của asset

    private void Awake()
    {
        // Ẩn UI ngay từ đầu
        if (dialoguePanel != null)
            dialoguePanel.SetActive(false);

        // Subcribe event để bật/tắt UI đúng lúc
        if (dialogueManager != null)
        {
            dialogueManager.OnDialogueStarted += () =>
            {
                if (dialoguePanel != null)
                    dialoguePanel.SetActive(true);
            };
            dialogueManager.OnDialogueEnded += () =>
            {
                if (dialoguePanel != null)
                    dialoguePanel.SetActive(false);
            };
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (dialogueManager == null)
                return;

            if (!dialogueManager.IsActive)
            {
                // Chưa có hội thoại -> bắt đầu
                if (sequence == null)
                {
                    Debug.LogWarning("[GM] Chưa gán DialogueSequenceSO.");
                    return;
                }
                var id = string.IsNullOrWhiteSpace(startEntryId) ? null : startEntryId.Trim();
                dialogueManager.StartDialogueAtId(sequence, id);
            }
            else
            {
                // Đang có hội thoại -> tiến như nút Next
                dialogueManager.OnClickNext();
            }
        }
    }
}
