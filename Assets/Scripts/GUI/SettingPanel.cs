using UnityEngine;

public class SettingPanel : MonoBehaviour
{
    [SerializeField] private GameObject settingPanel;
    [SerializeField] private GameObject graphics;
    [SerializeField] private GameObject audioSetting;
    [SerializeField] private GameObject uiControl;
    [SerializeField] private GameObject mainMenu;

    public void OpenSettingPanel()
    {
        settingPanel.SetActive(true);
        mainMenu.SetActive(false);
    }
    public void CloseSettingPanel()
    {
        settingPanel.SetActive(false);
        mainMenu.SetActive(true);
        graphics.SetActive(false);
        audioSetting.SetActive(false);
        uiControl.SetActive(false);
    }
    public void OpenGraphicsSetting()
    {
        graphics.SetActive(true);
        audioSetting.SetActive(false);
        uiControl.SetActive(false);
    }
    public void OpenAudioSetting()
    {
        graphics.SetActive(false);
        audioSetting.SetActive(true);
        uiControl.SetActive(false);
    }
    public void OpenUIControl()
    {
        graphics.SetActive(false);
        audioSetting.SetActive(false);
        uiControl.SetActive(true);
    }

}
