using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    public Button start;
    public Button load;
    public Button quit;

    public void StartGame()
    {
        Debug.Log("Starting game...");
        SceneManager.LoadScene("LoadingScene");
    }
    public void LoadGame()
    {
        // Load game logic here
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
