using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BuyChairScript : MonoBehaviour
{
    CurrencyManagerScript currencyManager;
    AudioManagerScript audioManager;

    [Header("Chair Prefab")]
    [SerializeField] private GameObject[] chairPrefabs;

    [Header("Buy Next Prefab")]
    [SerializeField] private GameObject[] triggerPrefabs;

    [Header("Confirmation Panel")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private TextMeshProUGUI confirmationText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpText;
    [SerializeField] private GameObject parentInEnvironment;

    private void Start() {
        currencyManager = FindObjectOfType<CurrencyManagerScript>();
        LoadData();
    }

    private void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();

        yesButton.onClick.AddListener(ConfirmPurchase);
        noButton.onClick.AddListener(() => 
        {
            confirmationPanel.SetActive(false);
            audioManager.PlaySfx(audioManager.noButton);
        });
    }

    private void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) {
        if (scene.name == "UniversityScene") {
            LoadChairs();
        }
    }

    private void LoadChairs() {
        for (int i = 0; i < CurrencyManagerScript.ChairPrefabIndex; i++) {
            if (i < chairPrefabs.Length) {
                chairPrefabs[i].SetActive(true);
            }
        }

        if (CurrencyManagerScript.ChairPrefabIndex < triggerPrefabs.Length) {
            triggerPrefabs[CurrencyManagerScript.ChairPrefabIndex].SetActive(true);
            
            if(CurrencyManagerScript.ChairPrefabIndex > 0) {
                triggerPrefabs[0].SetActive(false);
            }
        }
    }

    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
        confirmationText.text = "-= Buy Chair =-\n\n" + "Cost: Rp. " + currencyManager.chairCost.ToString("N0") + "\n\n";
    }

    private void ConfirmPurchase()
    {
        if (currencyManager.currencyInGame >= currencyManager.chairCost)
        {
            audioManager.PlaySfx(audioManager.buildingSFX);

            currencyManager.currencyInGame -= currencyManager.chairCost;

            CurrencyManagerScript.chairAmount += 1;
            currencyManager.moneyMultiplier += 0.05; 

            if (CurrencyManagerScript.ChairPrefabIndex < triggerPrefabs.Length)
            {
                triggerPrefabs[CurrencyManagerScript.ChairPrefabIndex].SetActive(false);
            }

            if (CurrencyManagerScript.ChairPrefabIndex < chairPrefabs.Length)
            {
                chairPrefabs[CurrencyManagerScript.ChairPrefabIndex].SetActive(true);
            }

            CurrencyManagerScript.ChairPrefabIndex++;
            if (CurrencyManagerScript.ChairPrefabIndex < triggerPrefabs.Length)
            {
                triggerPrefabs[CurrencyManagerScript.ChairPrefabIndex].SetActive(true);
            }

            PlayerPrefs.SetInt("ChairPrefabIndex", CurrencyManagerScript.ChairPrefabIndex); 

            currencyManager.SaveData();
            currencyManager.UpdateCurrencyPerSecond(); 
            currencyManager.UpdateUI();

            SaveData();
        }
        else
        {
            audioManager.PlaySfx(audioManager.noButton);
            var popUp = Instantiate(popUpText, parentInEnvironment.transform.position, Quaternion.identity);
            popUp.transform.SetParent(parentInEnvironment.transform, false);
            var textComponent = popUp.GetComponent<TextMeshProUGUI>();
            textComponent.text = "Not enough money!";

            SaveData();
        }

        confirmationPanel.SetActive(false);
    }

    private void LoadData()
    {
        CurrencyManagerScript.ChairPrefabIndex = PlayerPrefs.GetInt("ChairPrefabIndex", 0);
        CurrencyManagerScript.chairAmount = PlayerPrefs.GetInt("ChairAmount", 0);
    }

    private void SaveData() {
        PlayerPrefs.SetInt("ChairPrefabIndex", CurrencyManagerScript.ChairPrefabIndex);
        PlayerPrefs.SetInt("ChairAmount", CurrencyManagerScript.chairAmount);
        PlayerPrefs.Save();
    }   
}