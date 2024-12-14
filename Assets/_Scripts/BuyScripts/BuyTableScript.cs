using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Unity.VisualScripting;

public class BuyTableScript : MonoBehaviour
{
    UpgradeTableScript upgradeTableScript;
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

    private void Awake() 
    {
        upgradeTableScript = FindObjectOfType<UpgradeTableScript>();
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
            LoadTables();
        }
    }

    private void LoadTables() 
    {
        for (int i = 0; i < CurrencyManagerScript.TablePrefabIndex; i++)
        {
            if (i < tablePrefabs.Length)
            {
                tablePrefabs[i].SetActive(true);
            }
        }

        if (CurrencyManagerScript.TablePrefabIndex < triggerPrefabs.Length)
        {
            triggerPrefabs[CurrencyManagerScript.TablePrefabIndex].SetActive(true);

            if (CurrencyManagerScript.TablePrefabIndex > 0)
            {
                triggerPrefabs[0].SetActive(false);
            }
        }

        if (CurrencyManagerScript.TablePrefabIndex == 0 && CurrencyManagerScript.TablePrefabIndex < triggerPrefabs.Length)
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
        confirmationText.text = "-= Buy Table? =-\n\n" + "Cost: Rp. " + currencyManager.tableCost.ToString("N0") + "\n\n";
    }

    private void ConfirmPurchase()
    {
        if (currencyManager.currencyInGame >= currencyManager.tableCost)
        {
            audioManager.PlaySfx(audioManager.buildingSFX);

            currencyManager.currencyInGame -= currencyManager.tableCost;

            CurrencyManagerScript.tableAmount += 1;
            currencyManager.moneyMultiplier += 0.1; 

            if (CurrencyManagerScript.TablePrefabIndex < triggerPrefabs.Length)
            {
                triggerPrefabs[CurrencyManagerScript.TablePrefabIndex].SetActive(false);
            }

            if (CurrencyManagerScript.TablePrefabIndex < tablePrefabs.Length)
            {
                tablePrefabs[CurrencyManagerScript.TablePrefabIndex].SetActive(true);
            }

            CurrencyManagerScript.TablePrefabIndex++;
            if (CurrencyManagerScript.TablePrefabIndex < triggerPrefabs.Length)
            {
                triggerPrefabs[CurrencyManagerScript.TablePrefabIndex].SetActive(true);
            }

            PlayerPrefs.SetInt("PrefabIndex", CurrencyManagerScript.TablePrefabIndex); 

            currencyManager.SaveData();
            currencyManager.UpdateUI();
            currencyManager.UpdateCurrencyPerSecond();

            if (CurrencyManagerScript.TablePrefabIndex == 14)
            {
                upgradeTableScript.UnlockUpgrades();
                upgradeTableScript.upgradeTableIndex += 1;

                SaveData();
                upgradeTableScript.SaveData();
            }

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
        tablePrefabs[index].SetActive(false);
    }

    private void LoadData()
    {
        CurrencyManagerScript.TablePrefabIndex = PlayerPrefs.GetInt("PrefabIndex", 0);
        CurrencyManagerScript.tableAmount = PlayerPrefs.GetInt("TableAmount", 0);
        upgradeTableScript.upgradeTableIndex = PlayerPrefs.GetInt("TableLevelIndex", 1);
    }

    private void SaveData() {
        PlayerPrefs.SetInt("PrefabIndex", CurrencyManagerScript.TablePrefabIndex);
        PlayerPrefs.SetInt("TableAmount", CurrencyManagerScript.tableAmount);
        PlayerPrefs.Save();
    }

    private void OnApplicationQuit()
    {
        SaveData();
    }
}