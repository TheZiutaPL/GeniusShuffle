using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Audio;
using UnityEngine.SceneManagement;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] bool saveOnChange = true;

    [Header("Window settings")]
    [SerializeField] private MenuList resolutionList;
    [SerializeField] private MenuToggle fullscreenToggle;

    [Header("Audio settings")]
    [SerializeField] private MenuSlider masterVolumeSlider;
    [SerializeField] private MenuSlider musicVolumeSlider;
    [SerializeField] private MenuSlider sfxVolumeSlider;
    [SerializeField] private AudioMixer audioMixer;
    private float[] savedValues; // This is needed to restore previous settings when current ones are not applied

    private void Start()
    {
        // Window settings
        if (fullscreenToggle != null)
        {
            fullscreenToggle.SetStatus(Screen.fullScreen);
            if (saveOnChange)
                fullscreenToggle.onValueChanged.AddListener(_ => SaveChanges()); 
                // This is done in such a weird way so it accepts a function with different parameters
        }

        if (resolutionList != null)
        {
            LoadResolutions();
            if (saveOnChange)
                resolutionList.onValueChanged.AddListener(_ => SaveChanges());
        }

        // Audio settings
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.onValueChanged.AddListener(ChangeMasterVolume);
            masterVolumeSlider.minValue = 0.001f;

            musicVolumeSlider.onValueChanged.AddListener(ChangeMusicVolume);
            musicVolumeSlider.minValue = 0.001f;

            sfxVolumeSlider.onValueChanged.AddListener(ChangeSFXVolume);
            sfxVolumeSlider.minValue = 0.001f;

            masterVolumeSlider.SetValue(PlayerPrefs.GetFloat(AudioManager.MASTER_KEY, 1f), false);
            musicVolumeSlider.SetValue(PlayerPrefs.GetFloat(AudioManager.MUSIC_KEY, 1f), false);
            sfxVolumeSlider.SetValue(PlayerPrefs.GetFloat(AudioManager.SFX_KEY, 1f), false);

            savedValues = new float[3] { masterVolumeSlider.value, musicVolumeSlider.value, sfxVolumeSlider.value };

            if (saveOnChange)
            {
                masterVolumeSlider.onValueChanged.AddListener(_ => SaveChanges());
                musicVolumeSlider.onValueChanged.AddListener(_ => SaveChanges());
                sfxVolumeSlider.onValueChanged.AddListener(_ => SaveChanges());
            }
        }
    }

    public void SaveChanges()
    {
        // Window settings
        if (resolutionList != null)
            SetResolution(resolutionList.currentOption);
        if (fullscreenToggle != null)
            SetFullscreen(fullscreenToggle.isOn);

        // Audio settings
        if (masterVolumeSlider != null)
        {
            savedValues = new float[3] { masterVolumeSlider.value, musicVolumeSlider.value, sfxVolumeSlider.value };
            PlayerPrefs.SetFloat(AudioManager.MASTER_KEY, masterVolumeSlider.value);
            PlayerPrefs.SetFloat(AudioManager.MUSIC_KEY, musicVolumeSlider.value);
            PlayerPrefs.SetFloat(AudioManager.SFX_KEY, sfxVolumeSlider.value);
        }
    }

    public void RestorePreviousSettings()
    {
        // Window settings
        if (resolutionList != null) 
        { 
            string currentResolution = Screen.width + "x" + Screen.height;
            resolutionList.currentOption = resolutionList.allOptions.FindIndex(option => option == currentResolution);
        }

        if (fullscreenToggle != null)
            fullscreenToggle.SetStatus(Screen.fullScreen);

        // Audio settings
        if (masterVolumeSlider != null)
        {
            masterVolumeSlider.value = savedValues[0];
            musicVolumeSlider.value = savedValues[1];
            sfxVolumeSlider.value = savedValues[2];
        }
    }

    #region Window settings
    private void LoadResolutions()
    {
        resolutionList.Clear();

        Resolution[] resolutions = Screen.resolutions; // List of all, unfiltered resolution options
        List<string> resolutionOptions = new List<string>(); // List of only unique resolutions, will later be added to the dropdown

        // Loop through all possible resolutions and move all unique ones to resolutionOptions
        int currentResolutionIndex = 0;
        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + "x" + resolutions[i].height;
            if (!resolutionOptions.Contains(option))
            {
                resolutionOptions.Add(option);

                // Find the index of current windows resolution
                if (option == Screen.width + "x" + Screen.height)
                    currentResolutionIndex = resolutionOptions.IndexOf(option);
            }
        }

        resolutionList.AddOptions(resolutionOptions);
        resolutionList.SetOption(currentResolutionIndex);
    }

    private void SetFullscreen(bool value)
    {
        Screen.fullScreen = value;
    }

    private void SetResolution(int resolutionIndex)
    {
        string[] resolution = resolutionList.allOptions[resolutionIndex].Split('x');
        Screen.SetResolution(int.Parse(resolution[0]), int.Parse(resolution[1]), Screen.fullScreen);
    }
    #endregion

    #region Audio settings
    private void ChangeMasterVolume(float volume) => audioMixer.SetFloat(AudioManager.MASTER_KEY, Mathf.Log10(volume) * 20);
    private void ChangeMusicVolume(float volume) => audioMixer.SetFloat(AudioManager.MUSIC_KEY, Mathf.Log10(volume) * 20);
    private void ChangeSFXVolume(float volume) => audioMixer.SetFloat(AudioManager.SFX_KEY, Mathf.Log10(volume) * 20);
    #endregion

    public void Quit()
    {
        Application.Quit();
    }
}
