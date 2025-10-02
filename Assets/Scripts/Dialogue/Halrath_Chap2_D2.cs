using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Halrath_Chap2_D2 : MonoBehaviour
{
    //public GameObject canvas;
    //public CanvasGroup canvasGroup;
    //public Button choice1;
    //public Button choice2;
    //public TMP_Text text1;
    //public TMP_Text text2;
    public GameObject F;

    public GameObject dialogueBox;
    private bool isDialogueActive = false;

    //public GameObject choicePanel;
    //public RectTransform _choicePanel;

    private PlayerStatsManager playerStatsManager;
    private PlayerController playerController;
    public DialogueBlendShapeController z;
    public DialogueBlendShapeController h;

    //public GameObject fighting;

    public CameraSetting cameraSetting;
    public Animator zino;
    public Animator halrath;
    private CreateCharacterText createCharacterText;



    //public CanvasShaking cv_Shaking;
    //public CharacterShaking char_Shaking;
    //public BackgroundChangeSystem bg;

    //public void ChoicePanelAnimation()
    //{
    //    choicePanel.SetActive(true);
    //    _choicePanel.DOMoveX(_choicePanel.position.x, 1).From(6000);
    //}

    // Start is called before the first frame update

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            F.SetActive(true);
            Debug.Log("Trigger Entered");
            //zino.SetFloat("Speed", 0);
            //dialogueBox.SetActive(true);
            //playerController.enabled = false;
            isDialogueActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        F.SetActive(false);
        isDialogueActive = false;
        z.StopTalking();
        zino.SetTrigger("Idle");
        halrath.SetTrigger("Idle");

    }
    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        playerStatsManager = FindAnyObjectByType<PlayerStatsManager>();
        cameraSetting = FindAnyObjectByType<CameraSetting>();
        createCharacterText = FindAnyObjectByType<CreateCharacterText>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isDialogueActive)
                StartDialogue();
        }
    }
    public void StartDialogue()
    {
        F.SetActive(false);
        zino.SetTrigger("Talking");
        halrath.SetTrigger("Talking");
        z.StartTalking();
        h.StartTalking();
        playerController.enabled = false;
        Debug.Log("Story point: " + playerStatsManager.storyProgress);
        zino.SetFloat("Speed", 0);
        dialogueBox.SetActive(true);
        StartCoroutine(Chap());
    }

    IEnumerator Chap()
    {
        switch (playerStatsManager.storyProgress)
        {
            case 11:
                {
                    yield return createCharacterText.H.Say("Oh!{a} Đã về rồi à?");
                    yield return createCharacterText.Z.Say("Vâng, tôi đã lấy được món đồ mà ông cần.");
                    yield return createCharacterText.H.Say("Tốt lắm! Cảm ơn cậu rất nhiều, hãy cầm lấy nó đi.{c}Cậu sẽ cần đến nó đấy.");
                    yield return createCharacterText.Z.Say("Nhưng đây là...........");
                    yield return createCharacterText.H.Say("Tất cả chuyện này là tôi muốn thử sức chiến đấu của cậu đến đâu và cậu đã hoàn thành rất tốt.{a} Hãy mở cái túi ra xem bên trong đi!!");
                    yield return createCharacterText.N.Say("Bạn nhận được 1000 vàng và 2 bình máu");
                    playerStatsManager.storyProgress++;
                    playerStatsManager.AddGold(1000);
                    yield return createCharacterText.H.Say("Giờ cũng đã khá trễ rồi, cậu nên bắt đầu đến thành phố để gặp Scy trước khi trời tối.{c}Cánh cổng phía sau nhà đang hoạt động, nó sẽ dẫn cậu đến thành phố.");
                    yield return createCharacterText.Z.Say("Vâng, tôi sẽ đi ngay bây giờ.");
                    StartCoroutine(Chap());
                    break;
                }
            case 12:
                {
                    z.StopTalking();
                    h.StopTalking();
                    halrath.SetTrigger("Idle");
                    zino.SetTrigger("Idle");
                    playerStatsManager.AddHPPotion(2);
                    dialogueBox.SetActive(false);
                    playerController.enabled = true;
                    yield return null;
                    break;
                }
            default:
                break;
        }
    }

    //public void Choice1()
    //{
    //    playerStatsManager.storyProgress++;
    //    choicePanel.SetActive(false);
    //    StartCoroutine(Chap());
    //}
    //public void Choice2()
    //{
    //    playerStatsManager.storyProgress += 2;
    //    choicePanel.SetActive(false);
    //    StartCoroutine(Chap());
    //}
}
