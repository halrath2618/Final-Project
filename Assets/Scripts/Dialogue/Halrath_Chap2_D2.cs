using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Halrath_Chap2_D2 : MonoBehaviour
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
            case 11:
                {
                    yield return createCharacterText.H.Say("Ah!{a} You’ve returned.");
                    yield return createCharacterText.Z.Say("Yes, I’ve retrieved the item you asked for.");
                    yield return createCharacterText.H.Say("Well done. Thank you, truly.{c}Take this,{a} you’ll need it.");
                    yield return createCharacterText.Z.Say("But this is.....");
                    yield return createCharacterText.H.Say("All of this was a test, {a}to see the measure of your strength.{c}And you’ve proven yourself. Go on, open the pouch.");
                    yield return createCharacterText.N.Say("Received 1000 gold and 2 Blood Vials.");
                    playerStatsManager.storyProgress++;
                    playerStatsManager.AddGold(1000);
                    yield return createCharacterText.H.Say("It’s getting late.{c}You should make haste to the city and find Scy before nightfall.{c}The gate behind the house is active, it will take you there.");
                    yield return createCharacterText.Z.Say("Understood. I’ll leave at once.");
                    StartCoroutine(Chap());
                    break;
                }
            case 12:
                {
                    z.StopTalking();
                    h.StopTalking();
                    halrath.SetTrigger("Idle");
                    zino.SetTrigger("Idle");
                    playerStatsManager.AddHPPotion(2);
                    dialogueBox.SetActive(false);
                    playerController.enabled = true;
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
}
