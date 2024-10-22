using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using TMPro;

public class SettingsMenuController : MonoBehaviour
{
    [Header("Menu Panels")]
    [SerializeField] private GameObject settingsMenuPanel;
    [SerializeField] private GameObject audioPanel;
    [SerializeField] private GameObject controlsPanel; // For future controls settings
    [SerializeField] private GameObject graphicsPanel; // For future graphics settings

    [Header("Audio Settings")]
    [SerializeField] private Slider bgmSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private TextMeshProUGUI bgmValueText;
    [SerializeField] private TextMeshProUGUI sfxValueText;

    private void Start()
    {
        // Initialize with audio panel visible, others hidden
        ShowAudioPanel();

        // Set initial values if SoundManager exists
        if (SoundManager.Instance != null)
        {
            bgmSlider.value = SoundManager.Instance.GetMusicVolume();
            sfxSlider.value = SoundManager.Instance.GetSFXVolume();
            UpdateVolumeTexts();
        }

        // Add listeners for slider changes
        bgmSlider.onValueChanged.AddListener(OnBGMVolumeChanged);
        sfxSlider.onValueChanged.AddListener(OnSFXVolumeChanged);
    }

    #region Menu Controls
    public void OpenSettingsMenu()
    {
        settingsMenuPanel.SetActive(true);
        ShowAudioPanel(); // Default to audio panel when opening settings
        Time.timeScale = 0; // Pause the game
    }

    public void CloseSettingsMenu()
    {
        settingsMenuPanel.SetActive(false);
        Time.timeScale = 1; // Resume the game
    }

    public void ToggleSettingsMenu()
    {
        if (settingsMenuPanel.activeSelf)
        {
            CloseSettingsMenu();
        }
        else
        {
            OpenSettingsMenu();
        }
    }

    // Panel Navigation
    public void ShowAudioPanel()
    {
        audioPanel.SetActive(true);
        if (controlsPanel != null) controlsPanel.SetActive(false);
        if (graphicsPanel != null) graphicsPanel.SetActive(false);
    }

    public void ShowControlsPanel()
    {
        if (controlsPanel != null) controlsPanel.SetActive(true);
        audioPanel.SetActive(false);
        if (graphicsPanel != null) graphicsPanel.SetActive(false);
    }

    public void ShowGraphicsPanel()
    {
        if (graphicsPanel != null) graphicsPanel.SetActive(true);
        audioPanel.SetActive(false);
        if (controlsPanel != null) controlsPanel.SetActive(false);
    }
    #endregion

    #region Audio Settings
    private void OnBGMVolumeChanged(float value)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetMusicVolume(value);
            UpdateVolumeTexts();
            SaveSettings();
        }
    }

    private void OnSFXVolumeChanged(float value)
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.SetSFXVolume(value);
            UpdateVolumeTexts();
            SaveSettings();
        }
    }

    private void UpdateVolumeTexts()
    {
        if (bgmValueText != null)
        {
            bgmValueText.text = $"{(bgmSlider.value * 100):F0}%";
        }

        if (sfxValueText != null)
        {
            sfxValueText.text = $"{(sfxSlider.value * 100):F0}%";
        }
    }
    #endregion

    #region Settings Save/Load
    private void SaveSettings()
    {
        PlayerPrefs.SetFloat("BGMVolume", bgmSlider.value);
        PlayerPrefs.SetFloat("SFXVolume", sfxSlider.value);
        PlayerPrefs.Save();
    }

    private void LoadSettings()
    {
        if (PlayerPrefs.HasKey("BGMVolume"))
        {
            float bgmVolume = PlayerPrefs.GetFloat("BGMVolume");
            bgmSlider.value = bgmVolume;
            OnBGMVolumeChanged(bgmVolume);
        }

        if (PlayerPrefs.HasKey("SFXVolume"))
        {
            float sfxVolume = PlayerPrefs.GetFloat("SFXVolume");
            sfxSlider.value = sfxVolume;
            OnSFXVolumeChanged(sfxVolume);
        }
    }
    #endregion
}