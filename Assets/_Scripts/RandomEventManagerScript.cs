using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RandomEventsManager : MonoBehaviour
{
    private static RandomEventsManager instance;

    [Header("Random Event Text")]
    [SerializeField] private TextMeshProUGUI randomEventTextPrefab;
    [SerializeField] private GameObject parentsInEnvironment;

    [Header("Random Event Settings")]
    [SerializeField] private float secondsBetweenEvents;

    [Header("Countdown Timer")]
    public float countdownTimer;

    CashManagerScript cashManagerScript;
    FocusUpScript focusUpEventScript;
    AudioManagerScript audioManager;

    Vector3 position = new Vector3(0, 2.5f, 0);

    private void Awake() 
    {
        cashManagerScript = FindObjectOfType<CashManagerScript>();
        focusUpEventScript = FindObjectOfType<FocusUpScript>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
    }

    private void Start()
    {
        StartCoroutine(RandomEvent());
    }

    IEnumerator RandomEvent()
    {
        while (true)
        {
            // Randomize the timer for the next event
            countdownTimer = Random.Range(1f * secondsBetweenEvents, 5f * secondsBetweenEvents);

            // Update the countdown timer in real-time
            while (countdownTimer > 0)
            {
                if (!focusUpEventScript.isFocusUpEventActive)
                {
                    countdownTimer -= Time.deltaTime;
                }
                yield return null;
            }

            // Check if FocusUpEvent is active
            if (focusUpEventScript.isFocusUpEventActive)
            {
                // Wait until the FocusUpEvent is finished
                yield return new WaitUntil(() => !focusUpEventScript.isFocusUpEventActive);
            }

            int eventIndex = Random.Range(0, 2); // Randomly choose between events
            switch (eventIndex)
            {
                case 0:
                    TextMeshProUGUI eventText = Instantiate(randomEventTextPrefab, position, Quaternion.identity);
                    eventText.transform.SetParent(parentsInEnvironment.transform, false);
                    eventText.text = "-= Money Rain =- \nEvent On Going!";
                    audioManager.PlaySfx(audioManager.moneyRainEvent);
                    
                    cashManagerScript.StartCashDropEvent();

                    StartCoroutine(DestroyEventText(eventText.gameObject, 5f));
                    break;
                case 1:
                    eventText = Instantiate(randomEventTextPrefab, position, Quaternion.identity);
                    eventText.transform.SetParent(parentsInEnvironment.transform, false);
                    eventText.text = "-= Focus Up! =- \nEvent On Going!";
                    audioManager.PlaySfx(audioManager.moneyRainEvent);

                    StartCoroutine(StartFocusUpEvent());
                    StartCoroutine(DestroyEventText(eventText.gameObject, 5f));
                    break;
            }
        }
    }

    private IEnumerator StartFocusUpEvent()
    {
        focusUpEventScript.StartFocusUpEvent();
        yield return new WaitUntil(() => !focusUpEventScript.isFocusUpEventActive);
    }

    private IEnumerator DestroyEventText(GameObject eventText, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(eventText);
    }
}