using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BuyTableScript : MonoBehaviour
{
    CurrencyManagerScript currencyManager;
    AudioManagerScript audioManager;

    [Header("Table Prefab")]
    [SerializeField] private GameObject[] tablePrefabs;

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
        currencyManager.UpdateCurrencyPerSecond();
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
            LoadTables();
        }
    }

    private void LoadTables() {
        for (int i = 0; i < CurrencyManagerScript.PrefabIndex; i++) {
            if (i < tablePrefabs.Length) {
                tablePrefabs[i].SetActive(true);
            }
        }

        if (CurrencyManagerScript.PrefabIndex < triggerPrefabs.Length) {
            triggerPrefabs[CurrencyManagerScript.PrefabIndex].SetActive(true);
            
            if(CurrencyManagerScript.PrefabIndex > 0) {
                triggerPrefabs[0].SetActive(false);
            }
        }
    }

    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
        confirmationText.text = "-= Buy Table =-\n\n" + "Cost: Rp. " + currencyManager.tableCost.ToString("N0") + "\n\n";
    }

    private void ConfirmPurchase()
    {
        if (currencyManager.currencyInGame >= currencyManager.tableCost)
        {
            audioManager.PlaySfx(audioManager.buildingSFX);

            currencyManager.currencyInGame -= currencyManager.tableCost;

            currencyManager.tableCost = currencyManager.tableCost * 1.5;

            CurrencyManagerScript.tableAmount += 1;

            if (CurrencyManagerScript.PrefabIndex < triggerPrefabs.Length)
            {
                triggerPrefabs[CurrencyManagerScript.PrefabIndex].SetActive(false);
            }

            if (CurrencyManagerScript.PrefabIndex < tablePrefabs.Length)
            {
                tablePrefabs[CurrencyManagerScript.PrefabIndex].SetActive(true);
            }

            CurrencyManagerScript.PrefabIndex++;
            if (CurrencyManagerScript.PrefabIndex < triggerPrefabs.Length)
            {
                triggerPrefabs[CurrencyManagerScript.PrefabIndex].SetActive(true);
            }

            PlayerPrefs.SetInt("PrefabIndex", CurrencyManagerScript.PrefabIndex); 

            currencyManager.SaveData();
            currencyManager.UpdateUI();
            currencyManager.UpdateCurrencyPerSecond();
        }
        else
        {
            audioManager.PlaySfx(audioManager.noButton);
            var go = Instantiate(popUpText, transform.position, Quaternion.identity);
            go.transform.SetParent(parentInEnvironment.transform, false);
            go.GetComponent<TextMeshProUGUI>().text = "Not enough money!";
        }

        confirmationPanel.SetActive(false);
    }

    private void LoadData()
    {
        CurrencyManagerScript.PrefabIndex = PlayerPrefs.GetInt("PrefabIndex", 0);
        CurrencyManagerScript.tableAmount = PlayerPrefs.GetInt("TableAmount", 0);
    }
}