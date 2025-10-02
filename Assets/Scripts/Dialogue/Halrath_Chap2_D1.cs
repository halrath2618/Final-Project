using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Halrath_Chap2_D1 : MonoBehaviour
{
    public GameObject canvas;
    public CanvasGroup canvasGroup;
    public Button choice1;
    public Button choice2;
    public TMP_Text text1;
    public TMP_Text text2;
    public GameObject F;
    public SphereCollider repeat;

    public GameObject dialogueBox;
    private bool isDialogueActive = false;

    public GameObject choicePanel;
    public RectTransform _choicePanel;

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

    public void ChoicePanelAnimation()
    {
        choicePanel.SetActive(true);
        _choicePanel.DOMoveX(_choicePanel.position.x, 1).From(6000);
    }

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
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Choice1();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Choice2();
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
            case 8:
                {
                    yield return createCharacterText.H.Say("Xin chào, đã tỉnh dậy rồi à?");
                    yield return createCharacterText.Z.Say("......{a}...Tôi...Tôi đang ở đâu vậy?");
                    yield return createCharacterText.Z.Say("Ông là ai? Sao tôi lại ở đây?");
                    yield return createCharacterText.H.Say("Bình tĩnh nào.{c}Uống cái này đi! Nó sẽ giúp cậu hồi phục sức khỏe.");
                    StartCoroutine(playerController.DrinkHPPotion());
                    yield return createCharacterText.H.Say("Ta là Halrath. Còn cậu là ai? Tại sao cậu lại ở nơi này?{a} Nơi này cực kỳ nguy hiểm đấy.");
                    yield return createCharacterText.Z.Say("Tôi là Zino, trong một cuộc tìm kiếm cổ vật, {a}một mảnh cổ vật đã bay ra khỏi rương và dẫn tôi đến một cánh cổng kỳ lạ.{c} Sau khi bước vào, trước mắt tôi là một khu rừng tối với đầy rẫy quái vật sung quanh.{c} Tôi đã phải chạy trốn đến đây và gục ngã bất tỉnh sau đó.");
                    yield return createCharacterText.H.Say("Cậu thật sự rất may mắn khi còn sống sót.{c} Khi tôi mở cửa thì phát hiện cậu đang nằm bất tỉnh, người đầy thương tích.{c} Mà cậu nói là cánh cổng cổ đại trong khu rừng này à??");
                    yield return createCharacterText.Z.Say("Vâng!!");
                    yield return createCharacterText.H.Say("Cánh cổng đó đã không hoạt động trong suốt gần 20 năm nay rồi.....{a} Thật kỳ lạ.{c}Cậu có đang giữ mảnh cổ vật đó không?");
                    yield return createCharacterText.Z.Say("Khi nó bay vào cánh cổng, tôi đã cố gắng nắm lấy nó nhưng không được.{c} Tôi không biết nó đã đi đâu nữa rồi.");
                    yield return createCharacterText.H.Say("Hmmm,{a} khó rồi đấy.......");
                    yield return createCharacterText.Z.Say("Bây giờ làm sao tôi có thể tìm lại mảnh cổ vật đó và quay trở lại được?");
                    yield return createCharacterText.H.Say("Cánh cổng đó đã không hoạt động trong một khoảng thời gian dài......{a} tôi biết có một người có thể giúp cậu.");
                    yield return createCharacterText.Z.Say("Thật sao, là ai vậy?");
                    yield return createCharacterText.H.Say("Nhưng trước hết, cậu cần phải nghỉ ngơi và hồi phục sức khỏe đã.{c} Nhưng con đường phía trước sẽ rất nguy hiểm, cậu có dám đối mặt hay không?");
                    yield return createCharacterText.N.Say("1. Đồng ý.\n2. Không.");
                    ChoicePanelAnimation();
                    break;
                }
            case 9:
                {
                    yield return createCharacterText.Z.Say("Tôi đồng ý, tôi sẽ làm bất cứ điều gì để có thể trở về thế giới của mình.");
                    yield return createCharacterText.H.Say("Tốt lắm, cậu ta tên là Scy, sống trong thành phố Azzaband.{c}Cậu có thể đến thành phố bằng cách đi qua cánh cổng dịch chuyển phía sau nhà.");
                    yield return createCharacterText.Z.Say("Cảm ơn ông rất nhiều, Halrath.{c} Tôi sẽ đến tìm và gặp Scy ngay.");
                    yield return createCharacterText.H.Say("Trước khi đi, tôi có một số thứ cho cậu, có thể sẽ giúp ích cho cậu trên đường đi đấy.");
                    yield return createCharacterText.N.Say("Nhận được 1 thanh kiếm ngắn.\nNhận được 2 bình máu.\nNhận được 2 bình Mana.");
                    playerStatsManager.AddHPPotion(2);
                    playerStatsManager.AddMPPotion(2);
                    yield return createCharacterText.Z.Say("Cảm ơn ông rất nhiều, Halrath.{c} Tôi sẽ đến tìm và gặp Scy ngay.");
                    yield return createCharacterText.H.Say("Khoan đã, đừng vội, tôi cần cậu giúp một số việc ở đây. Cũng như luyện tập một tí chứ nhỉ?");
                    StartCoroutine(BlackenOvertime());
                    yield return createCharacterText.N.Say("Sau khi nghỉ ngơi và hồi phục sức khỏe, Zino bắt đầu luyện tập chiến đấu với Halrath để chuẩn bị cho những thử thách phía trước.");
                    StartCoroutine(WhitenOvertime());
                    yield return createCharacterText.N.Say("Nhận được kỹ năng -tấn công cơ bản- và -tấn công liên trảm-");
                    playerStatsManager.characterClassNum = 1;
                    yield return createCharacterText.H.Say("Được rồi, bây giờ cậu đã sẵn sàng để đi rồi đấy.{c} Cánh cổng dịch chuyển ở phía sau nhà sẽ đưa cậu đến thành phố Azzaband.{c} Đây là lá thư giới thiệu của tôi, hãy đưa nó cho lính canh ở cổng vào, họ sẽ biết phải làm gì.{c} Trước khi khởi hành, tôi nhờ cậu một chuyện được chứ?");
                    yield return createCharacterText.Z.Say("Ông cứ nói đi, tôi sẽ giúp ông.");
                    yield return createCharacterText.H.Say("Có một thứ để ở sâu trong khu rừng tôi cần cậu mang về.{c}Hãy đi về phía bên trái của căn nhà, đi sâu vào trong rừng, cậu sẽ thấy một cái hòm chứa đồ, bên trong có món đồ tôi cần. Hãy mang nó về đây và tôi sẽ thưởng cho cậu.");
                    yield return createCharacterText.Z.Say("Vâng, tôi sẽ đi ngay bây giờ.");
                    z.StopTalking();
                    h.StopTalking();
                    halrath.SetTrigger("Idle");
                    zino.SetTrigger("Idle");
                    dialogueBox.SetActive(false);
                    repeat.enabled = true;
                    gameObject.SetActive(false);
                    playerController.enabled = true;
                    yield return null;
                    break;
                }
            case 10:
                {
                    yield return createCharacterText.Z.Say("Thế giới này quá đáng sợ… tôi không biết liệu mình có đủ sức đối mặt không…{c}Hay tôi sẽ chết ngoài đấy trong lúc tìm kiếm.{c}Tôi không biết nữa.....");
                    yield return createCharacterText.H.Say("Nếu cậu không đủ sức để đối mặt, tôi e rằng rất khó để cậu có thể vượt qua được.{c} Nhưng nếu cậu đã quyết định rồi thì tôi sẽ không ngăn cản nữa.{c}Cứ ở lại đây bao lâu tùy thích, khi quen với nó rồi, có lẽ sẽ không quá tệ đâu. Haha");
                    yield return createCharacterText.N.Say("Vì không còn ý chí vượt qua nữa, Zino quyết định ở lại khu rừng này và sống phần đời còn lại ở đây.");
                    StartCoroutine(playerController.GameOver());
                    SceneManager.LoadScene("Main Menu");
                    z.StopTalking();
                    h.StopTalking();
                    dialogueBox.SetActive(false);
                    //gameObject.SetActive(false);
                    playerController.enabled = true;
                    yield return null;
                    break;
                }
            default:
                break;
        }
    }

    public void Choice1()
    {
        playerStatsManager.storyProgress++;
        choicePanel.SetActive(false);
        StartCoroutine(Chap());
    }
    public void Choice2()
    {
        playerStatsManager.storyProgress += 2;
        choicePanel.SetActive(false);
        StartCoroutine(Chap());
    }
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
