using System.Collections;
using UnityEngine;

public class Zino_Chap1_D2 : MonoBehaviour
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

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        cameraSetting = FindAnyObjectByType<CameraSetting>();
        createCharacterText = FindAnyObjectByType<CreateCharacterText>();
        playerStatsManager = FindAnyObjectByType<PlayerStatsManager>();
    }
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

    IEnumerator Chap2()
    {
        switch (playerStatsManager.storyProgress)
        {
            case 1:
                {
                    z.StartTalking();
                    yield return createCharacterText.Z.Say("This chest…{a} older than time itself.{c}I feel it trembling beneath my skin.{c}The relic — it lies within. I know it.{c}I’ve come too far to doubt now.");
                    playerStatsManager.storyProgress++;
                    StartCoroutine(Chap2());
                    break;
                }
            case 2:
                {
                    z.StopTalking();
                    dialogueBox.SetActive(false);
                    gameObject.SetActive(false);
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
