using UnityEngine;

public class NoteController : MonoBehaviour
{
    public bool canbePressed; // Indicates if the note can be pressed
    public bool isMissed; // Indicates if the note was missed

    public float beatTempo; // Beats per minute
    public bool hasStarted; // Flag to check if the beat has started
    public float tempoMultiplier; // Multiplier for the beat tempo

    public KeyCode keyToPress; // The key that corresponds to this note
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.position += new Vector3(0f, beatTempo * Time.deltaTime * tempoMultiplier, 0f); // Move downwards based on the beat tempo
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
        if (other.tag == "Activator")
        {
            canbePressed = true;
            Debug.Log("Note can be pressed!"); // Log when the note can be pressed
        }
        else if (other.tag == "EndZone")
        {
            isMissed = true; // Set isMissed to true if the note enters the end zone
            canbePressed = false; // Disable pressing when the note enters the end zone
            Debug.Log("Note End Zone Entered!"); // Log when the note enters the end zone
            GameManager.instance.NoteMissed(); // Call the NoteMissed method in GameManager
            gameObject.SetActive(false); // Deactivate the note when it is missed
        }
    }
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.tag == "Activator")
        {
            canbePressed = false;
            Debug.Log("Note Exited!"); // Log when the note is missed
        }
    }
}
