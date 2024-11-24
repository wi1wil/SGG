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

    [Header("UI Elements")]
    [SerializeField] private GameObject confirmationDialog;
    [SerializeField] private TextMeshProUGUI doorConfirmationText;
    [SerializeField] private TextMeshProUGUI signConfirmationText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    private void Awake()
    {
        levelLoader = FindObjectOfType<LevelLoaderScript>();

        // Add listeners to the buttons
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);
    }

    public void LoadUniversity()
    {
        // Show the confirmation panel and activate the door confirmation text
        doorConfirmationText.gameObject.SetActive(true);
        signConfirmationText.gameObject.SetActive(false);
        confirmationDialog.SetActive(true);
    }

    private void OnYesButtonClicked()
    {
        confirmationDialog.SetActive(false);
        // Load the university scene
        levelLoader.LoadNextLevel();
    }

    private void OnNoButtonClicked()
    {
        // Close the confirmation panel
        confirmationDialog.SetActive(false);
    }
}