using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettingsScript : MonoBehaviour
{
    AudioManagerScript audioManager;

    [Header("UI Elements")]
    [SerializeField] private GameObject optionsMenuUI;
    [SerializeField] private GameObject mainMenuUI;

    [Header("Audio Mixer")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button backButton;

    private void Start() 
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
        if(PlayerPrefs.HasKey("MusicVolume") && PlayerPrefs.HasKey("SFXVolume")) {
            LoadVolume();
        } else {
            SetMusicVolume();
            SetSFXVolume();
        }

        backButton.onClick.AddListener(() => {
            PlayerPrefs.Save();

            audioManager.PlaySfx(audioManager.noButton);
            // Load the main menu scene
            mainMenuUI.SetActive(true);
            optionsMenuUI.SetActive(false);
        });
    }

    public void SetMusicVolume() 
    {
        float musicVolume = musicSlider.value;
        audioMixer.SetFloat("Music", Mathf.Log10(musicVolume)*20);
        PlayerPrefs.SetFloat("MusicVolume", musicVolume);
    }

    public void SetSFXVolume() 
    {
        float sfxVolume = sfxSlider.value; 
        audioMixer.SetFloat("SFX", Mathf.Log10(sfxVolume)*20);
        PlayerPrefs.SetFloat("SFXVolume", sfxVolume);
    }

    private void LoadVolume() 
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        SetMusicVolume();
        SetSFXVolume();
    }
}
