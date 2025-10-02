using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using Pathfinding;
using CHARACTERS;
using UnityEngine.Video;

public class Scy_Chap3_D1 : MonoBehaviour
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

    public CanvasShaking cv_Shaking;

    public VideoPlayer[] videos;

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
        cv_Shaking = GetComponent<CanvasShaking>();

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
            case 15:
                {
                    yield return createCharacterText.S.Say("Good morning! Sleep well?{c}Today will be a long one for you!");
                    cv_Shaking.Shake();
                    yield return createCharacterText.Z.Say("Ehhh???!!");
                    yield return createCharacterText.S.Say("Snap out of it!{c}Sit down and listen closely.{c}This is where your journey begins.");
                    yield return createCharacterText.S.Say("To conquer what lies ahead, you must choose your profession.{c}Each path bears its own strengths and weaknesses—so choose wisely.");
                    videos[0].enabled = true;
                    videos[0].Play();
                    yield return createCharacterText.S.Say("First, the Brawler — nimble, resilient, with moderate damage.");
                    videos[0].Stop();
                    videos[0].enabled = false;
                    videos[1].enabled = true;
                    videos[1].Play();
                    yield return createCharacterText.S.Say("Next, the Mage — devastating power, but slow to cast and fragile.");
                    videos[1].Stop();
                    videos[1].enabled = false;
                    videos[2].enabled = true;
                    videos[2].Play();
                    yield return createCharacterText.S.Say("Lastly, the Sword Master — pure blade technique, swift and deadly, but with almost no endurance.");
                    videos[2].Stop();
                    videos[2].enabled = false;
                    yield return createCharacterText.S.Say("Take your time. When you’ve made your decision, come find me.");
                    playerStatsManager.storyProgress++;
                    StartCoroutine(Chap());
                    break;
                }
            case 16:
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
