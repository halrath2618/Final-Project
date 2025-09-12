using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class GameManager : MonoBehaviour
{
    public VideoPlayer videoPlayer; // Reference to the VideoPlayer component

    public AudioSource music;
    public bool isGameStarted;

    public NoteController L_noteController; // Reference to the NoteController script
    public NoteController R_noteController; // Reference to the NoteController script
    public NoteController D_noteController;
    public NoteController U_noteController;

    public static GameManager instance;

    public int currentCombo; // Current combo count
    public float percentComplete; // Percentage of the game completed

    public TMP_Text currentComboText; // UI Text to display the current combo
    public TMP_Text currentPercentCompleteText; // UI Text to display the current percentage complete
    public Slider hp; // Slider to represent health points
    public float maxHp = 100f; // Maximum health points
    public float currentHp; // Current health points
    public float hpDecreaseRate = 10f; // Rate at which health points decrease
    public float hpIncreaseRate = 2f; // Rate at which health points increase
    public double maxPercentComplete = 100f; // Maximum percentage complete
    public double currentPercentComplete = 100f; // Current percentage complete

    public float maxCombo; // Maximum combo value


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        instance = this; // Initialize the singleton instance
    }

    // Update is called once per frame
    void Update()
    {
        UpdateHPBar(); // Update the health bar UI
        currentComboText.text = currentCombo.ToString(); // Update the combo text
        if (!isGameStarted)
        {
            if (Input.anyKeyDown)
            {
                isGameStarted = true; // Set the flag to true when any key is pressed
                L_noteController.hasStarted = true; // Start the left note controller
                R_noteController.hasStarted = true; // Start the right note controller
                D_noteController.hasStarted = true; // Start the down note controller
                U_noteController.hasStarted = true; // Start the up note controller
                videoPlayer.Play(); // Start playing the video
                music.Play(); // Start playing the music
            }
        }
    }

    public void NoteHit()
    {
        Debug.Log("Note hit!"); // Log when a note is hit

        currentHp += hpIncreaseRate; // Increase health points
        if (currentHp > maxHp) // Ensure health does not exceed maximum
        {
            currentHp = maxHp;
        }
        currentPercentComplete += Math.Round(maxPercentComplete / maxCombo, 2); // Increase the percentage complete based on the combo
        if (currentPercentComplete > maxPercentComplete) // Ensure percentage does not exceed maximum
        {
            currentPercentComplete = maxPercentComplete;
        }
        currentPercentCompleteText.text = currentPercentComplete.ToString() + "%"; // Update the percentage complete text
        currentCombo++; // Increase the combo count

    }
    public void NoteMissed()
    {
        Debug.Log("Note missed!"); // Log when a note is missed
        currentCombo = 0; // Reset the combo count
        currentHp -= hpDecreaseRate; // Decrease health points
        if (currentHp < 0) // Ensure health does not go below zero
        {
            currentHp = 0;
        }
        currentComboText.text = currentCombo.ToString(); // Update the combo text
    }

    public void UpdateHPBar()
    {
        hp.maxValue = maxHp; // Set the maximum value of the health bar
        hp.value = currentHp; // Set the current value of the health bar
    }
}
