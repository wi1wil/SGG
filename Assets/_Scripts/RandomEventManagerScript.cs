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
    AudioManagerScript audioManager;

    Vector3 position = new Vector3(0, 2.5f, 0);

    private void Awake() 
    {
        cashManagerScript = FindObjectOfType<CashManagerScript>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
    }

    private void Start()
    {
        StartCoroutine(RandomCashDropEvent());
    }

    IEnumerator RandomCashDropEvent()
    {
        while (true)
        {
            // Randomize the timer for the next event
            countdownTimer = Random.Range(1f * secondsBetweenEvents, 5f * secondsBetweenEvents);

            // Update the countdown timer in real-time
            while (countdownTimer > 0)
            {
                yield return null; // Wait for the next frame
                countdownTimer -= Time.deltaTime;
            }

            int eventIndex = 0;
            switch (eventIndex)
            {
                case 0:
                    TextMeshProUGUI eventText = Instantiate(randomEventTextPrefab, position, Quaternion.identity);
                    eventText.transform.SetParent(parentsInEnvironment.transform, false);
                    eventText.text = "-= Money Rain =- \nEvent On Going!";
                    audioManager.PlaySfx(audioManager.randomEvents);
                    
                    cashManagerScript.StartCashDropEvent();

                    StartCoroutine(DestroyEventText(eventText.gameObject, 5f));
                    break;
                case 1:
                    // StartAnotherEvent();
                    break;
            }
        }
    }

    private IEnumerator DestroyEventText(GameObject eventText, float duration)
    {
        yield return new WaitForSeconds(duration);
        Destroy(eventText);
    }
}