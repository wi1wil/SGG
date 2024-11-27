using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FocusUpScript : MonoBehaviour
{
    AudioManagerScript audioManager;
    CurrencyManagerScript currencyManager;

    [Header("Focus Up Event UI")]
    [SerializeField] private GameObject focusUpEventUI;
    [SerializeField] private GameObject timerPanel;
    public bool isFocusUpEventActive = false;

    [Header("Confirmation Panel")]
    [SerializeField] private GameObject confirmationPanel;
    [SerializeField] private Button yesButton;
    [SerializeField] private Button noButton;

    [Header("Accept Event Panel")]
    [SerializeField] private GameObject acceptEventPanel;
    [SerializeField] private Button acceptButton;
    [SerializeField] private Button declineButton;

    [Header("Duration UI")]
    [SerializeField] private GameObject durationPanel;
    [SerializeField] private Button tenMinutesButton;
    [SerializeField] private Button thirtyMinutesButton;

    [Header("Timer UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    public float remainingTime = 0f;

    [Header("Quit Button")]
    [SerializeField] private Button quitButton;
    bool quitButtonValue = false;

    private float totalFocusTime = 0f; 
    private bool tenMinutesTimer = false;
    private bool thirtyMinutesTimer = false;

    private void Start() {
        if (quitButton != null) quitButton.onClick.AddListener(OnQuitButtonClicked);
        if (yesButton != null) yesButton.onClick.AddListener(OnYesButtonClicked);
        if (noButton != null) noButton.onClick.AddListener(OnNoButtonClicked);
        if (acceptButton != null) acceptButton.onClick.AddListener(OnAcceptButtonClicked);
        if (declineButton != null) declineButton.onClick.AddListener(OnDeclineButtonClicked);
        if (tenMinutesButton != null) tenMinutesButton.onClick.AddListener(() => OnTenMinutesButtonClicked(600));
        if (thirtyMinutesButton != null) thirtyMinutesButton.onClick.AddListener(() => OnThirtyMinutesButtonClicked(1800));
    }

    private void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
        currencyManager = FindObjectOfType<CurrencyManagerScript>();
    }

    private void OnTenMinutesButtonClicked(float duration) {
        audioManager.PlaySfx(audioManager.yesButton);
        if (durationPanel != null) 
        {
            tenMinutesTimer = true;
            thirtyMinutesTimer = false;

            remainingTime = duration;
            totalFocusTime = duration;

            if (durationPanel != null) durationPanel.SetActive(false);
            if (timerPanel != null) timerPanel.SetActive(true);

            StartCoroutine(StartTimer());
        }
    }

    private void OnThirtyMinutesButtonClicked(float duration) {
        audioManager.PlaySfx(audioManager.yesButton);
        if (durationPanel != null) 
        {
            tenMinutesTimer = false;
            thirtyMinutesTimer = true;

            remainingTime = duration;
            totalFocusTime = duration;

            if (durationPanel != null) durationPanel.SetActive(false);
            if (timerPanel != null) timerPanel.SetActive(true);

            StartCoroutine(StartTimer());
        }
    }

    private void OnQuitButtonClicked() {
        audioManager.PlaySfx(audioManager.yesButton);
        quitButtonValue = true;
        if (confirmationPanel != null) confirmationPanel.SetActive(true);
    }

    private void OnAcceptButtonClicked() {
        audioManager.PlaySfx(audioManager.yesButton);
        if (durationPanel != null) 
        {
            acceptEventPanel.SetActive(false);
            durationPanel.SetActive(true);
        }
    }

    private void OnDeclineButtonClicked() {
        audioManager.PlaySfx(audioManager.noButton);
        if (focusUpEventUI != null) focusUpEventUI.SetActive(false);
        isFocusUpEventActive = false;
    }

    private void OnYesButtonClicked() {
        audioManager.PlaySfx(audioManager.yesButton);
        if (focusUpEventUI != null) focusUpEventUI.SetActive(false);
        isFocusUpEventActive = false; 
    }

    private void OnNoButtonClicked() {
        audioManager.PlaySfx(audioManager.noButton);
        if (confirmationPanel != null) confirmationPanel.SetActive(false);
    }

    public void StartFocusUpEvent() {
        if (isFocusUpEventActive) return; 
        isFocusUpEventActive = true;
        if (focusUpEventUI != null) focusUpEventUI.SetActive(true);
        if (acceptEventPanel != null) acceptEventPanel.SetActive(true);
    }

    private IEnumerator StartTimer() 
    {
        while (remainingTime > 0) 
        {
            remainingTime -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(remainingTime / 60F);
            int seconds = Mathf.FloorToInt(remainingTime - minutes * 60);
            if (timerText != null) timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return null;
        }

        if (focusUpEventUI != null) focusUpEventUI.SetActive(false);
        if (timerPanel != null) timerPanel.SetActive(false); 
        isFocusUpEventActive = false;

        if(tenMinutesTimer) 
        {
            currencyManager.addCash(totalFocusTime*currencyManager.currencyPerSecond);
            currencyManager.addCash(1.5*currencyManager.currencyInGame);
        }
        else if(thirtyMinutesTimer) 
        {
            currencyManager.addCash(totalFocusTime*currencyManager.currencyPerSecond);
            currencyManager.addCash(3*currencyManager.currencyInGame);
        }
    }

    private void Update() {
        if (isFocusUpEventActive && Input.anyKeyDown && !quitButtonValue) {
            return;
        }
    }
}