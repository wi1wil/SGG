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
            yield return new WaitForSeconds(timeToSpawn);

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
        isCashDropEventActive = true;

        yield return StartCoroutine(spawnCash(0.1f, 3f));

        isCashDropEventActive = false;
    }

    IEnumerator spawnCash(float spawnInterval, float duration)
    {
        float elapsedTime = 0f;

        while (elapsedTime < duration)
        {
            var spawnLocation = Random.Range(minX, maxX);
            var position = new Vector3(spawnLocation, transform.position.y, 0);

            GameObject cash = Instantiate(cashPrefab, position, Quaternion.identity);
            cash.transform.SetParent(parentsInEnvironment.transform);

            yield return new WaitForSeconds(spawnInterval);

            elapsedTime += spawnInterval;
        }
    }

    private void SpawnCash()
    {
        var spawnLocation = Random.Range(minX, maxX);
        var position = new Vector3(spawnLocation, transform.position.y, 0);

        GameObject cash = Instantiate(cashPrefab, position, Quaternion.identity);
        cash.transform.SetParent(parentsInEnvironment.transform);
    }
}