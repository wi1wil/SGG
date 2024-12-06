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
    [SerializeField] private GameObject[] triggerPrefabs;

    [Header("Lvl 1 Table Prefabs")]
    [SerializeField] private GameObject[] Lvl1TablePrefabs;

    [Header("Lvl 2 Table Prefabs")]
    [SerializeField] private GameObject[] Lvl2TablePrefabs;

    [Header("Confirmation Panel")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Pop Up Text")]
    [SerializeField] private GameObject popUpText;
    [SerializeField] private GameObject parentInEnvironment;

    public int upgradeIndex = 1;

    private void Update() {
        if (CurrencyManagerScript.TablePrefabIndex == 14) {
            if(CurrencyManagerScript.Lv1Table > 0) 
            {
                triggerPrefabs[0].SetActive(false);
            }
            else
            {
                triggerPrefabs[0].SetActive(true);
            }
        }
    }

    private void Awake()
    {
        buyTableScript = FindObjectOfType<BuyTableScript>();
        currencyManager = FindObjectOfType<CurrencyManagerScript>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
        

        yesButton.onClick.AddListener(ConfirmUpgrade);
        noButton.onClick.AddListener(() =>
        {
            confirmationPanel.SetActive(false);
            audioManager.PlaySfx(audioManager.noButton);
        });
        LoadData();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "UniversityScene") {
            LoadTables();
        }
    }

    private void LoadTables() {
        switch(upgradeIndex)
        {
            case 1:
                for (int i = 0; i < CurrencyManagerScript.Lv1Table; i++) {
                    if (i < Lvl1TablePrefabs.Length) {
                        Lvl1TablePrefabs[i].SetActive(true);
                    }
                }
                break;
            case 2:
                for (int i = 0; i < CurrencyManagerScript.Lv2Table; i++) {
                    if (i < Lvl2TablePrefabs.Length) {
                        Lvl2TablePrefabs[i].SetActive(true);
                    }
                }
                break;
        }

        if(CurrencyManagerScript.UpgradeTableIndex < triggerPrefabs.Length)
        {
            triggerPrefabs[CurrencyManagerScript.UpgradeTableIndex].SetActive(true);

            if(CurrencyManagerScript.UpgradeTableIndex > 0) {
                triggerPrefabs[0].SetActive(false);
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
            audioManager.PlaySfx(audioManager.yesButton);

            currencyManager.currencyInGame -= currencyManager.upgradeTableCost;


            CurrencyManagerScript.UpgradeTableIndex += 1;

            if(CurrencyManagerScript.UpgradeTableIndex == 14)
            {
                for(int i = 0; i < CurrencyManagerScript.UpgradeTableIndex; i++)
                {
                    triggerPrefabs[i].SetActive(false);
                }
                CurrencyManagerScript.UpgradeTableIndex = 0;
                currencyManager.upgradeTableCost *= 2;
                upgradeIndex += 1;
            }

            buyTableScript.DisablePrefab(CurrencyManagerScript.UpgradeTableIndex);

            if(CurrencyManagerScript.UpgradeTableIndex < triggerPrefabs.Length)
            {
                triggerPrefabs[CurrencyManagerScript.UpgradeTableIndex].SetActive(false);
            }

            switch(upgradeIndex)
            {   
                case 1:
                    CurrencyManagerScript.Lv1Table += 1;
                    currencyManager.moneyMultiplier += 0.2;
                    if(CurrencyManagerScript.UpgradeTableIndex < Lvl1TablePrefabs.Length)
                    {
                        Lvl1TablePrefabs[CurrencyManagerScript.UpgradeTableIndex].SetActive(true);
                    }

                    break;
                case 2:
                    CurrencyManagerScript.Lv2Table += 1;
                    currencyManager.moneyMultiplier += 0.3;
                    if(CurrencyManagerScript.UpgradeTableIndex < Lvl2TablePrefabs.Length)
                    {
                        Lvl2TablePrefabs[CurrencyManagerScript.UpgradeTableIndex].SetActive(true);
                    }
                    break;
            }

            CurrencyManagerScript.UpgradeTableIndex += 1;
            if(CurrencyManagerScript.UpgradeTableIndex < triggerPrefabs.Length)
            {
                triggerPrefabs[CurrencyManagerScript.UpgradeTableIndex].SetActive(true);
            }

            confirmationPanel.SetActive(false);

            currencyManager.SaveData();
            SaveData();
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
        CurrencyManagerScript.Lv1Table = PlayerPrefs.GetInt("Lv1Table", 0);
        CurrencyManagerScript.Lv2Table = PlayerPrefs.GetInt("Lv2Table", 0);
        CurrencyManagerScript.Lv1Chair = PlayerPrefs.GetInt("Lv1Chair", 0);
        CurrencyManagerScript.Lv2Chair = PlayerPrefs.GetInt("Lv2Chair", 0);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("UpgradeTableIndex", CurrencyManagerScript.UpgradeTableIndex);
        PlayerPrefs.SetInt("UpgradeChairIndex", CurrencyManagerScript.UpgradeChairIndex);
        PlayerPrefs.SetFloat("UpgradeChairCost", (float)currencyManager.upgradeChairCost);
        PlayerPrefs.SetFloat("UpgradeTableCost", (float)currencyManager.upgradeTableCost);
        PlayerPrefs.SetInt("Lv1Table", CurrencyManagerScript.Lv1Table);
        PlayerPrefs.SetInt("Lv2Table", CurrencyManagerScript.Lv2Table);
        PlayerPrefs.SetInt("Lv1Chair", CurrencyManagerScript.Lv1Chair);
        PlayerPrefs.SetInt("Lv2Chair", CurrencyManagerScript.Lv2Chair);
        PlayerPrefs.Save();
    }
}
