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

    [Header("Lvl 1 Chair Prefabs")]
    [SerializeField] private GameObject[] Lvl1ChairPrefabs;

    [Header("Lvl 2 Chair Prefabs")]
    [SerializeField] private GameObject[] Lvl2ChairPrefabs;

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
        if (CurrencyManagerScript.ChairPrefabIndex == 14) {
            if(CurrencyManagerScript.Lv1Chair > 0) 
            {
                triggerPrefabs[0].SetActive(false);
            }
            else
            {
                triggerPrefabs[0].SetActive(true);
            }
        }
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
            LoadChairs();
        }
    }

    private void LoadChairs() {
        switch(upgradeIndex)
        {
            case 1:
                for (int i = 0; i < CurrencyManagerScript.Lv1Chair; i++) {
                    if (i < Lvl1ChairPrefabs.Length) {
                        Lvl1ChairPrefabs[i].SetActive(true);
                    }
                }
                break;
            case 2:
                for (int i = 0; i < CurrencyManagerScript.Lv2Chair; i++) {
                    if (i < Lvl2ChairPrefabs.Length) {
                        Lvl2ChairPrefabs[i].SetActive(true);
                    }
                }
                break;
        }

        if(CurrencyManagerScript.UpgradeChairIndex < triggerPrefabs.Length)
        {
            triggerPrefabs[CurrencyManagerScript.UpgradeChairIndex].SetActive(true);

            if(CurrencyManagerScript.UpgradeChairIndex > 0) {
                triggerPrefabs[0].SetActive(false);
            }
        }
    }

    private void Awake()
    {
        buyChairScript = FindObjectOfType<BuyChairScript>();
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

    public void ShowConfirmationPanel()
    {
        confirmationPanel.SetActive(true);
        priceText.text = "-= Upgrade Chair? =- \n\n" + "Cost: Rp. " + currencyManager.upgradeChairCost.ToString("N0") + "\n\n";
    }

    public void ConfirmUpgrade()
    {
        if (currencyManager.currencyInGame >= currencyManager.upgradeChairCost)
        {
            audioManager.PlaySfx(audioManager.yesButton);

            currencyManager.currencyInGame -= currencyManager.upgradeChairCost;

            CurrencyManagerScript.UpgradeChairIndex += 1;

            if(CurrencyManagerScript.UpgradeChairIndex == 14)
            {
                for(int i = 0; i < CurrencyManagerScript.UpgradeChairIndex; i++)
                {
                    triggerPrefabs[i].SetActive(false);
                }
                CurrencyManagerScript.UpgradeChairIndex = 0;
                currencyManager.upgradeChairCost *= 2;
                upgradeIndex += 1;
            }

            buyChairScript.DisablePrefab(CurrencyManagerScript.UpgradeChairIndex);

            if(CurrencyManagerScript.UpgradeChairIndex < triggerPrefabs.Length)
            {
                triggerPrefabs[CurrencyManagerScript.UpgradeChairIndex].SetActive(false);
            }

            switch(upgradeIndex)
            {   
                case 1:
                    CurrencyManagerScript.Lv1Chair += 1;
                    currencyManager.moneyMultiplier += 0.2;
                    if(CurrencyManagerScript.UpgradeChairIndex < Lvl1ChairPrefabs.Length)
                    {
                        Lvl1ChairPrefabs[CurrencyManagerScript.UpgradeChairIndex].SetActive(true);
                    }

                    break;
                case 2:
                    CurrencyManagerScript.Lv2Chair += 1;
                    currencyManager.moneyMultiplier += 0.3;
                    if(CurrencyManagerScript.UpgradeChairIndex < Lvl2ChairPrefabs.Length)
                    {
                        Lvl2ChairPrefabs[CurrencyManagerScript.UpgradeChairIndex].SetActive(true);
                    }
                    break;
            }

            CurrencyManagerScript.UpgradeChairIndex += 1;
            if(CurrencyManagerScript.UpgradeChairIndex < triggerPrefabs.Length)
            {
                triggerPrefabs[CurrencyManagerScript.UpgradeChairIndex].SetActive(true);
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
