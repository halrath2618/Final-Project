using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Halrath_Chap2_D1 : MonoBehaviour
{
    public GameObject canvas;
    public CanvasGroup canvasGroup;
    public Button choice1;
    public Button choice2;
    public TMP_Text text1;
    public TMP_Text text2;
    public GameObject F;
    public SphereCollider repeat;

    public GameObject dialogueBox;
    private bool isDialogueActive = false;

    public GameObject choicePanel;
    public RectTransform _choicePanel;

    private PlayerStatsManager playerStatsManager;
    private PlayerController playerController;
    public DialogueBlendShapeController z;
    public DialogueBlendShapeController h;

    //public GameObject fighting;

    public CameraSetting cameraSetting;
    public Animator zino;
    public Animator halrath;
    private CreateCharacterText createCharacterText;



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
        halrath.SetTrigger("Idle");

    }
    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        playerStatsManager = FindAnyObjectByType<PlayerStatsManager>();
        cameraSetting = FindAnyObjectByType<CameraSetting>();
        createCharacterText = FindAnyObjectByType<CreateCharacterText>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            if (isDialogueActive)
                StartDialogue();
        }
        if (Input.GetKeyDown(KeyCode.Alpha1))
        {
            Choice1();
        }
        else if (Input.GetKeyDown(KeyCode.Alpha2))
        {
            Choice2();
        }
    }
    public void StartDialogue()
    {
        F.SetActive(false);
        zino.SetTrigger("Talking");
        halrath.SetTrigger("Talking");
        z.StartTalking();
        h.StartTalking();
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
            case 8:
                {
                    yield return createCharacterText.H.Say("Ah…{a} you’re awake.");
                    yield return createCharacterText.Z.Say("……{a}I…{a} Where am I?");
                    yield return createCharacterText.Z.Say("Who are you? Why am I here?");
                    yield return createCharacterText.H.Say("Easy now. Drink this,{a} it will help restore your strength.");
                    StartCoroutine(playerController.DrinkHPPotion());
                    yield return createCharacterText.H.Say("I am Halrath.{c}And you…{a} who are you?{c}What brought you to this place?{c}It is perilous beyond measure.");
                    yield return createCharacterText.Z.Say("I’m Zino.{c}I was searching for an ancient relic…{a} one fragment flew from the chest and led me to a strange gate.{c}When I stepped through,{a} I found myself in a dark forest teeming with monsters.{c}I fled…{a} and collapsed here.");
                    yield return createCharacterText.H.Say("You’re fortunate to be alive.{c}When I opened the door,{a} you were lying there—unconscious, wounded.{c}By the way, you said…{a} the ancient gate in that forest?");
                    yield return createCharacterText.Z.Say("Yes!");
                    yield return createCharacterText.H.Say("That gate hasn’t stirred in nearly twenty years…{a} Curious.{c}Do you still have the relic fragment?");
                    yield return createCharacterText.Z.Say("When it flew into the gate,{a} I tried to grab it—but failed.{c}I don’t know where it went.");
                    yield return createCharacterText.H.Say("Hmmm… that complicates things.");
                    yield return createCharacterText.Z.Say("How can I find it again?{c}How do I return?");
                    yield return createCharacterText.H.Say("The gate has long been dormant…{a} but I know someone who might help you.");
                    yield return createCharacterText.Z.Say("Really?{a} Who?");
                    yield return createCharacterText.H.Say("But first—you must rest and recover.{c}The path ahead is treacherous.{c}Tell me…{w} do you have the courage to face it?");

                    text1.text = "Yes, I will do whatever it takes to return to my world.";
                    text2.text = "This world is too frightening… I don't know if I can face it… Maybe I'll just die out there searching. I don't know...";
                    ChoicePanelAnimation();
                    break;
                }
            case 9:
                {
                    yield return createCharacterText.Z.Say("I accept.{c}I’ll do whatever it takes to return to my world.");
                    yield return createCharacterText.H.Say("Good.{c}The one you seek is named Scy.{c}He dwells in the city of Azzaband.{c}You may reach it through the teleportation gate behind this house.");
                    yield return createCharacterText.Z.Say("Thank you, Halrath.{c}I’ll find Scy without delay.");
                    yield return createCharacterText.H.Say("Before you go,{a} I have something for you.{c}It may aid you on the road ahead.");
                    yield return createCharacterText.N.Say("Received: 1 Short Sword.\nReceived: 2 Blood Vials.\nReceived: 2 Mana Vials.");
                    playerStatsManager.AddHPPotion(2);
                    playerStatsManager.AddMPPotion(2);
                    yield return createCharacterText.Z.Say("Thank you, Halrath.{c}I’ll go now.");
                    yield return createCharacterText.H.Say("Wait!!{a} don’t rush off just yet.{c}I need your help with a few tasks here.{c}And some training wouldn’t hurt,{a} would it?");
                    StartCoroutine(BlackenOvertime());
                    yield return createCharacterText.N.Say("After resting and regaining his strength,{a} Zino begins combat training with Halrath to prepare for the trials ahead.");
                    StartCoroutine(WhitenOvertime());
                    yield return createCharacterText.N.Say("Learned skills: -Basic Strike- and -Flurry Assault-.");
                    playerStatsManager.characterClassNum = 1;
                    yield return createCharacterText.H.Say("Now you’re ready.{c}The teleportation gate behind the house will take you to Azzaband. This is my letter of introduction,{a} give it to the gate sentries, they’ll know what to do.{c}But before you depart, may I ask for a favor?.");
                    yield return createCharacterText.Z.Say("Speak, and I’ll do what I can.");
                    yield return createCharacterText.H.Say("Deep within the forest lies something I need retrieved.{c}Head left from the house, go far into the woods, and you’ll find a chest.{c}Inside is the item I seek.{c}Bring it back, and you shall be rewarded.");
                    yield return createCharacterText.Z.Say("Understood. I’ll go at once.");
                    z.StopTalking();
                    h.StopTalking();
                    halrath.SetTrigger("Idle");
                    zino.SetTrigger("Idle");
                    dialogueBox.SetActive(false);
                    repeat.enabled = true;
                    gameObject.SetActive(false);
                    playerController.enabled = true;
                    yield return null;
                    break;
                }
            case 10:
                {
                    yield return createCharacterText.Z.Say("This world…{a} it’s terrifying.{c}I don’t know if I have the strength to face it… or if I’ll die out there, chasing shadows. I… I just don’t know anymore…");
                    yield return createCharacterText.H.Say("If you lack the strength to face what lies ahead,{a} I fear survival will be a fleeting hope.{c}But if your choice is made, I won’t stop you. Stay here as long as you wish.{c}In time, you may find it not so dreadful. Haha…");
                    yield return createCharacterText.N.Say("Having lost the will to press on, Zino chose to remain in the forest, living out the rest of his days in quiet solitude.");
                    StartCoroutine(playerController.GameOver());
                    SceneManager.LoadScene("Main Menu");
                    z.StopTalking();
                    h.StopTalking();
                    dialogueBox.SetActive(false);
                    //gameObject.SetActive(false);
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
        choicePanel.SetActive(false);
        StartCoroutine(Chap());
    }
    public void Choice2()
    {
        playerStatsManager.storyProgress += 2;
        choicePanel.SetActive(false);
        StartCoroutine(Chap());
    }
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
