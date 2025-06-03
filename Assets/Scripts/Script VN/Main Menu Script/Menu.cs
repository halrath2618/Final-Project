using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.IO;

public class Menu : MonoBehaviour
{
    public Button startButton;
    public Button loadButton;
    public Button settingButton;
    public Button libraryButton;
    public Button quitButton;
    public GameObject alert;
    public Button yes;
    public Button no;
    private int t = 5;

    public void Start()
    {
        
    }
    public void StartNewGame()
    {
        string path = Application.persistentDataPath + "/player.save";
        if (File.Exists(path))
        {
            alert.SetActive(true);
            if (t == 1)
            {
                // Reset the game state if starting a new game
                PlayerData data = new PlayerData(0);// Assuming 0 is the initial story progress
                SaveSystem.SaveGame(data);

                // Load the main game scene
                SceneManager.LoadScene("Loading");
            }
            else if (t == 0)
            {
                alert.SetActive(false);
            }
        }
        else
            SceneManager.LoadScene("Loading");
        
    }
    public void LoadSavedGame()
    {
        PlayerData data = SaveSystem.LoadGame(); 
        if (data != null)
        { 
            // Load the main game scene and continue from the saved state
            SceneManager.LoadScene("Loading");
        }
        else 
        { 
            Debug.LogWarning("No save file found. Starting a new game."); 
            StartNewGame(); 
        }
    }
    public void Yes()
    {
        t = 1;
        alert.SetActive(false);
        StartNewGame();
    }
    public void No()
    {
        t = 0;
        alert.SetActive(false);
        StartNewGame();
    }

    public void QuitToDesktop()
    {
        Debug.Log("Game is exiting...");
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else 
        Application.Quit();
        
        #endif
    }
}
