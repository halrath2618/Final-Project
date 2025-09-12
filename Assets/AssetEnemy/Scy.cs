using CHARACTERS;
using DG.Tweening;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Scy : MonoBehaviour
{
    public Button choice1;
    public Button choice2;
    public TMP_Text text1;
    public TMP_Text text2;

    public GameObject dialogueBox;
    private bool isDialogueActive = false;

    public GameObject choicePanel;
    public RectTransform _choicePanel;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private CharacterController Zino;
    [SerializeField] private DialogueBlendShapeController z;
    [SerializeField] private DialogueBlendShapeController s;

    //public GameObject fighting;

    public CameraSetting cameraSetting;
    [SerializeField] private Animator scy;
    [SerializeField] private Animator zino;


    [SerializeField] private Character Z;
    [SerializeField] private Character S;


    private static Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);



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
            Debug.Log("Trigger Entered");
            isDialogueActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isDialogueActive = false;
        z.StopTalking();
        s.StopTalking();
        scy.SetTrigger("Idle");
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
        scy.SetTrigger("Talking");
        z.StartTalking();
        s.StartTalking();
        Zino.enabled = false;
        Debug.Log("Story point: " + playerController.storyProgress);
        Z = CreateCharacter("Zino") as Character_Text;
        S = CreateCharacter("Scy") as Character_Text;

        dialogueBox.SetActive(true);
        StartCoroutine(Chap());
    }
    IEnumerator Chap()
    {
        switch (playerController.storyProgress)
        {
            case 0:
                {
                    yield return S.Say("Này Zino cậu có dự định gì khi rời khỏi thành phố này không?");
                    yield return Z.Say("Chắc có lẽ tôi sẽ phải hoàn thành tiếp các thử thách để chinh phục và thu thập đủ các mảnh ghép để nhanh chóng trở về thế giới của riêng mình.");

                    yield return S.Say("Tốt lắm… Tôi rất thích những người có lòng dũng cảm như cậu.");
                    yield return Z.Say("Cảm ơn Scy. Nhưng tớ thấy rất áy náy, lúc đó vì mãi đắm chìm trong mộng huyễn mà suýt chút nữa là tôi hại cậu rồi.");

                    yield return S.Say("Không hẳn đâu, lúc đấy tôi cũng rất sợ, sợ phải chết và tôi sợ chính nổi sợ của tôi nữa.");
                    yield return S.Say("Khi mà tôi khuyên cậu tỉnh lại cũng chỉnh là thời khắc tôi đánh thức bản thân của mình.");
                    yield return S.Say("Trước kia vì sự nhút nhát không dám đối mặt với quá khứ và kỉ niệm tôi đã bị giam cầm và liên tục thất bại khi truy tìm các mảnh ghép báu vật, nhờ có cậu mà tôi đã tìm lại được bản thân của mình. Cảm ơn cậu Zino à….");
                    
                    yield return Z.Say("Cậu rất dũng cảm mà Scy, không có cậu chắc giờ này tôi còn ở lại cảnh cổng ấy mất.");

                    yield return S.Say("Không nhắc lại chuyện cũ nữa. Nếu như cậu đã muốn hoàn thành xong hết thử thách để sớm quay về thế giới thực của mình thì Scy tôi sẽ cố gắng giúp cậu hoàn thành.");
                    yield return S.Say("Tuy nhiên, cậu còn nhớ lời hứa của mình khi nhờ tôi làm nhiệm vụ lần này không?");

                    choicePanel.SetActive(true);
                    yield return S.Say("1. Nhớ chứ, cậu nói đi tôi sẵn sàng thực hiện rồi đây.\n2. Nhớ chứ, nhưng cậu có thể tạm hoãn việc thực hiện nhiệm vụ này không?\r\n");
                    break;
                }
            case 1:
                {
                    yield return S.Say("Đừng làm vẻ mặt nghiêm túc quá đấy Zino. Nhiệm vụ đơn giản thôi?");
                    yield return S.Say("Nhưng cần khá nhiều thời gian để thực hiện.");
                    yield return S.Say("Ngày mai, cậu và tôi sẽ cùng nhau đi tìm 1 ít thảo dược và sách phép thuật đề tôi điều chế 1 số vật dụng trong phòng phép thuật và cũng như giúp cậu hoàn thành nhiệm vụ sắp tới.");
                    yield return S.Say("À… mà quên nói với cậu. Ở nhiệm vụ tiếp theo tôi sẽ không đi cùng cậu được bởi còn có 1 số việc quan trọng tại thị trấn, nên trong thời gian này tranh thủ học hỏi được thêm gì ở đây thì nhanh chóng đi nhé.\r\n");

                    yield return S.Say("Tuân lệnh ngài Scy.");

                    dialogueBox.SetActive(false);
                    choicePanel.SetActive(false);
                    Zino.enabled = true;
                    yield return null;
                    break;
                }
            case 2:
                {
                    yield return Z.Say("Hiện tại tôi muốn bản thân mình ưu tiên việc hoàn thành việc chinh phục vực trước.");
                    yield return Z.Say("Sau khi trở về tôi sẽ cùng cậu thực hiện lời hứa nhé.");

                    yield return S.Say("Đừng làm vẻ mặt nghiêm túc quá đấy Zino.");
                    yield return S.Say("Không sao đâu vậy tôi sẽ tự thực hiện.");
                    yield return S.Say("À… mà quên nói với cậu. Ở nhiệm vụ tiếp theo tôi sẽ không đi cùng cậu được bởi còn có 1 số việc quan trọng tại thị trấn, nên trong thời gian này tranh thủ học hỏi được thêm gì ở đây thì nhanh chóng đi nhé.");

                    yield return Z.Say("Tuân lệnh ngài Scy.");
                    dialogueBox.SetActive(false);
                    choicePanel.SetActive(false);
                    Zino.enabled = true;
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
        choicePanel.SetActive(false);
        StartCoroutine(Chap());
    }
    public void Choice2()
    {
        playerController.storyProgress = +2;
        choicePanel.SetActive(false);
        StartCoroutine(Chap());
    }
}
