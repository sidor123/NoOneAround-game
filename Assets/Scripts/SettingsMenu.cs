using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SettingsMenu : MonoBehaviour
{
    public GameObject settingsPanel;
    public Slider volumeSlider;
    public TMP_Dropdown difficultyDropdown;
    public Toggle guideToggle;

    private void Start()
    {
        volumeSlider.value = PlayerPrefs.GetFloat("Volume", 50);
        difficultyDropdown.value = PlayerPrefs.GetInt("Difficulty", 0);;
        guideToggle.isOn = PlayerPrefs.GetInt("Guide", 0) == 1;

        AudioListener.volume = volumeSlider.value / 100f;
        Screen.fullScreen = guideToggle.isOn;
    }

    public void SetVolume() {
        float volume = volumeSlider.value;
        AudioListener.volume = volume / 100f;
        PlayerPrefs.SetFloat("Volume", volume);
    }

    public void SetDifficulty() {
        int difficulty = difficultyDropdown.value;
        PlayerPrefs.SetInt("Difficulty", difficulty);
    }

    public void SetGuide() {
        bool isGuideDisabled = guideToggle.isOn;
        PlayerPrefs.SetInt("Guide", isGuideDisabled ? 1 : 0);
    }

    public void OpenSettings()
    {
        settingsPanel.SetActive(true);
    }

    public void CloseSettings()
    {
        settingsPanel.SetActive(false);
    }
}