using CHARACTERS;
using COMMANDS;
using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Collections.LowLevel.Unsafe;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using System.IO;
using System;
using Unity.Mathematics;

public class Halrath : MonoBehaviour
{
    public Button choice1;
    public Button choice2;
    public TMP_Text text1;
    public TMP_Text text2;

    public GameObject dialogueBox;
    private bool isDialogueActive = false;

    public GameObject choicePanel;
    public RectTransform _choicePanel;

    [SerializeField] private PlayerController playerController;

    [SerializeField] private CharacterController Zino;
    [SerializeField] private DialogueBlendShapeController z;
    [SerializeField] private DialogueBlendShapeController h;

    //public GameObject fighting;

    public CameraSetting cameraSetting;
    [SerializeField] private Animator halrath;
    [SerializeField] private Animator zino;


    [SerializeField] private Character Z;
    [SerializeField] private Character H;


    private static Character CreateCharacter(string name) => CharacterManager.instance.CreateCharacter(name);



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
            Debug.Log("Trigger Entered");
            isDialogueActive = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        isDialogueActive = false;
        z.StopTalking();
        h.StopTalking();
        halrath.SetTrigger("Idle");
        zino.SetTrigger("Idle");
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F))
        {
            if (isDialogueActive)
                StartDialogue();
        }
    }
    public void StartDialogue()
    {
        zino.SetTrigger("Talking");
        halrath.SetTrigger("Talking");
        z.StartTalking();
        h.StartTalking();
        Zino.enabled = false;
        Debug.Log("Story point: " + playerController.storyProgress);
        Z = CreateCharacter("Zino") as Character_Text;
        H = CreateCharacter("Halrath") as Character_Text;

        dialogueBox.SetActive(true);
        StartCoroutine(Chap());
    }
    IEnumerator Chap()
    {
        switch (playerController.storyProgress)
        {
            case 0:
                {
                    yield return Z.Say("Ô Xin chào");
                    yield return H.Say("Oh hế lô");

                    yield return Z.Say("Tôi là Zino, rất vui được gặp bạn");

                    yield return H.Say("Chọn đi??");
                    choicePanel.SetActive(true);
                    yield return H.Say("1. ........\n2. .......");
                    break;
                }
            case 1:
                {
                    yield return Z.Say("Tôi muốn hỏi về Halrath");
                    dialogueBox.SetActive(false);
                    choicePanel.SetActive(false);
                    Zino.enabled = true;
                    yield return null;
                    break;
                }
            case 2:
                {
                    yield return Z.Say("Tôi muốn hỏi về Halrath");
                    dialogueBox.SetActive(false);
                    choicePanel.SetActive(false);
                    Zino.enabled = true;
                    yield return null;
                    break;
                }
            default:
                break;
        }
    }

    public void Choice1()
    {
        playerController.storyProgress = +1;
        choicePanel.SetActive(false);
        StartCoroutine(Chap());
    }
    public void Choice2()
    {
        playerController.storyProgress = +2;
        choicePanel.SetActive(false);
        StartCoroutine(Chap());
    }
}
