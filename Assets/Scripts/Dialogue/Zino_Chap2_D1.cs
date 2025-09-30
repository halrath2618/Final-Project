using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using UnityEngine.SceneManagement;

public class Zino_Chap2_D1 : MonoBehaviour
{
    //public GameObject canvas;
    //public CanvasGroup canvasGroup;
    //public Button choice1;
    //public Button choice2;
    //public TMP_Text text1;
    //public TMP_Text text2;
    public GameObject F;
    public SphereCollider repeat;

    public GameObject dialogueBox;
    private bool isDialogueActive = false;

    //public GameObject choicePanel;
    //public RectTransform _choicePanel;

    private PlayerStatsManager playerStatsManager;
    private PlayerController playerController;
    public DialogueBlendShapeController z;
    //public DialogueBlendShapeController h;

    //public GameObject fighting;

    public Animator zino;
    public Animator treasure;
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
            dialogueBox.SetActive(true);
            if (isDialogueActive)
                StartDialogue();
        }
        //if (Input.GetKeyDown(KeyCode.Alpha1))
        //{
        //    Choice1();
        //}
        //else if (Input.GetKeyDown(KeyCode.Alpha2))
        //{
        //    Choice2();
        //}
    }
    public void StartDialogue()
    {
        F.SetActive(false);
        zino.SetTrigger("Talking");
        treasure.SetTrigger("Open");
        z.StartTalking();
        playerController.enabled = false;
        Debug.Log("Story point: " + playerStatsManager.storyProgress);
        zino.SetFloat("Speed", 0);
        
        StartCoroutine(Chap());
    }

    IEnumerator Chap()
    {
        switch (playerStatsManager.storyProgress)
        {
            case 9:
                {
                    yield return createCharacterText.Z.Say("Nó đây rồi....{c}Chắc đây là những thứ Halrath cần.{c}Mang nó về thôi.");
                    playerStatsManager.storyProgress += 2;
                    StartCoroutine(Chap());
                    break;
                }
            case 11:
                {
                    z.StopTalking();
                    dialogueBox.SetActive(false);
                    playerController.enabled = true;
                    //choicePanel.SetActive(false);
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
