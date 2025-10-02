using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Pathfinding;
using CHARACTERS;

public class Scy_Chap2_D2 : MonoBehaviour
{
    public GameObject canvas;
    public CanvasGroup canvasGroup;
    //public Button choice1;
    //public Button choice2;
    //public TMP_Text text1;
    //public TMP_Text text2;
    public GameObject F;

    public GameObject dialogueBox;
    private bool isDialogueActive = false;

    //public GameObject choicePanel;
    //public RectTransform _choicePanel;


    public PlayerStatsManager playerStatsManager;
    public PlayerController playerController;
    public DialogueBlendShapeController z;
    public DialogueBlendShapeController s;

    //public GameObject fighting;

    public Animator zino;
    public Animator scy;
    public CreateCharacterText createCharacterText;



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
        scy.SetTrigger("Idle");

    }
    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        createCharacterText = FindAnyObjectByType<CreateCharacterText>();
        playerStatsManager = FindAnyObjectByType<PlayerStatsManager>();


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
        scy.SetTrigger("Talking");
        z.StartTalking();
        s.StartTalking();
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
            case 14:
                {
                    yield return createCharacterText.S.Say("Đến rồi!{c}Đầu tiên vẫn phải mua cho cậu một bộ quần áo đã.{c}Cậu không thể cứ mặc bộ đồ rách rưới đó đi loanh quanh được.{c}Cậu có mang theo vàng không, tôi khá chắc là Halrath đã đưa cho cậu một ít rồi đúng không?");
                    yield return createCharacterText.Z.Say("Ừm...{a} Có chứ, tôi có một ít vàng đây.");
                    yield return createCharacterText.N.Say("Zino đưa cho Scy 500 đồng vàng.");
                    playerStatsManager.AddGold(-500);
                    yield return createCharacterText.S.Say("Chắc là đủ đấy, đợi tôi một chút nhé.");
                    StartCoroutine(BlackenOvertime());
                    yield return createCharacterText.N.Say("Scy đi vào trong cửa hàng và mua cho Zino một bộ quần áo mới.{c}Sau khi thay xong, Zino cảm thấy tự tin hơn rất nhiều.");
                    StartCoroutine(WhitenOvertime());
                    yield return createCharacterText.S.Say("Bây giờ thì,{a}cậu hãy đi nghỉ ngơi trước đi.{c}Hãy đến căn nhà bên phải của tòa nhà nơi tôi gặp cậu,{a} tôi có chuẩn bị sẵn cho cậu đấy.{c}Hẹn gặp lại cậu vào ngày mai nhé.{c}");
                    playerStatsManager.storyProgress++;
                    StartCoroutine(Chap());
                    break;
                }
            case 15:
                {
                    z.StopTalking();
                    s.StopTalking();
                    scy.SetTrigger("Idle");
                    zino.SetTrigger("Idle");
                    dialogueBox.SetActive(false);
                    playerController.enabled = true;
                    gameObject.SetActive(false);
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
    IEnumerator BlackenOvertime()
    {
        canvas.SetActive(true);
        float elapsedTime = 0f;
        float duration = 2f; // Duration of the fade effect in seconds
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(elapsedTime / duration);
            canvasGroup.alpha = alpha;
            yield return null;
        }
    }
    IEnumerator WhitenOvertime()
    {
        float elapsedTime = 0f;
        float duration = 2f; // Duration of the fade effect in seconds
        while (elapsedTime < duration)
        {
            elapsedTime += Time.deltaTime;
            float alpha = Mathf.Clamp01(1 - (elapsedTime / duration));
            canvasGroup.alpha = alpha;
            yield return null;
        }
        canvas.SetActive(false);
    }
}
