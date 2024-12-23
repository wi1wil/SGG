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

        confirmationPanel.SetActive(false);
    }

    private void OnBackButtonClicked()
    {
        audioManager.PlaySfx(audioManager.yesButton);
        confirmationPanel.SetActive(true);
        confirmationText.gameObject.SetActive(true);
    }

    private void OnYesButtonClicked()
    {
        audioManager.PlaySfx(audioManager.yesButton);
        levelLoader.LoadPrevLevel();
    }

    private void OnNoButtonClicked()
    {
        audioManager.PlaySfx(audioManager.noButton);
        confirmationPanel.SetActive(false);
    }
}