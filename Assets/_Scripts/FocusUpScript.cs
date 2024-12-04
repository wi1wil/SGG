using System.Collections;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class FocusUpScript : MonoBehaviour
{
    AudioManagerScript audioManager;
    CurrencyManagerScript currencyManager;

    [Header("Start Focus Up Button")]
    [SerializeField] private Button startFocusUpButton;

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

    [Header("Accept Panel")]
    [SerializeField] private GameObject acceptPanel;
    [SerializeField] private Button acceptPanelButton;
    [SerializeField] private Button declinePanelButton;
    [SerializeField] private Button quitPanelButton;

    [Header("Duration UI")]
    [SerializeField] private GameObject durationPanel;
    [SerializeField] private Button tenMinutesButton;
    [SerializeField] private Button thirtyMinutesButton;

    [Header("Timer UI")]
    [SerializeField] private TextMeshProUGUI timerText;
    [SerializeField] private TextMeshProUGUI timerDescriptionText;
    public float remainingTime = 0f;
    public float elapsedTime = 0f;

    [Header("Quit Button")]
    [SerializeField] private Button quitButton;
    bool quitButtonValue = false;

    private float totalFocusTime = 0f; 
    private bool tenMinutesTimer = false;
    private bool thirtyMinutesTimer = false;
    private Coroutine focusUpTimerCoroutine;

    private void Start() {
        if (startFocusUpButton != null) startFocusUpButton.onClick.AddListener(StartFocusUp);
        if (quitButton != null) quitButton.onClick.AddListener(OnQuitButtonClicked);
        if (quitPanelButton != null) quitPanelButton.onClick.AddListener(OnQuitPanelButtonClicked);
        if (yesButton != null) yesButton.onClick.AddListener(OnYesButtonClicked);
        if (noButton != null) noButton.onClick.AddListener(OnNoButtonClicked);
        if (acceptButton != null) acceptButton.onClick.AddListener(OnAcceptButtonClicked);
        if (declineButton != null) declineButton.onClick.AddListener(OnDeclineButtonClicked);
        if (acceptPanelButton != null) acceptPanelButton.onClick.AddListener(OnAcceptPanelButtonClicked);
        if (declinePanelButton != null) declinePanelButton.onClick.AddListener(OnDeclinePanelButtonClicked);
        if (tenMinutesButton != null) tenMinutesButton.onClick.AddListener(() => OnTenMinutesButtonClicked(600));
        if (thirtyMinutesButton != null) thirtyMinutesButton.onClick.AddListener(() => OnThirtyMinutesButtonClicked(1800));
    }

    private void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
        currencyManager = FindObjectOfType<CurrencyManagerScript>();
    }

    private void OnTenMinutesButtonClicked(float duration) {
        audioManager.PlaySfx(audioManager.yesButton);
        tenMinutesTimer = true;
        thirtyMinutesTimer = false;

        remainingTime = duration;
        totalFocusTime = duration;

        if (durationPanel != null) durationPanel.SetActive(false);
        if (timerPanel != null) timerPanel.SetActive(true);
        if (quitButton != null) quitButton.gameObject.SetActive(true);
        if (quitPanelButton != null) quitPanelButton.gameObject.SetActive(false);

        StartCoroutine(StartTimer());
    }

    private void OnThirtyMinutesButtonClicked(float duration) {
        audioManager.PlaySfx(audioManager.yesButton);
        tenMinutesTimer = false;
        thirtyMinutesTimer = true;

        remainingTime = duration;
        totalFocusTime = duration;

        if (durationPanel != null) durationPanel.SetActive(false);
        if (timerPanel != null) timerPanel.SetActive(true);
        if (quitButton != null) quitButton.gameObject.SetActive(true);
        if (quitPanelButton != null) quitPanelButton.gameObject.SetActive(false);

        StartCoroutine(StartTimer());
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
            currencyManager.addCash(totalFocusTime * currencyManager.currencyPerSecond);
            currencyManager.addCash(1.1 * currencyManager.currencyInGame);
        }
        else if(thirtyMinutesTimer) 
        {
            currencyManager.addCash(totalFocusTime * currencyManager.currencyPerSecond);
            currencyManager.addCash(1.5 * currencyManager.currencyInGame);
        }
    }

    private void Update() {
        if (isFocusUpEventActive && Input.anyKeyDown && !quitButtonValue) {
            return;
        }
    }

    public void StartFocusUp() {
        if (isFocusUpEventActive) return;
        isFocusUpEventActive = true;
        if (focusUpEventUI != null) focusUpEventUI.SetActive(true);
        if (acceptPanel != null) acceptPanel.SetActive(true);
    }

    private void OnAcceptPanelButtonClicked() {
        audioManager.PlaySfx(audioManager.yesButton);
        if (acceptPanel != null) acceptPanel.SetActive(false);
        if (timerPanel != null) timerPanel.SetActive(true);
        if (quitButton != null) quitButton.gameObject.SetActive(false);
        if (quitPanelButton != null) quitPanelButton.gameObject.SetActive(true);
        
        StartFocusUpTimer();
    }

    private void OnDeclinePanelButtonClicked() {
        audioManager.PlaySfx(audioManager.noButton);
        if (focusUpEventUI != null) focusUpEventUI.SetActive(false);
        isFocusUpEventActive = false;
    }

    private void StartFocusUpTimer() {
        timerDescriptionText.text = "Elapsed Time: ";
        currencyManager.moneyMultiplier += 0.5f;
        currencyManager.UpdateUI();
        Debug.Log("Money Multiplier increased to: " + currencyManager.moneyMultiplier);

        if (focusUpTimerCoroutine != null)
        {
            StopCoroutine(focusUpTimerCoroutine);
        }
        focusUpTimerCoroutine = StartCoroutine(FocusUpTimerCoroutine());
    }

    private IEnumerator FocusUpTimerCoroutine() {
        while (true) {
            elapsedTime += Time.deltaTime;
            int minutes = Mathf.FloorToInt(elapsedTime / 60F);
            int seconds = Mathf.FloorToInt(elapsedTime - minutes * 60);
            if (timerText != null) timerText.text = string.Format("{0:00}:{1:00}", minutes, seconds);
            yield return null;
        }
    }

    private void OnQuitPanelButtonClicked() {
        audioManager.PlaySfx(audioManager.yesButton);
        
        StopFocusUpTimer();
        currencyManager.moneyMultiplier -= 0.5f;
        currencyManager.UpdateUI();
        Debug.Log("Money Multiplier decreased to: " + currencyManager.moneyMultiplier);

        if (focusUpEventUI != null) focusUpEventUI.SetActive(false);
        isFocusUpEventActive = false;
    }

    private void StopFocusUpTimer() {
        if (focusUpTimerCoroutine != null)
        {
            StopCoroutine(focusUpTimerCoroutine);
            focusUpTimerCoroutine = null;
        }

        totalFocusTime = elapsedTime / 60;

        if (totalFocusTime >= 5 && totalFocusTime < 15) {
            currencyManager.addCash(1.1 * currencyManager.currencyInGame);
        }
        else if (totalFocusTime >= 15 && totalFocusTime < 25) {
            currencyManager.addCash(1.25 * currencyManager.currencyInGame);
        }
        else if (totalFocusTime >= 25 && totalFocusTime < 35) {
            currencyManager.addCash(1.5 * currencyManager.currencyInGame);
        }

        elapsedTime = 0;
    }
}