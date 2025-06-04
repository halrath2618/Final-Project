using UnityEngine;

public class EventTrigger : MonoBehaviour
{
    public GameObject dialogueBox;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player has exited the trigger quest area.");
            dialogueBox.SetActive(false); // Hide the dialogue box when player exits
        }
    }

    public void QuestTrigger()
    {
        Debug.Log("Quest Triggered!");
        dialogueBox.SetActive(true);
    }
}
