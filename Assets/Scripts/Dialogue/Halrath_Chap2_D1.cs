using UnityEngine;
using System.Collections;

public class Halrath_Chap2_D1 : MonoBehaviour
{
    //public Button choice1;
    //public Button choice2;
    //public TMP_Text text1;
    //public TMP_Text text2;

    public GameObject dialogueBox;
    private bool isDialogueActive = false;

    //public GameObject choicePanel;
    //public RectTransform _choicePanel;

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

            Debug.Log("Trigger Entered");
            //zino.SetFloat("Speed", 0);
            //dialogueBox.SetActive(true);
            //playerController.enabled = false;
            isDialogueActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isDialogueActive = false;
        z.StopTalking();
        zino.SetTrigger("Idle");

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
        zino.SetTrigger("Talking");
        z.StartTalking();
        playerController.enabled = false;
        Debug.Log("Story point: " + playerController.storyProgress);
        zino.SetFloat("Speed", 0);
        dialogueBox.SetActive(true);
        StartCoroutine(Chap());
    }

    IEnumerator Chap()
    {
        switch (playerController.storyProgress)
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
                    break;
                }
            case 1:
                {
                    z.StopTalking();
                    dialogueBox.SetActive(false);
                    //gameObject.SetActive(false);
                    playerController.enabled = true;
                    //choicePanel.SetActive(false);
                    yield return null;
                    break;
                }
            default:
                break;
        }
    }

    public void Choice1()
    {
        playerController.storyProgress = +1;
        //choicePanel.SetActive(false);
        //StartCoroutine(Chap());
    }
    public void Choice2()
    {
        playerController.storyProgress = +2;
        //choicePanel.SetActive(false);
        //StartCoroutine(Chap());
    }
}
