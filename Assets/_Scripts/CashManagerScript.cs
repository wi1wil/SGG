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

    void Start() 
    {
        StartCoroutine(spawnCash());
    }

    IEnumerator spawnCash()
    {
        while(true) 
        {
            // Set a random spawn location
            var spawnLocation = Random.Range(minX, maxX);
            var position = new Vector3(spawnLocation, transform.position.y, 0);

            // Spawn the Cash Prefab
            GameObject cash = Instantiate(cashPrefab, position, Quaternion.identity);
            cash.transform.SetParent(parentsInEnvironment.transform);

            // Wait for the next spawn
            yield return new WaitForSeconds(timeToSpawn);
        }
    }

    
}
