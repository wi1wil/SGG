using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BuySignScript : MonoBehaviour
{
    public static bool isUniversityBought = false;

    [Header("Scripts")]
    [SerializeField] CurrencyManagerScript currencyScript;
    AudioManagerScript audioManager;    

    [Header("GameObjects")]
    [SerializeField] GameObject parentInEnvironment;
    [SerializeField] GameObject universityBuilding;
    [SerializeField] GameObject confirmationDialog;
    [SerializeField] GameObject buySignPrefab;

    [Header("Texts")]
    [SerializeField] TextMeshProUGUI signConfirmationText;
    [SerializeField] GameObject popUpText;

    [Header("Buttons")]
    [SerializeField] Button yesButton;
    [SerializeField] Button noButton;

    private Vector3 Offset = new Vector3(1, 0, 0);
    
    private void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
    }

    private void Start()
    {
        currencyScript = FindObjectOfType<CurrencyManagerScript>();

        // Add listeners to the buttons
        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);

        // Load university status on scene load
        LoadUniversityStatus();
    }

    public void buySign()
    {
        // Show the confirmation panel and activate the sign confirmation text
        signConfirmationText.gameObject.SetActive(true);
        confirmationDialog.SetActive(true);

        audioManager.PlaySfx(audioManager.yesButton);
    }

    private void OnYesButtonClicked()
    {
        audioManager.PlaySfx(audioManager.yesButton);
        // Check if the currency is enough
        if (currencyScript.currencyInGame >= currencyScript.buySignCost)
        {
            // Decrease the currency by the cost of the sign
            currencyScript.currencyInGame -= currencyScript.buySignCost;

            currencyScript.currencyPerSecText.gameObject.SetActive(true);
            currencyScript.moneyMultiplierText.gameObject.SetActive(true);
            currencyScript.hireTeacherUI.SetActive(true);
            currencyScript.enrollStudentsUI.SetActive(true);
            currencyScript.hireJanitorUI.SetActive(true);

            audioManager.PlaySfx(audioManager.buildingSFX);

            // Disable the buy sign
            buySignPrefab.SetActive(false);

            // Set the university building to active
            universityBuilding.SetActive(true);
            isUniversityBought = true;

            // Save the university status
            SaveUniversityStatus();
        }
        else
        {
            audioManager.PlaySfx(audioManager.noButton);
            // Instantiate the pop-up text as a child of parentInEnvironment
            var go = Instantiate(popUpText, transform.position, Quaternion.identity);
            go.transform.SetParent(parentInEnvironment.transform, false);
            go.GetComponent<TextMeshProUGUI>().text = "Not enough money!";
        }

        // Close the confirmation panel
        confirmationDialog.SetActive(false);
    }

    private void OnNoButtonClicked()
    {
        audioManager.PlaySfx(audioManager.noButton);
        // Close the confirmation panel
        confirmationDialog.SetActive(false);
    }

    private void CheckUniversityStatus()
    {
        if (isUniversityBought)
        {
            // Load all UI and TextGameObject
            currencyScript.currencyPerSecText.gameObject.SetActive(true);
            currencyScript.moneyMultiplierText.gameObject.SetActive(true);

            // if (SceneManager.GetActiveScene().name == "UniversityScene")
            // {
                currencyScript.hireTeacherUI.SetActive(true);
                currencyScript.enrollStudentsUI.SetActive(true);
            // }

            if(SceneManager.GetActiveScene().name == "GameplayScene" && CurrencyManagerScript.isJanitorHired != 1)
            {
                currencyScript.hireJanitorUI.SetActive(true);
            }

            // Deactivate the sign and activate the building
            buySignPrefab.SetActive(false);
            universityBuilding.SetActive(true);
        }
    }

    private void SaveUniversityStatus()
    {
        PlayerPrefs.SetInt("isUniversityBought", isUniversityBought ? 1 : 0);
        PlayerPrefs.Save();
    }

    private void LoadUniversityStatus()
    {
        isUniversityBought = PlayerPrefs.GetInt("isUniversityBought", 0) == 1;
        CheckUniversityStatus();
    }
}   