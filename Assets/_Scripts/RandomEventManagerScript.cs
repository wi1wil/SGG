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

    CashManagerScript cashManagerScript;

    Vector3 position = new Vector3(0, 2.5f, 0);

    private void Awake() 
    {
        cashManagerScript = FindObjectOfType<CashManagerScript>();
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
            float randomEventTimer = Random.Range(1f * secondsBetweenEvents, 5f * secondsBetweenEvents);
            yield return new WaitForSeconds(randomEventTimer);

            int eventIndex = 0;
            switch (eventIndex)
            {
                case 0:
                    TextMeshProUGUI eventText = Instantiate(randomEventTextPrefab, position, Quaternion.identity);
                    eventText.transform.SetParent(parentsInEnvironment.transform, false);
                    eventText.text = "-= Money Rain =- \nEvent On Going!";
                    
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