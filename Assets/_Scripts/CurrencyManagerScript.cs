using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class CurrencyManagerScript : MonoBehaviour
{
    StudentBehaviourScript studentBehaviour;
    AudioManagerScript audioManager;
    StaminaScript staminaScript;

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

    public bool canEnrollStudents;

    [Header("Hiring Teachers")]
    public GameObject teacher;
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

    [Header ("Janitor Settings")]
    public TextMeshProUGUI janitorText;
    public GameObject janitor;
    public GameObject hireJanitorUI;
    public double janitorCost;
    public static int isJanitorHired = 0;

    [Header("Tables and Chairs")]
    public double tableCost;
    public double chairCost;

    public double upgradeTableCost;
    public double upgradeChairCost;

    public static int TablePrefabIndex = 0;
    public static int ChairPrefabIndex = 0;

    public static int UpgradeTableIndex = 0;
    public static int UpgradeChairIndex = 0;

    public static int tableAmount = 0;
    public static int chairAmount = 0;

    [SerializeField] private int totalTables;
    [SerializeField] private int totalChairs;

    public static int Lvl2Table = 0;
    public static int Lvl3Table = 0;

    public static int Lvl2Chair = 0;
    public static int Lvl3Chair = 0;

    public static float maxStamina = 100;

    private void Awake() {
        studentBehaviour = FindObjectOfType<StudentBehaviourScript>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
        staminaScript = FindObjectOfType<StaminaScript>();
    }

    private void Start()
    {
        LoadData();
        UpdateCurrencyPerSecond(); 
        UpdateUI();
        CheckTeacherStatus();
        CheckJanitorStatus();
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadData();
    }

    private void OnDisable() {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SaveData(); 
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindJanitor();
        FindTeacher();
        CheckTeacherStatus();
        CheckJanitorStatus();
    }

    public void LoadData()
    {
        janitorCost = PlayerPrefs.GetFloat("JanitorCost", 20000000);
        isJanitorHired = PlayerPrefs.GetInt("IsJanitorHired", 0); 
        tableAmount = PlayerPrefs.GetInt("TableAmount", 0);
        tableCost = PlayerPrefs.GetFloat("TableCost", 200000);
        chairAmount = PlayerPrefs.GetInt("ChairAmount", 0);
        chairCost = PlayerPrefs.GetFloat("ChairCost", 100000);
        currencyInGame = PlayerPrefs.GetFloat("CurrencyInGame", 0);
        currencyPerSecond = PlayerPrefs.GetFloat("CurrencyPerSecond", 0);
        totalStudents = PlayerPrefs.GetInt("TotalStudents", 0);
        enrollStudentCost = PlayerPrefs.GetFloat("EnrollStudentCost", 150000); 
        isTeacherHired = PlayerPrefs.GetInt("IsTeacherHired", 0);
        hireTeacherCost = PlayerPrefs.GetFloat("HireTeacherCost", 250000);
        upgradeTeacherCost = PlayerPrefs.GetFloat("UpgradeTeacherCost", 1000000); 
        teacherLevel = PlayerPrefs.GetInt("TeacherLevel", 1);
        moneyMultiplier = PlayerPrefs.GetFloat("MoneyMultiplier", 1);
        idleGains = totalStudents * 2500; 
        TablePrefabIndex = PlayerPrefs.GetInt("PrefabIndex", 0);
        ChairPrefabIndex = PlayerPrefs.GetInt("ChairPrefabIndex", 0);
        doubleMultiplier = PlayerPrefs.GetInt("DoubleMultiplier", 1);
        doubleCashValue = PlayerPrefs.GetInt("DoubleCashValue", 1);
        UpgradeTableIndex = PlayerPrefs.GetInt("UpgradeTableIndex", 0);
        UpgradeChairIndex = PlayerPrefs.GetInt("UpgradeChairIndex", 0);
        upgradeTableCost = PlayerPrefs.GetFloat("UpgradeTableCost", (float)tableCost * 2.5f);
        upgradeChairCost = PlayerPrefs.GetFloat("UpgradeChairCost", (float)chairCost * 2.5f);
        Lvl2Table = PlayerPrefs.GetInt("Lvl2Table", 0);
        Lvl3Table = PlayerPrefs.GetInt("Lvl3Table", 0);
        Lvl2Chair = PlayerPrefs.GetInt("Lvl2Chair", 0); 
        Lvl3Chair = PlayerPrefs.GetInt("Lvl3Chair", 0);
        maxStamina = PlayerPrefs.GetFloat("MaxStamina", 100); 
    }

    public void SaveData()
    {
        PlayerPrefs.SetFloat("JanitorCost", (float)janitorCost);
        PlayerPrefs.SetInt("IsJanitorHired", isJanitorHired); 
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
        PlayerPrefs.SetInt("PrefabIndex", TablePrefabIndex);
        PlayerPrefs.SetInt("ChairPrefabIndex", ChairPrefabIndex);
        PlayerPrefs.SetInt("DoubleMultiplier", doubleMultiplier);
        PlayerPrefs.SetInt("DoubleCashValue", doubleCashValue);
        PlayerPrefs.SetInt("UpgradeTableIndex", UpgradeTableIndex);
        PlayerPrefs.SetInt("UpgradeChairIndex", UpgradeChairIndex);
        PlayerPrefs.SetFloat("UpgradeTableCost", (float)upgradeTableCost);
        PlayerPrefs.SetFloat("UpgradeChairCost", (float)upgradeChairCost);
        PlayerPrefs.SetInt("Lvl2Table", Lvl2Table);
        PlayerPrefs.SetInt("Lvl3Table", Lvl3Table);
        PlayerPrefs.SetInt("Lvl2Chair", Lvl2Chair);
        PlayerPrefs.SetInt("Lvl3Chair", Lvl3Chair);
        PlayerPrefs.SetFloat("MaxStamina", maxStamina); 
        PlayerPrefs.Save();
    }

    private void Update() 
    {
        currencyInGame += (currencyPerSecond * Time.deltaTime);

        checkStudentEnrollment();
        SaveData();
        UpdateUI();
    }

    public void checkStudentEnrollment() 
    {
        totalChairs = chairAmount;
        totalTables = tableAmount;

        canEnrollStudents = ((tableAmount > 0 && chairAmount > 0) && (tableAmount == chairAmount) && ((totalStudents < tableAmount) && (totalStudents < chairAmount)) && (totalStudents < 14));

        if(canEnrollStudents)
        {
            enrollStudentsUI.SetActive(true);
        }
        else
        {
            enrollStudentsUI.SetActive(false);
        }
    }

    private void NotEnoughMoney()
    {
        var go = Instantiate(popUpText, new Vector3(0, 5, 0), Quaternion.identity);
        go.transform.SetParent(parentInEnvironment.transform, false);
        go.GetComponent<TextMeshProUGUI>().text = "Not enough money!";
    }

    public void addCash(double addedCash) 
    {
        currencyInGame += (addedCash);
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
        janitorText.text = "Hire Janitor\nCost - " + janitorCost.ToString("N0");

        canEnrollStudents = ((tableAmount > 0 && chairAmount > 0) && (tableAmount == chairAmount));

    }

    public void hireTeacher() 
    {
        if (currencyInGame >= hireTeacherCost) 
        {
            audioManager.PlaySfx(audioManager.buySuccessSFX);
            currencyInGame -= hireTeacherCost;
            moneyMultiplier += 0.25;
            isTeacherHired = 1;
            teacher.SetActive(true);
            hireTeacherUI.SetActive(false);
            upgradeTeacherUI.SetActive(true);

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

    public void enrollStudents() 
    {
        enrollStudentsUI.SetActive(true);
        if (currencyInGame >= enrollStudentCost) 
        {
            audioManager.PlaySfx(audioManager.buySuccessSFX);
            totalStudents++;
            currencyInGame -= enrollStudentCost;
            enrollStudentCost *= 1.5;
            
            idleGains = totalStudents * 2500;

            SaveData();
            studentBehaviour.ActivateStudent(totalStudents - 1);
            UpdateCurrencyPerSecond(); 
            UpdateUI();
        }
        else
        {
            audioManager.PlaySfx(audioManager.noButton);
            NotEnoughMoney();
        }
    }

    public void upgradeTeachers() 
    {
        if (currencyInGame >= upgradeTeacherCost) 
        {
            audioManager.PlaySfx(audioManager.buySuccessSFX);
            currencyInGame -= upgradeTeacherCost;
            moneyMultiplier += 0.25;
            upgradeTeacherCost *= 1.5;
            teacherLevel++;
            maxStamina += 50;
            staminaScript.stamina = maxStamina;

            SaveData();
            staminaScript.UpdateStaminaBar();
            staminaScript.SaveStamina();
            staminaScript.LoadStamina();

            UpdateCurrencyPerSecond();  
            UpdateUI();
        }
        else
        {
            audioManager.PlaySfx(audioManager.noButton);
            NotEnoughMoney();
        }
    }

    public void hireJanitor() 
    {
        if (currencyInGame >= janitorCost) 
        {
            audioManager.PlaySfx(audioManager.buySuccessSFX);
            currencyInGame -= janitorCost;
            isJanitorHired = 1;
            janitor.SetActive(true);
            hireJanitorUI.SetActive(false);

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

    public void UpdateCurrencyPerSecond() 
    {
        currencyPerSecond = (moneyMultiplier * doubleMultiplier) * idleGains;
        SaveData(); 
    }

    private void CheckTeacherStatus()
    {
        if (SceneManager.GetActiveScene().name == "UniversityScene")
        {
            if (isTeacherHired == 1)
            {
                teacher.SetActive(true);
                hireTeacherUI.SetActive(false);
                upgradeTeacherUI.SetActive(true);
            }
            else  
            {
                teacher.SetActive(false);
                hireTeacherUI.SetActive(true);
                upgradeTeacherUI.SetActive(false);
            }
        }
    }

    private void CheckJanitorStatus()
    {
        if (SceneManager.GetActiveScene().name == "GameplayScene")
        {
            if (isJanitorHired == 1)
            {
                janitor.SetActive(true);
                hireJanitorUI.SetActive(false);
            }
            else
            {
                janitor.SetActive(false);
            }
        }
    }

    private void FindJanitor()
    {
        SpriteRenderer[] activeAndInactive = FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var obj in activeAndInactive)
        {
            if (obj.gameObject.name == "Janitor")
            {
                janitor = obj.gameObject;
                break;
            }
        }
    }

    private void FindTeacher()
    {
        SpriteRenderer[] activeAndInactive = FindObjectsByType<SpriteRenderer>(FindObjectsInactive.Include, FindObjectsSortMode.None);
        foreach (var obj in activeAndInactive)
        {
            if (obj.gameObject.name == "Teacher")
            {
                teacher = obj.gameObject;
                break;
            }
        }
    }

    private void OnApplicationQuit() {
        SaveData();
    }
}