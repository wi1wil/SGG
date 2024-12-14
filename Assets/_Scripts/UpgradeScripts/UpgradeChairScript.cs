using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class UpgradeChairScript : MonoBehaviour
{
    BuyChairScript buyChairScript;
    CurrencyManagerScript currencyManager;
    AudioManagerScript audioManager;

    [Header("Triggers")]
    [SerializeField] private GameObject[] triggerPrefabs;

    [Header("Lvl 2 Chair Prefabs")]
    [SerializeField] private GameObject[] Lvl2ChairPrefabs;

    [Header("Lvl 3 Chair Prefabs")]
    [SerializeField] private GameObject[] Lvl3ChairPrefabs;

    [Header("Confirmation Panel")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpText;
    [SerializeField] private GameObject parentInEnvironment;

    public int upgradeChairIndex = 1;

    private void Awake()
    {
        buyChairScript = FindObjectOfType<BuyChairScript>();
        currencyManager = FindObjectOfType<CurrencyManagerScript>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();

        LoadData();
        
        yesButton.onClick.AddListener(ConfirmUpgrade);
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

    public void UnlockUpgrades()
    {
        triggerPrefabs[0].SetActive(true);
        SaveData();
    }

    private void LoadChairs() {
        switch (upgradeChairIndex) {
            case 2:
                for(int i = 0; i < CurrencyManagerScript.Lvl2Chair; i++) {
                    if (i < Lvl2ChairPrefabs.Length) {
                        Lvl2ChairPrefabs[i].SetActive(true);
                        buyChairScript.DisablePrefab(i);
                    }
                }
                break;
            case 3:
                for(int i = 0; i < CurrencyManagerScript.Lvl3Chair; i++) {
                    if (i < Lvl3ChairPrefabs.Length) {
                        Lvl3ChairPrefabs[i].SetActive(true);
                        Lvl2ChairPrefabs[i].SetActive(false);
                        buyChairScript.DisablePrefab(i);
                    }
                }
                break;
        }
        if (CurrencyManagerScript.UpgradeChairIndex < triggerPrefabs.Length && CurrencyManagerScript.ChairPrefabIndex == 14) {
            triggerPrefabs[CurrencyManagerScript.UpgradeChairIndex].SetActive(true);

            if (CurrencyManagerScript.UpgradeChairIndex > 0) {
                triggerPrefabs[0].SetActive(false);
            }
        }
    }

    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
        priceText.text = "-= Upgrade Chair? =- \n\n" + "Cost: Rp. " + currencyManager.upgradeChairCost.ToString("N0") + "\n\n";
    }

    public void ConfirmUpgrade()
    {
        if (currencyManager.currencyInGame >= currencyManager.upgradeChairCost)
        {
            audioManager.PlaySfx(audioManager.buySuccessSFX);

            currencyManager.currencyInGame -= currencyManager.upgradeChairCost;

            buyChairScript.DisablePrefab(CurrencyManagerScript.UpgradeChairIndex);

            if(CurrencyManagerScript.UpgradeChairIndex < triggerPrefabs.Length)
            {
                triggerPrefabs[CurrencyManagerScript.UpgradeChairIndex].SetActive(false);
            }

            switch(upgradeChairIndex)
            {   
                case 2:
                    if(CurrencyManagerScript.UpgradeChairIndex < Lvl2ChairPrefabs.Length)
                    {
                        Lvl2ChairPrefabs[CurrencyManagerScript.UpgradeChairIndex].SetActive(true);
                    }
                    CurrencyManagerScript.Lvl2Chair += 1;
                    currencyManager.moneyMultiplier += 0.2;

                    SaveData();
                    break;
                case 3:
                    if(CurrencyManagerScript.UpgradeChairIndex < Lvl2ChairPrefabs.Length)
                    {
                        Lvl3ChairPrefabs[CurrencyManagerScript.UpgradeChairIndex].SetActive(true);
                        Lvl2ChairPrefabs[CurrencyManagerScript.UpgradeChairIndex].SetActive(false);
                    }
                    CurrencyManagerScript.Lvl3Chair += 1;
                    currencyManager.moneyMultiplier += 0.3;

                    SaveData();
                    break;
            }

            CurrencyManagerScript.UpgradeChairIndex += 1;
            if(CurrencyManagerScript.UpgradeChairIndex < triggerPrefabs.Length)
            {
                triggerPrefabs[CurrencyManagerScript.UpgradeChairIndex].SetActive(true);
            }

            // Reusing the Upgrade Trigger Prefabs
            if(CurrencyManagerScript.UpgradeChairIndex == 14)
            {
                CurrencyManagerScript.UpgradeChairIndex = 0;

                UnlockUpgrades();

                currencyManager.upgradeChairCost *= 2;
                upgradeChairIndex += 1;

                SaveData();
            }

            confirmationPanel.SetActive(false);

            SaveData();
            currencyManager.SaveData();
            currencyManager.UpdateUI();
            currencyManager.UpdateCurrencyPerSecond();
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
    }

    private void LoadData()
    {
        CurrencyManagerScript.UpgradeChairIndex = PlayerPrefs.GetInt("UpgradeChairIndex", 0);
        currencyManager.upgradeChairCost = PlayerPrefs.GetFloat("UpgradeChairCost", 0);
        CurrencyManagerScript.Lvl2Chair = PlayerPrefs.GetInt("Lvl2Chair", 0);
        CurrencyManagerScript.Lvl3Chair = PlayerPrefs.GetInt("Lvl3Chair", 0);
        upgradeChairIndex = PlayerPrefs.GetInt("ChairLevelIndex", 1);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("UpgradeChairIndex", CurrencyManagerScript.UpgradeChairIndex);
        PlayerPrefs.SetFloat("UpgradeChairCost", (float)currencyManager.upgradeChairCost);
        PlayerPrefs.SetInt("Lvl2Chair", CurrencyManagerScript.Lvl2Chair);
        PlayerPrefs.SetInt("Lvl3Chair", CurrencyManagerScript.Lvl3Chair);
        PlayerPrefs.SetInt("ChairLevelIndex", upgradeChairIndex);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}
