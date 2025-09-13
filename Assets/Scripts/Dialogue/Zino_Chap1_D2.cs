using System.Collections;
using UnityEngine;

public class Zino_Chap1_D2 : MonoBehaviour
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

    //public GameObject fighting;

    public CameraSetting cameraSetting;
    [SerializeField] private Animator zino;
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
            zino.SetFloat("Speed", 0);
            playerController.enabled = false;
            dialogueBox.SetActive(true);
            StartCoroutine(Chap2());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isDialogueActive = false;
        z.StopTalking();
        zino.SetTrigger("Idle");
    }
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.F))
    //    {
    //        if (isDialogueActive)
    //            StartDialogue();
    //    }
    //}
    //public void StartDialogue()
    //{
    //    zino.SetTrigger("Talking");
    //    z.StartTalking();
    //    Zino.enabled = false;
    //    Debug.Log("Story point: " + playerController.storyProgress);
    //    Z = CreateCharacter("Zino") as Character_Text;

    //    dialogueBox.SetActive(true);
    //    StartCoroutine(Chap1());
    //}

    IEnumerator Chap2()
    {
        switch (playerController.storyProgress)
        {
            case 1:
                {
                    z.StartTalking();
                    yield return createCharacterText.Z.Say("Chiếc rương này... cổ hơn cả thời gian. Tôi cảm nhận được nó đang rung lên dưới làn da mình. Di vật — nó ở bên trong. Tôi biết mà. Tôi đã đi quá xa để còn nghi ngờ.");
                    playerController.storyProgress++;
                    StartCoroutine(Chap2());
                    break;
                }
            case 2:
                {
                    z.StopTalking();
                    dialogueBox.SetActive(false);
                    //choicePanel.SetActive(false);
                    playerController.enabled = true;
                    gameObject.GetComponent<SphereCollider>().enabled = false;
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
