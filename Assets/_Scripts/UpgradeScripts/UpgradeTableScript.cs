using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using Unity.PlasticSCM.Editor.WebApi;
using JetBrains.Annotations;
using System;

public class UpgradeTableScript : MonoBehaviour
{
    BuyTableScript buyTableScript;
    CurrencyManagerScript currencyManager;
    AudioManagerScript audioManager;

    [Header("Triggers")]
    [SerializeField] private GameObject[] triggerLvlPrefabs;

    [Header("Lvl 2 Table Prefabs")]
    [SerializeField] private GameObject[] Lvl2TablePrefabs;

    [Header("Lvl 3 Table Prefabs")]
    [SerializeField] private GameObject[] Lvl3TablePrefabs;

    [Header("Confirmation Panel")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpText;
    [SerializeField] private GameObject parentInEnvironment;

    public int upgradeIndex = 1;

    private void Awake()
    {
        buyTableScript = FindObjectOfType<BuyTableScript>();
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
            LoadTables();
        }
    }

    public void UnlockUpgrades()
    {
        triggerLvlPrefabs[0].SetActive(true);
        SaveData();
    }

    private void LoadTables() {
        switch (upgradeIndex) {
            case 2:
                for(int i = 0; i < CurrencyManagerScript.Lvl2Table; i++) {
                    if (i < Lvl2TablePrefabs.Length) {
                        Lvl2TablePrefabs[i].SetActive(true);
                        buyTableScript.DisablePrefab(i);
                    }
                }
                break;
            case 3:
                for(int i = 0; i < CurrencyManagerScript.Lvl3Table; i++) {
                    if (i < Lvl3TablePrefabs.Length) {
                        Lvl3TablePrefabs[i].SetActive(true);
                        Lvl2TablePrefabs[i].SetActive(false);
                        buyTableScript.DisablePrefab(i);
                    }
                }
                break;
        }
        if (CurrencyManagerScript.UpgradeTableIndex < triggerLvlPrefabs.Length && CurrencyManagerScript.TablePrefabIndex == 14) {
            triggerLvlPrefabs[CurrencyManagerScript.UpgradeTableIndex].SetActive(true);

            if (CurrencyManagerScript.UpgradeTableIndex > 0) {
                triggerLvlPrefabs[0].SetActive(false);
            }
        }
    }

    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
        priceText.text = "-= Upgrade Table? =- \n\n" + "Cost: Rp. " + currencyManager.upgradeTableCost.ToString("N0") + "\n\n";
    }

    public void ConfirmUpgrade()
    {
        if (currencyManager.currencyInGame >= currencyManager.upgradeTableCost)
        {
            audioManager.PlaySfx(audioManager.buySuccessSFX);

            currencyManager.currencyInGame -= currencyManager.upgradeTableCost;

            buyTableScript.DisablePrefab(CurrencyManagerScript.UpgradeTableIndex);

            if(CurrencyManagerScript.UpgradeTableIndex < triggerLvlPrefabs.Length)
            {
                triggerLvlPrefabs[CurrencyManagerScript.UpgradeTableIndex].SetActive(false);
            }

            switch(upgradeIndex)
            {   
                case 2:
                    if(CurrencyManagerScript.UpgradeTableIndex < Lvl2TablePrefabs.Length)
                    {
                        Lvl2TablePrefabs[CurrencyManagerScript.UpgradeTableIndex].SetActive(true);
                    }
                    CurrencyManagerScript.Lvl2Table += 1;
                    currencyManager.moneyMultiplier += 0.2;

                    SaveData();
                    break;
                case 3:
                    if(CurrencyManagerScript.UpgradeTableIndex < Lvl2TablePrefabs.Length)
                    {
                        Lvl3TablePrefabs[CurrencyManagerScript.UpgradeTableIndex].SetActive(true);
                        Lvl2TablePrefabs[CurrencyManagerScript.UpgradeTableIndex].SetActive(false);
                    }
                    CurrencyManagerScript.Lvl3Table += 1;
                    currencyManager.moneyMultiplier += 0.3;

                    SaveData();
                    break;
            }

            CurrencyManagerScript.UpgradeTableIndex += 1;
            if(CurrencyManagerScript.UpgradeTableIndex < triggerLvlPrefabs.Length)
            {
                triggerLvlPrefabs[CurrencyManagerScript.UpgradeTableIndex].SetActive(true);
            }

            // Reusing the Upgrade Trigger Prefabs
            if(CurrencyManagerScript.UpgradeTableIndex == 14)
            {
                CurrencyManagerScript.UpgradeTableIndex = 0;

                UnlockUpgrades();

                currencyManager.upgradeTableCost *= 2;
                upgradeIndex += 1;

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
        CurrencyManagerScript.UpgradeTableIndex = PlayerPrefs.GetInt("UpgradeTableIndex", 0);
        CurrencyManagerScript.UpgradeChairIndex = PlayerPrefs.GetInt("UpgradeChairIndex", 0);
        currencyManager.upgradeTableCost = PlayerPrefs.GetFloat("UpgradeTableCost", 0);
        currencyManager.upgradeChairCost = PlayerPrefs.GetFloat("UpgradeChairCost", 0);
        CurrencyManagerScript.Lvl2Table = PlayerPrefs.GetInt("Lvl2Table", 0);
        CurrencyManagerScript.Lvl3Table = PlayerPrefs.GetInt("Lvl3Table", 0);
        CurrencyManagerScript.Lvl2Chair = PlayerPrefs.GetInt("Lvl2Chair", 0);
        CurrencyManagerScript.Lvl3Chair = PlayerPrefs.GetInt("Lvl3Chair", 0);
        upgradeIndex = PlayerPrefs.GetInt("TableLevelIndex", 1);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("UpgradeTableIndex", CurrencyManagerScript.UpgradeTableIndex);
        PlayerPrefs.SetInt("UpgradeChairIndex", CurrencyManagerScript.UpgradeChairIndex);
        PlayerPrefs.SetFloat("UpgradeChairCost", (float)currencyManager.upgradeChairCost);
        PlayerPrefs.SetFloat("UpgradeTableCost", (float)currencyManager.upgradeTableCost);
        PlayerPrefs.SetInt("Lvl2Table", CurrencyManagerScript.Lvl2Table);
        PlayerPrefs.SetInt("Lvl3Table", CurrencyManagerScript.Lvl3Table);
        PlayerPrefs.SetInt("Lvl2Chair", CurrencyManagerScript.Lvl2Chair);
        PlayerPrefs.SetInt("Lvl3Chair", CurrencyManagerScript.Lvl3Chair);
        PlayerPrefs.SetInt("TableLevelIndex", upgradeIndex);
        PlayerPrefs.Save();
    }
}
