using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class Zino_Chap2_D1 : MonoBehaviour
{
    //public Button choice1;
    //public Button choice2;
    //public TMP_Text text1;
    //public TMP_Text text2;

    public GameObject dialogueBox;

    //public GameObject choicePanel;
    //public RectTransform _choicePanel;

    private PlayerStatsManager playerStatsManager;
    private PlayerController playerController;
    public DialogueBlendShapeController z;

    //public GameObject fighting;

    public CameraSetting cameraSetting;
    public Animator zino;
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

            Debug.Log("Trigger Entered");
            zino.SetFloat("Speed", 0);
            dialogueBox.SetActive(true);
            playerController.enabled = false;
            StartCoroutine(Chap());
        }
    }
    private void OnTriggerExit(Collider other)
    {
        z.StopTalking();
        zino.SetTrigger("Idle");
    }
    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        cameraSetting = FindAnyObjectByType<CameraSetting>();
        createCharacterText = FindAnyObjectByType<CreateCharacterText>();
        playerStatsManager = FindAnyObjectByType<PlayerStatsManager>();
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
    //    Debug.Log("Story point: " + playerStatsManager.storyProgress);
    //    Z = CreateCharacter("Zino") as Character_Text;

    //    dialogueBox.SetActive(true);
    //    StartCoroutine(Chap1());
    //}

    IEnumerator Chap()
    {
        switch (playerStatsManager.storyProgress)
        {
            case 9:
                {
                    createCharacterText.Z.Say("Nó đây rồi....{c}Chắc đây là những thứ Halrath cần.{c}Mang nó về thôi.");
                    playerStatsManager.storyProgress += 2;
                    StartCoroutine(Chap());
                    break;
                }
            case 11:
                {
                    z.StopTalking();
                    dialogueBox.SetActive(false);
                    gameObject.SetActive(false);
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
        playerStatsManager.storyProgress = +1;
        //choicePanel.SetActive(false);
        //StartCoroutine(Chap());
    }
    public void Choice2()
    {
        playerStatsManager.storyProgress = +2;
        //choicePanel.SetActive(false);
        //StartCoroutine(Chap());
    }
}
