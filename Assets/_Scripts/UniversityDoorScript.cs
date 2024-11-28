using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UniversityDoorScript : MonoBehaviour
{
    [Header("Scripts")]
    public LevelLoaderScript levelLoader;
    AudioManagerScript audioManager;

    [Header("UI Elements")]
    [SerializeField] private GameObject confirmationDialog;
    [SerializeField] private TextMeshProUGUI doorConfirmationText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private void Awake()
    {
        levelLoader = FindObjectOfType<LevelLoaderScript>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();

        // Add listeners to the buttons
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    public void LoadUniversity()
    {
        // Show the confirmation panel and activate the door confirmation text
        doorConfirmationText.gameObject.SetActive(true);
        confirmationDialog.SetActive(true);
    }

    private void OnYesButtonClicked()
    {
        confirmationDialog.SetActive(false);
        audioManager.PlaySfx(audioManager.yesButton);
        // Load the university scene
        levelLoader.LoadNextLevel();
    }

    private void OnNoButtonClicked()
    {
        audioManager.PlaySfx(audioManager.noButton);
        // Close the confirmation panel
        confirmationDialog.SetActive(false);
    }
}