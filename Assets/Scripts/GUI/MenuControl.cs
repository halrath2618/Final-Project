using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    public GameObject menuCanvas;
    public PlayerController playerController;
    public CameraSetting cameraSetting;

    private bool _isMenuOpen = false;
    private bool _wasCursorLocked;
    private CursorLockMode _previousCursorState;

    void Start()
    {
        menuCanvas.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    void Update()
    {
        if (Keyboard.current.escapeKey.wasPressedThisFrame)
        {
            ToggleMenu();
        }
    }

    public void ToggleMenu()
    {
        _isMenuOpen = !_isMenuOpen;

        if (_isMenuOpen)
        {
            // Save current cursor state
            _wasCursorLocked = Cursor.lockState == CursorLockMode.Locked;
            _previousCursorState = Cursor.lockState;

            // Pause game
            Time.timeScale = 0f;
            menuCanvas.SetActive(true);

            // Enable cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            // Disable player and camera controls
            if (playerController) playerController.enabled = false;
            if (cameraSetting) cameraSetting.enabled = false;
        }
        else
        {
            // Resume game
            Time.timeScale = 1f;
            menuCanvas.SetActive(false);

            // Restore cursor state
            Cursor.lockState = _wasCursorLocked ? CursorLockMode.Locked : _previousCursorState;
            Cursor.visible = !_wasCursorLocked;

            // Enable player and camera controls
            if (playerController) playerController.enabled = true;
            if (cameraSetting) cameraSetting.enabled = true;
        }
    }
    public void ExitToMenu()
    {
        SceneManager.LoadScene("Main Menu");
    }
    public void ResumeGame()
    {
        if (_isMenuOpen) ToggleMenu();
    }

    public void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
    }
}