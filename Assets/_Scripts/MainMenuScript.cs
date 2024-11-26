using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    AudioManagerScript audioManager;
    LevelLoaderScript levelLoader;

    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button optionsButton;

    public void Awake() 
    {
        // Playing SFX when the buttons are clicked
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
        levelLoader = FindObjectOfType<LevelLoaderScript>();

        // Adding listeners to the Start, Quit, and Options Buttons
        startButton.onClick.AddListener(() => {
            audioManager.PlaySfx(audioManager.yesButton);
            Debug.Log("Starting Game....");
            levelLoader.LoadNextLevel();
        });

        quitButton.onClick.AddListener(() => {
            audioManager.PlaySfx(audioManager.noButton);
            Application.Quit();
            PlayerPrefs.DeleteAll();
            Debug.Log("Quitting Game....");
        });

        optionsButton.onClick.AddListener(() => {
            audioManager.PlaySfx(audioManager.yesButton);
            Debug.Log("Opening Options Menu....");
        });
    }
}