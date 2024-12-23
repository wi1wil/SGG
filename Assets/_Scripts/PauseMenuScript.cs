using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class PauseMenuScript : MonoBehaviour
{
    CurrencyManagerScript currencyManager;

    [SerializeField] private GameObject pauseMenuUI;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider sfxSlider;
    [SerializeField] private Button exitButton;
    [SerializeField] private AudioMixer audioMixer;

    LevelLoaderScript levelLoader;
    AudioManagerScript audioManager;
    public bool GameIsPaused = false;

    private void Awake()
    {
        levelLoader = FindObjectOfType<LevelLoaderScript>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
        currencyManager = FindObjectOfType<CurrencyManagerScript>();
    }

    private void Start()
    {
        pauseMenuUI.SetActive(false);

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        sfxSlider.onValueChanged.AddListener(SetSFXVolume);
        exitButton.onClick.AddListener(ExitGame);

        if (PlayerPrefs.HasKey("MusicVolume") && PlayerPrefs.HasKey("SFXVolume"))
        {
            LoadVolume();
        }
        else
        {
            SetMusicVolume(musicSlider.value);
            SetSFXVolume(sfxSlider.value);
        }
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                Resume();
                audioManager.PlaySfx(audioManager.yesButton);
            }
            else
            {
                Pause();
                audioManager.PlaySfx(audioManager.noButton);
            }
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
    }

    void Pause()
    {
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        currencyManager.SaveData();
    }

    public void SetMusicVolume(float volume)
    {
        audioMixer.SetFloat("Music", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("MusicVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        audioMixer.SetFloat("SFX", Mathf.Log10(volume) * 20);
        PlayerPrefs.SetFloat("SFXVolume", volume);
    }

    private void LoadVolume()
    {
        musicSlider.value = PlayerPrefs.GetFloat("MusicVolume");
        sfxSlider.value = PlayerPrefs.GetFloat("SFXVolume");

        SetMusicVolume(musicSlider.value);
        SetSFXVolume(sfxSlider.value);
    }

    void ExitGame()
    {
        currencyManager.SaveData(); 
        audioManager.PlaySfx(audioManager.noButton);
        Time.timeScale = 1f;
        levelLoader.loadMenu();
    }
}