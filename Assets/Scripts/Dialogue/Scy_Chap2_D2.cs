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
                    yield return createCharacterText.S.Say("Here we are!{c}First things first,{a} we need to get you some proper clothes.{c}You can’t be wandering around in those tattered rags.{c}You’ve got some gold, haven’t you? I’m fairly certain Halrath gave you a bit.");
                    yield return createCharacterText.Z.Say("Uh… yes,{a} I’ve got some gold with me.");
                    yield return createCharacterText.N.Say("Zino hands Scy 500 gold coins.");
                    playerStatsManager.AddGold(-500);
                    yield return createCharacterText.S.Say("That should do. Wait here a moment.");
                    StartCoroutine(BlackenOvertime());
                    yield return createCharacterText.N.Say("Scy enters the shop and returns with a new set of clothes for Zino. After changing, Zino feels a renewed sense of confidence.");
                    StartCoroutine(WhitenOvertime());
                    yield return createCharacterText.S.Say("Now then, go get some rest.{c}Head to the house to the right of the building where we met—I’ve prepared a place for you.{c}We’ll speak again tomorrow.");
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
