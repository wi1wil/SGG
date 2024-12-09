using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class BuyChairScript : MonoBehaviour
{
    UpgradeChairScript upgradeChairScript;
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

    private void Awake() 
    {
        upgradeChairScript = FindObjectOfType<UpgradeChairScript>();
        currencyManager = FindObjectOfType<CurrencyManagerScript>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
    
        LoadData();

        yesButton.onClick.AddListener(ConfirmPurchase);
        noButton.onClick.AddListener(() => 
        {
            confirmationPanel.SetActive(false);
            audioManager.PlaySfx(audioManager.noButton);
        });

        if (SceneManager.GetActiveScene().name == "UniversityScene") 
        {
            LoadChairs();
        }
    }

    private void LoadChairs() {
        for (int i = 0; i < CurrencyManagerScript.ChairPrefabIndex; i++) 
        {
            if (i < chairPrefabs.Length) {
                chairPrefabs[i].SetActive(true);
            }
        }

        if (CurrencyManagerScript.ChairPrefabIndex < triggerPrefabs.Length) 
        {
            triggerPrefabs[CurrencyManagerScript.ChairPrefabIndex].SetActive(true);
            
            if(CurrencyManagerScript.ChairPrefabIndex > 0) {
                triggerPrefabs[0].SetActive(false);
            }
        }

        if (CurrencyManagerScript.ChairPrefabIndex == 0 && CurrencyManagerScript.ChairPrefabIndex < triggerPrefabs.Length)
        {
            triggerPrefabs[0].SetActive(true);
        }
        else
        {
            triggerPrefabs[0].SetActive(false);
        }
    }

    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
        confirmationText.text = "-= Buy Chair? =-\n\n" + "Cost: Rp. " + currencyManager.chairCost.ToString("N0") + "\n\n";
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
            
            if (CurrencyManagerScript.TablePrefabIndex == 14)
            {
                upgradeChairScript.UnlockUpgrades();
                upgradeChairScript.upgradeIndex += 1;

                SaveData();
                upgradeChairScript.SaveData();
            }

            currencyManager.SaveData();
            currencyManager.UpdateUI();
            currencyManager.UpdateCurrencyPerSecond();

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

    public void DisablePrefab(int index)
    {   
        chairPrefabs[index].SetActive(false);
    }

    private void LoadData()
    {
        CurrencyManagerScript.ChairPrefabIndex = PlayerPrefs.GetInt("ChairPrefabIndex", 0);
        CurrencyManagerScript.chairAmount = PlayerPrefs.GetInt("ChairAmount", 0);
        upgradeChairScript.upgradeIndex = PlayerPrefs.GetInt("ChairLevelIndex", 0);
    }

    private void SaveData() {
        PlayerPrefs.SetInt("ChairPrefabIndex", CurrencyManagerScript.ChairPrefabIndex);
        PlayerPrefs.SetInt("ChairAmount", CurrencyManagerScript.chairAmount);
        PlayerPrefs.Save();
    }   
}