using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("UI References")]
    public Scrollbar volumeSlider;
    public Scrollbar opacitySlider;
    public CanvasGroup settingsGroup;

    [Header("Audio")]
    public AudioMixer masterMixer;
    private const string VOL_PARAM = "masterVol";

    void Start()
    {
        // Load saved prefs or set defaults
        float vol = PlayerPrefs.GetFloat("Volume", 0f);
        float op = PlayerPrefs.GetFloat("UIOpacity", 1f);

        volumeSlider.value = vol;
        opacitySlider.value = op;

        ApplyVolume(vol);
        ApplyOpacity(op);

        // Hook up callbacks
        volumeSlider.onValueChanged.AddListener(ApplyVolume);
        opacitySlider.onValueChanged.AddListener(ApplyOpacity);
    }

    public void ApplyVolume(float sliderValue)
    {
        // sliderValue should range from -80 (mute) to 0 (full)
        masterMixer.SetFloat(VOL_PARAM, sliderValue);
        PlayerPrefs.SetFloat("Volume", sliderValue);
    }

    public void ApplyOpacity(float alpha)
    {
        settingsGroup.alpha = alpha;
        PlayerPrefs.SetFloat("UIOpacity", alpha);
    }
}