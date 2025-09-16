using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class Halrath_Chap2_D1 : MonoBehaviour
{
    public Button choice1;
    public Button choice2;
    public TMP_Text text1;
    public TMP_Text text2;
    public GameObject F;

    public GameObject dialogueBox;
    private bool isDialogueActive = false;

    public GameObject choicePanel;
    public RectTransform _choicePanel;

    [SerializeField] private PlayerStatsManager playerStatsManager;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private DialogueBlendShapeController z;
    [SerializeField] private DialogueBlendShapeController h;

    //public GameObject fighting;

    public CameraSetting cameraSetting;
    [SerializeField] private Animator zino;
    [SerializeField] private Animator halrath;
    [SerializeField] CreateCharacterText createCharacterText;



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

    }
    private void Start()
    {
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
        z.StartTalking();
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
            case 0:
                {
                    yield return createCharacterText.H.Say("Xin chào, đã tỉnh dậy rồi à?");
                    yield return createCharacterText.Z.Say("......{a}...Tôi...Tôi đang ở đâu vậy?");
                    yield return createCharacterText.Z.Say("Ông là ai? Sao tôi lại ở đây?");
                    yield return createCharacterText.H.Say("Bình tĩnh nào.{c}Ta là Halrath. Còn cậu là ai? Tại sao cậu lại ở nơi này?{a} Nơi này cực kỳ nguy hiểm đấy.");
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
                    yield return createCharacterText.H.Say("1. Đồng ý.\n2. Không.");
                    ChoicePanelAnimation();
                    break;
                }
            case 1:
                {
                    yield return createCharacterText.H.Say("Tốt lắm, cậu ta tên là Scy, sống trong thành phố Azzaband.{c}Cậu có thể đến thành phố bằng cách đi qua cánh cổng dịch chuyển phía sau nhà.");
                    yield return createCharacterText.Z.Say("Cảm ơn ông rất nhiều, Halrath.{c} Tôi sẽ đến tìm và gặp Scy ngay.");
                    yield return createCharacterText.H.Say("Trước khi đi, tôi có một số thứ cho cậu, có thể sẽ giúp ích cho cậu trên đường đi đấy.");
                    yield return createCharacterText.N.Say("Nhận được 1 thanh kiếm ngắn.\nNhận được 2 bình máu.\nNhận được 2 bình Mana.");
                    playerStatsManager.AddHPPotion(2);
                    playerStatsManager.AddHPPotion(2);
                    playerController.SwitchToMeetHalrath();
                    dialogueBox.SetActive(false);
                    //gameObject.SetActive(false);
                    playerController.enabled = true;
                    yield return null;
                    break;
                }
            case 2:
                {
                    z.StopTalking();
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
}
