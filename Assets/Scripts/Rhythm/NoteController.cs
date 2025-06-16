using UnityEngine;

public class NoteController : MonoBehaviour
{
    public bool canbePressed; // Indicates if the note can be pressed

    public KeyCode keyToPress; // The key that corresponds to this note
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(keyToPress))
        {
            if (canbePressed)
            {
                gameObject.SetActive(false); // Deactivate the note when the corresponding key is pressed

                GameManager.instance.NoteHit(); // Call the NoteHit method in GameManager
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Activator")
        {
            canbePressed = true; // Set the note as pressable when an arrow enters the trigger
            Debug.Log("Note hit!");
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canbePressed = false; // Reset the note when the arrow exits the trigger
            Debug.Log("Note exited!");
            GameManager.instance.NoteMissed(); // Call the NoteMissed method in GameManager
        }
    }

}
