using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DoorClose : MonoBehaviour
{
    public GameObject canvas;
    public CanvasGroup canvasGroup;
    public PlayerStatsManager playerStatsManager;
    public PlayerController playerController;
    public CreateCharacterText createCharacterText;
    public GameObject dialogueBox;
    public Animator zino;
    public Animator animator;

    private void Start()
    {
        playerStatsManager = FindAnyObjectByType<PlayerStatsManager>();
        playerController = FindAnyObjectByType<PlayerController>();
        createCharacterText = FindAnyObjectByType<CreateCharacterText>();
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            playerController.enabled = false;
            zino.SetTrigger("Idle");
            zino.SetFloat("Speed", 0f);
            dialogueBox.SetActive(true);
            animator.SetTrigger("Close");
            StartCoroutine(Chap());
        }
    }
    IEnumerator Chap()
    {
        switch (playerStatsManager.storyProgress)
        {
            case 15:
                {
                    yield return createCharacterText.Z.Say("Có lẽ nên nghỉ ngơi thôi,{a} ngày mai là một ngày dài đang đợi.");
                    yield return createCharacterText.N.Say("Zino đóng cửa và nghỉ ngơi tận hưởng khoảng thời gian còn lại trong ngày.");
                    
                    StartCoroutine(BlackenOvertime());
                    SceneManager.LoadScene("LoadingScene 2.5");
                    break;
                }
        }
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
}
