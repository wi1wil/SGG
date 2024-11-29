using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CurrencyManagerScript : MonoBehaviour
{
    AudioManagerScript audioManager;

    [Header("Currency Settings")]
    public TextMeshProUGUI currencyText;
    public double currencyInGame;

    public TextMeshProUGUI currencyPerSecText;
    public double currencyPerSecond;

    public TextMeshProUGUI moneyMultiplierText;
    public double moneyMultiplier;

    public double idleGains;
    public static int doubleMultiplier = 1;
    public static int doubleCashValue = 1;

    [Header("Pop Up Text Settings")]
    [SerializeField] private TextMeshProUGUI popUpText;
    [SerializeField] private GameObject parentInEnvironment;

    [Header("Enrolling Students / Students Settings")]
    public TextMeshProUGUI enrollStudentsText;
    public GameObject enrollStudentsUI;
    public double enrollStudentCost;
    public int totalStudents;

    [Header("Hiring Teachers")]
    public TextMeshProUGUI hireTeacherText;
    public GameObject hireTeacherUI;
    public double hireTeacherCost;
    public static int isTeacherHired = 0;

    [Header("Teacher Upgrades")]
    public TextMeshProUGUI upgradeTeacherText;
    public GameObject upgradeTeacherUI;
    public double upgradeTeacherCost;
    public static int teacherLevel = 1;

    [Header ("Buy Sign Settings")]
    public double buySignCost;

    [Header("Tables and Chairs")]
    public double tableCost;
    public double chairCost;

    public static int PrefabIndex = 0;
    public static int ChairPrefabIndex = 0;

    public static int tableAmount = 0;
    public static int chairAmount = 0;

    private void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
    }

    private void OnDisable() {
        SaveData(); 
    }

    private void Start()
    {
        // Load from PlayerPrefs
        LoadData();
        // Initialize currency per second and update the UI
        UpdateCurrencyPerSecond(); 
        UpdateUI();
        // Check if teacher is hired and deactivate the hireTeacherUI if true
        CheckTeacherStatus();
    }

    private void LoadData()
    {
        tableAmount = PlayerPrefs.GetInt("TableAmount", 0);
        tableCost = PlayerPrefs.GetFloat("TableCost", 100000);
        chairAmount = PlayerPrefs.GetInt("ChairAmount", 0);
        chairCost = PlayerPrefs.GetFloat("ChairCost", 75000);
        currencyInGame = PlayerPrefs.GetFloat("CurrencyInGame", 0);
        currencyPerSecond = PlayerPrefs.GetFloat("CurrencyPerSecond", 0);
        totalStudents = PlayerPrefs.GetInt("TotalStudents", 0);
        enrollStudentCost = PlayerPrefs.GetFloat("EnrollStudentCost", 150000); 
        isTeacherHired = PlayerPrefs.GetInt("IsTeacherHired", 0);
        hireTeacherCost = PlayerPrefs.GetFloat("HireTeacherCost", 250000);
        upgradeTeacherCost = PlayerPrefs.GetFloat("UpgradeTeacherCost", 1000000); 
        teacherLevel = PlayerPrefs.GetInt("TeacherLevel", 1);
        moneyMultiplier = PlayerPrefs.GetFloat("MoneyMultiplier", 1);
        idleGains = totalStudents * 10000; 
        PrefabIndex = PlayerPrefs.GetInt("PrefabIndex", 0);
        ChairPrefabIndex = PlayerPrefs.GetInt("ChairPrefabIndex", 0);
        doubleMultiplier = PlayerPrefs.GetInt("DoubleMultiplier", 1);
        doubleCashValue = PlayerPrefs.GetInt("DoubleCashValue", 1);
    }

    public void SaveData()
    {
        PlayerPrefs.SetInt("TableAmount", tableAmount);
        PlayerPrefs.SetFloat("TableCost", (float)tableCost);
        PlayerPrefs.SetInt("ChairAmount", chairAmount);
        PlayerPrefs.SetFloat("ChairCost", (float)chairCost);
        PlayerPrefs.SetFloat("CurrencyInGame", (float)currencyInGame);
        PlayerPrefs.SetFloat("CurrencyPerSecond", (float)currencyPerSecond);
        PlayerPrefs.SetInt("TotalStudents", totalStudents);
        PlayerPrefs.SetFloat("EnrollStudentCost", (float)enrollStudentCost); 
        PlayerPrefs.SetInt("IsTeacherHired", isTeacherHired);
        PlayerPrefs.SetFloat("HireTeacherCost", (float)hireTeacherCost); 
        PlayerPrefs.SetFloat("UpgradeTeacherCost", (float)upgradeTeacherCost); 
        PlayerPrefs.SetInt("TeacherLevel", teacherLevel);
        PlayerPrefs.SetFloat("MoneyMultiplier", (float)moneyMultiplier);
        PlayerPrefs.SetFloat("IdleGains", (float)idleGains);
        PlayerPrefs.SetInt("PrefabIndex", PrefabIndex);
        PlayerPrefs.SetInt("ChairPrefabIndex", ChairPrefabIndex);
        PlayerPrefs.SetInt("DoubleMultiplier", doubleMultiplier);
        PlayerPrefs.SetInt("DoubleCashValue", doubleCashValue);
        PlayerPrefs.Save();
    }

    private void Update() 
    {
        // Increment currency based on currency per second and time elapsed
        currencyInGame += (currencyPerSecond * Time.deltaTime);
        // Save currency to PlayerPrefs
        SaveData();
        UpdateUI();
    }

    private void NotEnoughMoney()
    {
        var go = Instantiate(popUpText, transform.position, Quaternion.identity);
        go.transform.SetParent(parentInEnvironment.transform, false);
        go.GetComponent<TextMeshProUGUI>().text = "Not enough money!";
    }

    public void addCash(double addedCash) 
    {
        currencyInGame += (addedCash);
        // Save currency to PlayerPrefs
        SaveData();
        UpdateUI();
    }

    public void UpdateUI() 
    {
        currencyText.text = "Rp. " + currencyInGame.ToString("N0");
        currencyPerSecText.text = "Rp. " + currencyPerSecond.ToString("N0") +  "/ sec";
        moneyMultiplierText.text = "Money Multiplied by " + (moneyMultiplier * doubleMultiplier).ToString("F2");
        
        enrollStudentsText.text = "Enroll Students\nCost - " + enrollStudentCost.ToString("N0");
        hireTeacherText.text = "Hire Teacher\nCost - " + hireTeacherCost.ToString("N0");
        upgradeTeacherText.text = "Upgrade Teacher\n Cost - " + upgradeTeacherCost.ToString("N0");
    }

    // Method to hire a teacher
    public void hireTeacher() 
    {
        if (currencyInGame >= hireTeacherCost) 
        {
            audioManager.PlaySfx(audioManager.yesButton);
            currencyInGame -= hireTeacherCost;
            moneyMultiplier += 1;
            isTeacherHired = 1;
            hireTeacherUI.SetActive(false);
            upgradeTeacherUI.SetActive(true);

            // Save currency to PlayerPrefs
            SaveData();

            UpdateCurrencyPerSecond(); 
            UpdateUI();
        }
        else
        {
            audioManager.PlaySfx(audioManager.noButton);
            NotEnoughMoney();
        }
    }

    // Method to enroll students
    public void enrollStudents() 
    {
        if (currencyInGame >= enrollStudentCost) 
        {
            audioManager.PlaySfx(audioManager.yesButton);
            totalStudents++;
            currencyInGame -= enrollStudentCost;
            enrollStudentCost *= 2;
            
            // Update idleGains based on totalStudents
            idleGains = totalStudents * 10000;

            // Save currency to PlayerPrefs
            SaveData();

            UpdateCurrencyPerSecond(); 
            UpdateUI();
        }
        else
        {
            audioManager.PlaySfx(audioManager.noButton);
            NotEnoughMoney();
        }
    }

    // Method to upgrade teachers
    public void upgradeTeachers() 
    {
        if (currencyInGame >= upgradeTeacherCost) 
        {
            audioManager.PlaySfx(audioManager.yesButton);
            currencyInGame -= upgradeTeacherCost;
            moneyMultiplier *= 2;
            upgradeTeacherCost *= 2;
            teacherLevel++;

            // Save currency to PlayerPrefs
            SaveData();

            UpdateCurrencyPerSecond();  
            UpdateUI();
        }
        else
        {
            audioManager.PlaySfx(audioManager.noButton);
            NotEnoughMoney();
        }
    }

    // Method to update the currency per second based on the current values
    public void UpdateCurrencyPerSecond() 
    {
        currencyPerSecond = (moneyMultiplier * doubleMultiplier) * idleGains;
        currencyPerSecond += idleGains * (tableAmount * 0.5);
        currencyPerSecond += idleGains * (chairAmount * 0.3); 
        SaveData(); // Save the updated currencyPerSecond
    }

    // Method to check if the teacher is hired and deactivate the hireTeacherUI if true
    private void CheckTeacherStatus()
    {
        if (SceneManager.GetActiveScene().name == "UniversityScene")
        {
            if (isTeacherHired == 1)
            {
                hireTeacherUI.SetActive(false);
                upgradeTeacherUI.SetActive(true);
            }
        }
    }
}