using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CashManagerScript : MonoBehaviour
{
    [Header("Cash Spawning Settings")]
    [SerializeField] private float timeToSpawn;
    [SerializeField] private float minX;
    [SerializeField] private float maxX;

    [Header("Cash Prefab")]
    [SerializeField] private GameObject cashPrefab;
    [SerializeField] private GameObject parentsInEnvironment;

    public bool isCashDropEventActive = false;

    void Start()
    {
        StartCoroutine(RegularCashSpawn());
    }

    IEnumerator RegularCashSpawn()
    {
        while (true)
        {
            // Wait for the next spawn interval
            yield return new WaitForSeconds(timeToSpawn);

            // Spawn cash if the cash drop event is not active
            if (!isCashDropEventActive)
            {
                SpawnCash();
            }
        }
    }

    public void StartCashDropEvent()
    {
        StartCoroutine(CashDropEvent());
    }

    IEnumerator CashDropEvent()
    {
        // Activate the cash drop event
        isCashDropEventActive = true;

        yield return StartCoroutine(spawnCash(0.1f, 3f));

        // Deactivate the cash drop event
        isCashDropEventActive = false;
    }

    IEnumerator spawnCash(float spawnInterval, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            // Set a random spawn location
            var spawnLocation = Random.Range(minX, maxX);
            var position = new Vector3(spawnLocation, transform.position.y, 0);

            // Spawn the Cash Prefab
            GameObject cash = Instantiate(cashPrefab, position, Quaternion.identity);
            cash.transform.SetParent(parentsInEnvironment.transform);

            // Wait for the next spawn
            yield return new WaitForSeconds(spawnInterval);

            // Update the elapsed time
            elapsedTime += spawnInterval;
        }
    }

    private void SpawnCash()
    {
        // Set a random spawn location
        var spawnLocation = Random.Range(minX, maxX);
        var position = new Vector3(spawnLocation, transform.position.y, 0);

        // Spawn the Cash Prefab
        GameObject cash = Instantiate(cashPrefab, position, Quaternion.identity);
        cash.transform.SetParent(parentsInEnvironment.transform);
    }
}