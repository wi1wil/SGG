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

        yesButton.onClick.AddListener(OnYesButtonClicked);
        noButton.onClick.AddListener(OnNoButtonClicked);

        LoadUniversityStatus();
    }

    public void buySign()
    {
        signConfirmationText.gameObject.SetActive(true);
        confirmationDialog.SetActive(true);

        audioManager.PlaySfx(audioManager.yesButton);
    }

    private void OnYesButtonClicked()
    {
        audioManager.PlaySfx(audioManager.yesButton);
        if (currencyScript.currencyInGame >= currencyScript.buySignCost)
        {
            currencyScript.currencyInGame -= currencyScript.buySignCost;

            currencyScript.currencyPerSecText.gameObject.SetActive(true);
            currencyScript.moneyMultiplierText.gameObject.SetActive(true);
            currencyScript.hireTeacherUI.SetActive(true);
            currencyScript.enrollStudentsUI.SetActive(true);
            currencyScript.hireJanitorUI.SetActive(true);

            audioManager.PlaySfx(audioManager.buildingSFX);

            buySignPrefab.SetActive(false);

            universityBuilding.SetActive(true);
            isUniversityBought = true;

            SaveUniversityStatus();
        }
        else
        {
            audioManager.PlaySfx(audioManager.noButton);
            var go = Instantiate(popUpText, transform.position, Quaternion.identity);
            go.transform.SetParent(parentInEnvironment.transform, false);
            go.GetComponent<TextMeshProUGUI>().text = "Not enough money!";
        }

        confirmationDialog.SetActive(false);
    }

    private void OnNoButtonClicked()
    {
        audioManager.PlaySfx(audioManager.noButton);
        confirmationDialog.SetActive(false);
    }

    private void CheckUniversityStatus()
    {
        if (isUniversityBought)
        {
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