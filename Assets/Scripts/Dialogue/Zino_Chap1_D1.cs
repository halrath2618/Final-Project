using System.Collections;
using UnityEngine;

public class Zino_Chap1_D1 : MonoBehaviour
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
            playerController.enabled = false;
            zino.SetFloat("Speed", 0);
            dialogueBox.SetActive(true);
            StartCoroutine(Chap1());
        }
    }
    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        cameraSetting = FindAnyObjectByType<CameraSetting>();
        createCharacterText = FindAnyObjectByType<CreateCharacterText>();
        playerStatsManager = FindAnyObjectByType<PlayerStatsManager>();
    }
    private void OnTriggerExit(Collider other)
    {
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
    //    Debug.Log("Story point: " + playerStatsManager.storyProgress);
    //    Z = CreateCharacter("Zino") as Character_Text;

    //    dialogueBox.SetActive(true);
    //    StartCoroutine(Chap1());
    //}
    IEnumerator Chap1()
    {
        switch (playerStatsManager.storyProgress)
        {
            case 0:
                {
                    z.StartTalking();
                    yield return createCharacterText.Z.Say("So… this is the place.{c}Whispered only in half-remembered tales and fading dreams.{c}The forest devours the path, the stars retreat from the heavens…{a} yet I press on.{c}Something beckons.{a} Something old.{a} Older than time,{a} and far less forgiving.");
                    playerStatsManager.storyProgress++;
                    StartCoroutine(Chap1());
                    break;
                }
            case 1:
                {
                    z.StopTalking();
                    dialogueBox.SetActive(false);
                    gameObject.SetActive(false);
                    //choicePanel.SetActive(false);
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
        //choicePanel.SetActive(false);
        //StartCoroutine(Chap());
    }
    public void Choice2()
    {
        playerStatsManager.storyProgress += 2;
        //choicePanel.SetActive(false);
        //StartCoroutine(Chap());
    }
}
