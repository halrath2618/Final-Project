using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Guard_Chap2_D1 : MonoBehaviour
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

    public PlayerStatsManager playerStatsManager;
    public PlayerController playerController;
    public DialogueBlendShapeController z;

    //public GameObject fighting;

    public Animator zino;
    public BoxCollider door;
    public Animator aran;
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
        aran.SetTrigger("Idle");

    }
    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        playerStatsManager = FindAnyObjectByType<PlayerStatsManager>();
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
        aran.SetTrigger("talking");
        z.StartTalking();
        playerController.enabled = false;
        zino.SetFloat("Speed", 0);
        dialogueBox.SetActive(true);
        StartCoroutine(Chap());
    }

    IEnumerator Chap()
    {
        switch (playerStatsManager.storyProgress)
        {
            case 12:
                {
                    yield return createCharacterText.Z.Say("Greetings~ May I ask… where might I find this person?");
                    yield return createCharacterText.N.Say("Zino hands the guard Halrath’s letter.");
                    yield return createCharacterText.G.Say("Ah, I see.{c}Very well, follow my directions.{c}Head straight down this road to the fountain, then turn left.{c}You’ll find a large house on your right.{c}He’s inside.");
                    yield return createCharacterText.Z.Say("Thank you kindly!");
                    playerStatsManager.storyProgress++;
                    StartCoroutine(Chap());
                    break;
                }
            case 13:
                {
                    door.enabled = false;
                    z.StopTalking();
                    aran.SetTrigger("Idle");
                    zino.SetTrigger("Idle");
                    playerStatsManager.AddHPPotion(2);
                    dialogueBox.SetActive(false);
                    playerController.enabled = true;
                    yield return null;
                    break;
                }
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
