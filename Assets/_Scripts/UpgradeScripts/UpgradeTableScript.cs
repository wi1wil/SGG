using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.InputSystem.UI;

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

    public static int currentTableLevel = 1;

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

        SceneManager.sceneUnloaded += OnSceneUnloaded;
    }


    public void UnlockUpgrades()
    {
        triggerLvlPrefabs[0].SetActive(true);
        SaveData();
    }

    private void LoadTables() {
        switch (currentTableLevel) {
            case 2:
                for(int i = 0; i < CurrencyManagerScript.Lvl2Table; i++) {
                    if (i < Lvl2TablePrefabs.Length) {
                        Lvl2TablePrefabs[i].SetActive(true);
                        buyTableScript.DisablePrefab(i);
                    }
                }
                break;
            case 3:
                for(int i = 0; i < CurrencyManagerScript.Lvl2Table; i++) {
                    if (i < Lvl2TablePrefabs.Length) {
                        Lvl2TablePrefabs[i].SetActive(true);
                        buyTableScript.DisablePrefab(i);
                    }
                }
                for(int i = 0; i < CurrencyManagerScript.Lvl3Table; i++) {
                    if (i < Lvl3TablePrefabs.Length) {
                        Lvl3TablePrefabs[i].SetActive(true);
                        Lvl2TablePrefabs[i].SetActive(false);
                    }
                }
                break;
        }
        if (CurrencyManagerScript.currentUpgradedTableIndex < triggerLvlPrefabs.Length && CurrencyManagerScript.TablePrefabIndex == 14) {
            triggerLvlPrefabs[CurrencyManagerScript.currentUpgradedTableIndex].SetActive(true);

            if (CurrencyManagerScript.currentUpgradedTableIndex > 0) {
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

            buyTableScript.DisablePrefab(CurrencyManagerScript.currentUpgradedTableIndex);

            if(CurrencyManagerScript.currentUpgradedTableIndex < triggerLvlPrefabs.Length)
            {
                triggerLvlPrefabs[CurrencyManagerScript.currentUpgradedTableIndex].SetActive(false);
            }

            switch(currentTableLevel)
            {   
                case 2:
                    if(CurrencyManagerScript.currentUpgradedTableIndex < Lvl2TablePrefabs.Length)
                    {
                        Lvl2TablePrefabs[CurrencyManagerScript.currentUpgradedTableIndex].SetActive(true);
                    }
                    CurrencyManagerScript.Lvl2Table += 1;
                    currencyManager.moneyMultiplier += 0.2;

                    SaveData();
                    break;
                case 3:
                    if(CurrencyManagerScript.currentUpgradedTableIndex < Lvl2TablePrefabs.Length)
                    {
                        Lvl3TablePrefabs[CurrencyManagerScript.currentUpgradedTableIndex].SetActive(true);
                        Lvl2TablePrefabs[CurrencyManagerScript.currentUpgradedTableIndex].SetActive(false);
                    }
                    CurrencyManagerScript.Lvl3Table += 1;
                    currencyManager.moneyMultiplier += 0.3;

                    SaveData();
                    break;
            }

            CurrencyManagerScript.currentUpgradedTableIndex += 1;
            if(CurrencyManagerScript.currentUpgradedTableIndex < triggerLvlPrefabs.Length)
            {
                triggerLvlPrefabs[CurrencyManagerScript.currentUpgradedTableIndex].SetActive(true);
            }

            if(CurrencyManagerScript.currentUpgradedTableIndex == 14)
            {
                if(currentTableLevel == 3) return;

                CurrencyManagerScript.currentUpgradedTableIndex = 0;

                UnlockUpgrades();

                currencyManager.upgradeTableCost *= 2.5;
                currentTableLevel++;

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

    private void OnApplicationQuit()
    {
        SaveData();
    }

    private void OnSceneUnloaded(Scene current)
    {
        SaveData();
    }

    private void LoadData()
    {
        CurrencyManagerScript.currentUpgradedTableIndex = PlayerPrefs.GetInt("UpgradeTableIndex", 0);
        currencyManager.upgradeTableCost = PlayerPrefs.GetFloat("UpgradeTableCost", 0);
        CurrencyManagerScript.Lvl2Table = PlayerPrefs.GetInt("Lvl2Table", 0);
        CurrencyManagerScript.Lvl3Table = PlayerPrefs.GetInt("Lvl3Table", 0);
        currentTableLevel = PlayerPrefs.GetInt("TableLevel", 1);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("UpgradeTableIndex", CurrencyManagerScript.currentUpgradedTableIndex);
        PlayerPrefs.SetFloat("UpgradeTableCost", (float)currencyManager.upgradeTableCost);
        PlayerPrefs.SetInt("Lvl2Table", CurrencyManagerScript.Lvl2Table);
        PlayerPrefs.SetInt("Lvl3Table", CurrencyManagerScript.Lvl3Table);
        PlayerPrefs.SetInt("TableLevel", currentTableLevel);
        PlayerPrefs.Save();
    }
}
