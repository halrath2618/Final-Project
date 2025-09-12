using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuInGame : MonoBehaviour
{
    public GameObject menuInGame;
    public GameObject confirmPanel;
    public Button resume;
    public Button main;
    public Button quit;
    public Button config;
    private bool _config = false;

    private bool _isBack = false;

    public void Confirmed()
    {
        confirmPanel.SetActive(false);
        SceneManager.LoadScene("Main Menu");
    }
    public void Refused()
    {
        confirmPanel.SetActive(false);
        _isBack = false;
    }

    public void OpenMenuInGame()
    {
        menuInGame.SetActive(true);
    }
    public void Clicked()
    {
        _config = true;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) || _config)
        {
            OpenMenuInGame();
        }
    }

    public void ResumeGame()
    {
        menuInGame.SetActive(false);
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
    public void MainMenu()
    {
        Debug.Log("mở bảng thông báo");
        confirmPanel.SetActive(true);
    }
}
