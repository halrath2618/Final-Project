using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Pathfinding;
using CHARACTERS;

public class Scy_Chap2_D1 : MonoBehaviour
{
    public GameObject canvas;
    public CanvasGroup canvasGroup;
    //public Button choice1;
    //public Button choice2;
    //public TMP_Text text1;
    //public TMP_Text text2;
    public GameObject F;

    public AIDestinationSetter location;

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
        location = FindAnyObjectByType<AIDestinationSetter>();
        location.enabled = false;
        
        
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
            case 13:
                {
                    yield return createCharacterText.S.Say("You kept me waiting, young one.{c}Before you throw a barrage of questions at me…{a} I’m Scy.{c}A pleasure to meet you.{c}Halrath spoke of you. Come in—I’ll tell you everything.");
                    StartCoroutine(BlackenOvertime());
                    yield return createCharacterText.N.Say("Scy sat and spoke at length, recounting the city’s history and the events that had recently unfolded. Zino listened intently, noting every detail with care.");
                    StartCoroutine(WhitenOvertime());
                    yield return createCharacterText.S.Say("Now then, come with me.{c}There are preparations to be made.");
                    playerStatsManager.storyProgress++;
                    StartCoroutine(Chap());
                    break;
                }
            case 14:
                {
                    location.enabled = true;
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
