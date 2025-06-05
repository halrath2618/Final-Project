using UnityEngine;
using System.Collections;
using TMPro;
public class EventTrigger : MonoBehaviour
{
    [Header("Animators for Zino and Halrath")]
    [SerializeField] private Animator animZ;
    [SerializeField] private Animator animH;
    [SerializeField] private GameObject dialogueBox;

    [Header("Dialogue Text Objects for Zino and Halrath")]
    [SerializeField] private TMP_Text dialogueText;
    [SerializeField] private TMP_Text zName;
    [SerializeField] private TMP_Text hName;

    [Header("Dialogue Text Objects")]
    [SerializeField] private GameObject zName_Text;
    [SerializeField] private GameObject hName_Text;
    [SerializeField] private GameObject dialogue_Text;
    
    [Header("Talking Blendshape")]
    [SerializeField] private DialogueBlendShapeController zino;
    [SerializeField] private DialogueBlendShapeController halrath;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        dialogueBox.SetActive(false); // Ensure the dialogue box is initially hidden
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has entered the trigger quest area.");
            QuestTrigger();
            animZ.SetTrigger("Talking"); // Trigger the talking animation
            animH.SetTrigger("Talking"); // Trigger the talking animation for Halrath
            zino.StartTalking(); // Start Zino's talking blend shape animation
            halrath.StartTalking(); // Start Halrath's talking blend shape animation
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has exited the trigger quest area.");
            dialogueBox.SetActive(false); // Hide the dialogue box when player exits
            animZ.SetTrigger("Idle"); // Reset to idle animation
            animH.SetTrigger("Idle"); // Reset Halrath to idle animation
            zName_Text.SetActive(false); // Hide Zino's name text
            hName_Text.SetActive(false); // Hide Halrath's name text
            dialogue_Text.SetActive(false); // Hide the dialogue text
            dialogueText.text = ""; // Clear the dialogue text
            zino.StopTalking(); // Stop Zino's talking blend shape animation
            halrath.StopTalking(); // Stop Halrath's talking blend shape animation
        }
    }

    public void QuestTrigger()
    {
        Debug.Log("Quest Triggered!");
        dialogueBox.SetActive(true);
        StartCoroutine(TestQuest()); // Start the quest dialogue coroutine
    }

    IEnumerator TestQuest()
    {
        zName.text = "Zino"; // Set Zino's name
        zName_Text.SetActive(true);
        dialogue_Text.SetActive(true);
        yield return dialogueText.text = "Hello, this is the quest test dialogue for Zino. " +
                            "Please follow the instructions carefully to complete the quest. " +
                            "Make sure to interact with all necessary objects and characters. " +
                            "Good luck on your adventure!";
        yield return new WaitForSeconds(10f); // Wait for seconds to allow the player to read the dialogue
        zName_Text.SetActive(false);
        dialogue_Text.SetActive(false);

        yield return new WaitForSeconds(2f); // Wait for a short time before showing Halrath's dialogue
        hName.text = "Halrath"; // Set Halrath's name
        hName_Text.SetActive(true);
        dialogueText.text = ""; // Clear the dialogue text for Halrath
        dialogue_Text.SetActive(true);
        yield return dialogueText.text = "Hello, I am Halrath. " +
                            "I will assist you in your quest. " +
                            "Make sure to pay attention to the details and follow the quest instructions carefully.";
    }
}
