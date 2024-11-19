using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenuUIScript : MonoBehaviour
{
    AudioManagerScript audioManager;

    [SerializeField] private Button startButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button optionsButton;

    public void Awake() {
        // Playing SFX when the buttons are clicked
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();

        // Adding listeners to the Start, Quit, and Options Buttons
        startButton.onClick.AddListener(() => {
            audioManager.PlaySfx(audioManager.buttonClick);
            Debug.Log("Starting Game....");
            SceneManager.LoadScene(1);
        });

        quitButton.onClick.AddListener(() => {
            audioManager.PlaySfx(audioManager.buttonClick);
            Application.Quit();
            Debug.Log("Quitting Game....");
        });

        optionsButton.onClick.AddListener(() => {
            audioManager.PlaySfx(audioManager.buttonClick);
            Debug.Log("Opening Options Menu....");
        });
    }
}