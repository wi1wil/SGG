using UnityEngine;
using TMPro;

public class CurrencyManagerScript : MonoBehaviour
{
    [Header("Currency Settings")]
    public TextMeshProUGUI currencyText;
    public double currencyInGame;

    public TextMeshProUGUI currencyPerSecText;
    public double currencyPerSecond;

    public TextMeshProUGUI multiplierText;
    public double moneyMultiplier;

    [Header("Enrolling Students / Students Settings")]
    public TextMeshProUGUI enrollStudentsText;
    public GameObject enrollStudentsUI;
    public double enrollStudentCost;
    public int totalStudents;

    [Header("Hiring Teachers")]
    public TextMeshProUGUI hireTeacherText;
    public GameObject hireTeacherUI;
    public double hireTeacherCost;

    [Header("Teacher Upgrades")]
    public TextMeshProUGUI upgradeTeacherText;
    public GameObject upgradeTeacherUI;
    public double upgradeTeacherCost;

    [Header ("Buy Sign Settings")]
    public double buySignCost;

    
    private void Start()
    {
        // Initialize currency per second and update the UI
        UpdateCurrencyPerSecond(); 
        UpdateUI();
    }

    private void Update() 
    {
        // Increment currency based on currency per second and time elapsed
        currencyInGame += currencyPerSecond * Time.deltaTime;
        UpdateUI();
    }

    public void addCash(double addedCash) 
    {
        // Add the cash multiplied by the money multiplier to the currency
        currencyInGame += (addedCash * moneyMultiplier);
        UpdateUI();
    }

    // Method to update the UI elements with the current values
    private void UpdateUI() 
    {
        currencyText.text = "Rp. " + currencyInGame.ToString("N0");
        currencyPerSecText.text = "Rp. " + currencyPerSecond.ToString("N0") +  "/ sec";
        multiplierText.text = "Money Multiplied by " + moneyMultiplier.ToString("F2");
        
        enrollStudentsText.text = "Enroll Students\nCost - " + enrollStudentCost.ToString("N0");
        hireTeacherText.text = "Hire Teacher\nCost - " + hireTeacherCost.ToString("N0");
        upgradeTeacherText.text = "Upgrade Teacher\n Cost - " + upgradeTeacherCost.ToString("N0");
    }

    // Method to hire a teacher
    public void hireTeacher() 
    {
        if (currencyInGame >= hireTeacherCost) 
        {
            currencyInGame -= hireTeacherCost;
            moneyMultiplier += 1;
            hireTeacherUI.SetActive(false);

            UpdateCurrencyPerSecond(); 
            UpdateUI();
        }
    }

    // Method to enroll students
    public void enrollStudents() 
    {
        if (currencyInGame >= enrollStudentCost) 
        {
            totalStudents++;
            currencyInGame -= enrollStudentCost;
            enrollStudentCost *= 2;
            
            UpdateCurrencyPerSecond(); 
            UpdateUI();
        }
    }

    // Method to upgrade teachers
    public void upgradeTeachers() 
    {
        if (currencyInGame >= upgradeTeacherCost) 
        {
            currencyInGame -= upgradeTeacherCost;
            moneyMultiplier *= 2;
            upgradeTeacherCost *= 2;

            UpdateCurrencyPerSecond();  
            UpdateUI();
        }
    }

    // Method to update the currency per second based on the current values
    private void UpdateCurrencyPerSecond() 
    {
        currencyPerSecond = moneyMultiplier * (totalStudents * 10000);
    }
}
