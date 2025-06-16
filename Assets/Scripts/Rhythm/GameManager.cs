using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component

    public AudioSource music;
    public bool isGameStarted;
    public BeatScroller beatScroller;

    public static GameManager instance;

    public int currentCombo; // Current combo count
    public float percentComplete; // Percentage of the game completed


    public TMP_Text scoreText; // UI Text to display the score
    public TMP_Text combo; // UI Text to display the combo

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this; // Initialize the singleton instance
    }

    // Update is called once per frame
    void Update()
    {
        if(!isGameStarted)
        {
            if (Input.anyKeyDown)
            {
                isGameStarted = true; // Set the flag to true when any key is pressed
                
                beatScroller.hasStarted = true; // Set the BeatScroller's hasStarted flag to true
                videoPlayer.Play(); // Start playing the video
                music.Play(); // Start playing the music
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Note hit!"); // Log when a note is hit
        currentCombo++; // Increase the combo count
    }
    public void NoteMissed()
    {
        Debug.Log("Note missed!"); // Log when a note is missed
    }
}
