using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BackButtonScript : MonoBehaviour
{
    [Header("Back Button")]
    [SerializeField] Button backButton;

    [Header("Confirmation Panel")]
    [SerializeField] GameObject confirmationPanel;
    [SerializeField] TextMeshProUGUI confirmationText;
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    private LevelLoaderScript levelLoader;
    AudioManagerScript audioManager;

    private void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
    }

    private void Start()
    {
        levelLoader = FindObjectOfType<LevelLoaderScript>();

        backButton.onClick.AddListener(OnBackButtonClicked);
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);

        // Initially hide the confirmation panel
        confirmationPanel.SetActive(false);
    }

    private void OnBackButtonClicked()
    {
        audioManager.PlaySfx(audioManager.yesButton);
        // Show the confirmation panel
        confirmationPanel.SetActive(true);
        confirmationText.gameObject.SetActive(true);
    }

    private void OnYesButtonClicked()
    {
        audioManager.PlaySfx(audioManager.yesButton);
        // Load the previous level
        levelLoader.LoadPrevLevel();
    }

    private void OnNoButtonClicked()
    {
        audioManager.PlaySfx(audioManager.noButton);
        // Close the confirmation panel
        confirmationPanel.SetActive(false);
    }
}