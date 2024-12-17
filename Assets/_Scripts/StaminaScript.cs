using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;

public class StaminaScript : MonoBehaviour
{   
    CurrencyManagerScript currencyManager;

    [Header("Stamina Settings")]
    [SerializeField] private Image staminaBar;
    [SerializeField] public float stamina;
    [SerializeField] public float maxStamina;
    [SerializeField] private float staminaDecreaseRate = 1f; 

    [Header("Typing Challenge")]
    [SerializeField] private TMP_InputField inputField;
    [SerializeField] private GameObject typingChallengePanel;
    [SerializeField] private List<string> wordsToType;

    [Header("Timer Settings")]
    [SerializeField] private float typingChallengeTimer = 10f;
    [SerializeField] private TextMeshProUGUI timerText;
    
    private bool isDecreasing = true;

    private void Awake() {
        currencyManager = FindObjectOfType<CurrencyManagerScript>();
    }

    private void Start()
    {
        LoadStamina();
        UpdateStaminaBar();
        maxStamina = CurrencyManagerScript.maxStamina;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        maxStamina = CurrencyManagerScript.maxStamina;
        currencyManager.LoadData();
        
        LoadStamina();
        UpdateStaminaBar();
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        SaveStamina();
        SaveIsDecreasing();
        currencyManager.SaveData(); 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name != "UniversityScene")
        {
            staminaBar.gameObject.SetActive(false);
        }
        else if (scene.name == "UniversityScene" && CurrencyManagerScript.isTeacherHired == 1)
        {
            staminaBar.gameObject.SetActive(true);
            currencyManager = FindObjectOfType<CurrencyManagerScript>();
            maxStamina = CurrencyManagerScript.maxStamina; 

            LoadStamina();
            UpdateStaminaBar();}
            LoadIsDecreasing();
    }

    private void Update()
    {
        if (CurrencyManagerScript.isTeacherHired == 1)
        {
            staminaBar.gameObject.SetActive(true);
            if (isDecreasing)
            {
                stamina -= staminaDecreaseRate * Time.deltaTime;
                if (stamina <= 0)
                {
                    stamina = 0;
                    isDecreasing = false;
                    currencyManager.moneyMultiplier = currencyManager.moneyMultiplier - CurrencyManagerScript.teacherLevel;
                    ActivateTypingChallenge();
                }
            }
            else if (typingChallengePanel.activeSelf)
            {
                ActivateTypingChallenge();
            }
            else
            {
                stamina += staminaDecreaseRate * Time.deltaTime;

                if (stamina >= maxStamina)
                {
                    stamina = maxStamina;
                    isDecreasing = true;
                    currencyManager.moneyMultiplier = currencyManager.moneyMultiplier + CurrencyManagerScript.teacherLevel;
                    typingChallengePanel.SetActive(false);
                }
            }

            stamina = Mathf.Clamp(stamina, 0, maxStamina);
            UpdateStaminaBar();
        }
        else
        {
            staminaBar.gameObject.SetActive(false);
        }
    }

    void ActivateTypingChallenge()
    {
        typingChallengeTimer -= Time.deltaTime;
        timerText.text = typingChallengeTimer.ToString("F0");
        typingChallengePanel.SetActive(true);
        if (typingChallengeTimer > 0)
        {
            string text = inputField.text;
            bool checkWord = checkTypeWords(text);
            if (checkWord)
            {
                typingChallengePanel.SetActive(false);
                typingChallengeTimer = 10f;
                inputField.text = "";
                text = "";
                stamina = maxStamina;
                isDecreasing = true;
                currencyManager.moneyMultiplier = currencyManager.moneyMultiplier + CurrencyManagerScript.teacherLevel;
            }
        }
        else
        {
            typingChallengePanel.SetActive(false);
            typingChallengeTimer = 10f;
        }
    }

    bool checkTypeWords(string text)
    {
        foreach (string word in wordsToType)
        {
            if (string.Equals(text.Trim(), word.Trim(), System.StringComparison.OrdinalIgnoreCase))
            {
                return true;
            }
        }
        return false;
    }

    private void OnApplicationQuit()
    {
        SaveStamina();
        SaveIsDecreasing();
        currencyManager.SaveData();
    }

    public void SaveStamina()
    {
        PlayerPrefs.SetFloat("TeacherStamina", stamina);
    }

    public void LoadStamina()
    {
        stamina = PlayerPrefs.GetFloat("TeacherStamina", maxStamina);
    }

    private void SaveIsDecreasing()
    {
        PlayerPrefs.SetInt("IsDecreasing", isDecreasing ? 1 : 0);
    }

    private void LoadIsDecreasing()
    {
        isDecreasing = PlayerPrefs.GetInt("IsDecreasing", 1) == 1;
    }

    public void UpdateStaminaBar()
    {
        if (staminaBar != null)
        {
            staminaBar.fillAmount = stamina / maxStamina;
        }
    }
}