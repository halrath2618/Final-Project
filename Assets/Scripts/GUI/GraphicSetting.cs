using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScreenSettingsManager : MonoBehaviour
{
    [Header("UI References")]
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;

    [Header("Available Resolutions")]
    public List<Vector2Int> presetResolutions = new List<Vector2Int>()
    {
        new Vector2Int(1024, 768),
        new Vector2Int(1366, 720),
        new Vector2Int(1920, 1080)
    };

    private bool _isFullscreen;
    private int _currentResolutionIndex;

    void Start()
    {
        // Initialize UI elements
        if (fullscreenToggle != null)
        {
            _isFullscreen = Screen.fullScreen;
            fullscreenToggle.isOn = _isFullscreen;
            fullscreenToggle.onValueChanged.AddListener(ToggleFullscreen);
        }

        if (resolutionDropdown != null)
        {
            // Populate dropdown with resolution options
            resolutionDropdown.ClearOptions();

            List<string> options = new List<string>();
            foreach (var res in presetResolutions)
            {
                options.Add($"{res.x} × {res.y}");
            }

            resolutionDropdown.AddOptions(options);

            // Set current resolution
            _currentResolutionIndex = FindCurrentResolutionIndex();
            resolutionDropdown.value = _currentResolutionIndex;
            resolutionDropdown.onValueChanged.AddListener(ChangeResolution);
        }
    }

    private int FindCurrentResolutionIndex()
    {
        for (int i = 0; i < presetResolutions.Count; i++)
        {
            if (Screen.width == presetResolutions[i].x &&
                Screen.height == presetResolutions[i].y)
            {
                return i;
            }
        }
        return 0; // Default to first resolution if not found
    }

    public void ToggleFullscreen(bool isFullscreen)
    {
        _isFullscreen = isFullscreen;
        Screen.fullScreen = _isFullscreen;

        // Adjust resolution when changing fullscreen mode
        if (!_isFullscreen)
        {
            ApplyResolution(_currentResolutionIndex);
        }
        else
        {
            // In fullscreen, use native desktop resolution
            Screen.SetResolution(
                Screen.currentResolution.width,
                Screen.currentResolution.height,
                true
            );
        }
    }

    public void ChangeResolution(int index)
    {
        _currentResolutionIndex = index;
        ApplyResolution(index);
    }

    private void ApplyResolution(int index)
    {
        if (index >= 0 && index < presetResolutions.Count)
        {
            Screen.SetResolution(
                presetResolutions[index].x,
                presetResolutions[index].y,
                _isFullscreen
            );
        }
    }

    // For standalone keyboard shortcuts (add to Update())
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F11))
        {
            ToggleFullscreen(!Screen.fullScreen);
        }
    }
}