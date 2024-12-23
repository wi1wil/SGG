using System;
using System.Collections;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomEventsManager : MonoBehaviour
{
    private static RandomEventsManager instance;

    [Header("Random Event Text")]
    [SerializeField] private TextMeshProUGUI randomEventTextPrefab;
    [SerializeField] private GameObject parentsInEnvironment;

    [Header("Countdown Timer")]
    [SerializeField] private TextMeshProUGUI countdownText;
    public float countdownTimer;

    CashManagerScript cashManagerScript;
    FocusUpScript focusUpEventScript;
    AudioManagerScript audioManager;

    Vector3 position = new Vector3(0, 2.5f, 0);

    private void Awake() 
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        cashManagerScript = FindObjectOfType<CashManagerScript>();
        focusUpEventScript = FindObjectOfType<FocusUpScript>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
        
        LoadData();

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() 
    {
        SaveData();
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void Start()
    {
        FindParentsInEnvironment();
        FindCountdownText();
        if (focusUpEventScript != null)
        {
            StartCoroutine(RandomEvent());
        }
        else
        {
            Debug.LogError("FocusUpScript not found!");
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        cashManagerScript = FindObjectOfType<CashManagerScript>();
        focusUpEventScript = FindObjectOfType<FocusUpScript>();
        FindParentsInEnvironment();
        FindCountdownText();
    }

    private void FindParentsInEnvironment()
    {
        GameObject canvas = GameObject.Find("MainCanvas");
        if (canvas != null)
        {
            Transform parentInEnvironment = canvas.transform.Find("CurrencyManager");
            if (parentInEnvironment != null)
            {
                parentsInEnvironment = parentInEnvironment.gameObject;
            }
        }

        if (parentsInEnvironment == null)
        {
            parentsInEnvironment = GameObject.Find("MainCanvas");
        }
    }

    private void FindCountdownText()
    {
        countdownText = GameObject.FindGameObjectWithTag("CountdownText")?.GetComponent<TextMeshProUGUI>();
    }

    private void LoadData()
    {
        CurrencyManagerScript.doubleMultiplier = PlayerPrefs.GetInt("DoubleMultiplier", 1);
        CurrencyManagerScript.doubleCashValue = PlayerPrefs.GetInt("DoubleCashValue", 1);
    }

    private void SaveData()
    {
        PlayerPrefs.SetInt("DoubleMultiplier", CurrencyManagerScript.doubleMultiplier);
        PlayerPrefs.SetInt("DoubleCashValue", CurrencyManagerScript.doubleCashValue);
        PlayerPrefs.Save();
    }

    IEnumerator RandomEvent()
    {
        if (focusUpEventScript == null)
        {
            yield break;
        }

        while (true)
        {
            countdownTimer = 150;

            while (countdownTimer > 0)
            {
                float minutes = Mathf.Floor(countdownTimer / 60);
                float seconds = Mathf.RoundToInt(countdownTimer % 60);

                if (countdownText != null)
                {
                    countdownText.text = "Next Event in: " + string.Format("{0:00}:{1:00}", minutes, seconds);
                }

                if (!focusUpEventScript.isFocusUpEventActive)
                {
                    countdownTimer -= Time.deltaTime;
                }
                yield return null;
            }

            if (focusUpEventScript.isFocusUpEventActive)
            {
                yield return new WaitUntil(() => !focusUpEventScript.isFocusUpEventActive);
            }

            int eventIndex;
            if(SceneManager.GetActiveScene().name == "GameplayScene")
            {
                eventIndex = UnityEngine.Random.Range(0, 3);
            } 
            else 
            {
                eventIndex = UnityEngine.Random.Range(1, 3);
            }

            switch (eventIndex)
            {
                case 0:
                    CreateEventText("-= Money Rain =- \nEvent On Going!", audioManager.moneyRainEvent);
                    cashManagerScript.StartCashDropEvent();
                    break;
                case 1:
                    CreateEventText("-= Focus Up! =- \nEvent On Going!", audioManager.moneyRainEvent);
                    StartCoroutine(StartFocusUpEvent());
                    break;
                case 2:
                    CreateEventText("-= Double Fallen Cash =- \nEvent On Going!", audioManager.moneyRainEvent);
                    CurrencyManagerScript.doubleCashValue = 2;
                    SaveData();
                    StartCoroutine(ResetDoubleEventMultiplier(30f));
                    break;
                case 3:
                    CreateEventText("-= Double Multiplier! =- \nEvent On Going!", audioManager.moneyRainEvent);
                    CurrencyManagerScript.doubleMultiplier = 2;
                    SaveData();
                    StartCoroutine(ResetDoubleEventMultiplier(30f));
                    break;
            }
        }
    }

    private void CreateEventText(string text, AudioClip audioClip)
    {
        var popUp = Instantiate(randomEventTextPrefab, position, Quaternion.identity, parentsInEnvironment.transform);
        popUp.GetComponent<TextMeshProUGUI>().text = text;
        audioManager.PlaySfx(audioClip);

        StartCoroutine(DestroyEventText(popUp.gameObject, 5f));
    }

    private IEnumerator ResetDoubleEventMultiplier(float duration)
    {
        yield return new WaitForSeconds(duration);
        CurrencyManagerScript.doubleCashValue = 1;
        CurrencyManagerScript.doubleMultiplier = 1;
        CreateEventText("-= Event Ended! =-", audioManager.moneyRainEvent);
        SaveData();
    }

    private IEnumerator StartFocusUpEvent()
    {
        focusUpEventScript.StartFocusUpEvent();
        yield return new WaitUntil(() => !focusUpEventScript.isFocusUpEventActive);
    }

    private IEnumerator DestroyEventText(GameObject eventTextParent, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(eventTextParent);
    }
}