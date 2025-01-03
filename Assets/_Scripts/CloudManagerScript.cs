using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class CloudManagerScript : MonoBehaviour
{
    [Header("Cloud Prefabs")]
    [SerializeField] private GameObject[] clouds;

    [Header("Cloud EndPoint")]
    [SerializeField] private GameObject endPoint;

    [Header("Cloud Parents")]
    [SerializeField] private GameObject cloudParent;
    
    [Header("Cloud Spawn Interval")]
    public float spawnInterval;

    Vector3 startPos;

    void Start() 
    {
        startPos = transform.position; 

        Prespawn();

        Invoke("AttemptSpawn", spawnInterval);
    }

    void SpawnClouds(Vector3 startPos) 
    {
        int randomIndex = UnityEngine.Random.Range(0, clouds.Length);

        GameObject cloud = Instantiate(clouds[randomIndex], Vector3.zero, Quaternion.identity);
        cloud.transform.SetParent(cloudParent.transform);

        float startY = UnityEngine.Random.Range(startPos.y - 1f, startPos.y + 1.5f);
        cloud.transform.position = new Vector3(startPos.x, startY, startPos.z);

        float scale = UnityEngine.Random.Range(0.8f, 1.2f);
        cloud.transform.localScale = new Vector2(scale, scale);

        float speed = UnityEngine.Random.Range(1f, 2f);
        cloud.GetComponent<CloudPrefabScript>().startFloating(speed, endPoint.transform.position.x);

        float randomOpacity = UnityEngine.Random.Range(0.5f, 1f); 
        SpriteRenderer cloudRenderer = cloud.GetComponent<SpriteRenderer>();
        if (cloudRenderer != null) {
            Color cloudColor = cloudRenderer.color;
            cloudColor.a = randomOpacity;
            cloudRenderer.color = cloudColor;
        }

    }

    void AttemptSpawn()
    {
        SpawnClouds(startPos);
        Invoke("AttemptSpawn", spawnInterval);
    }

    void Prespawn() 
    {
        for(int i = 0; i < 15; i++) 
        {
            float randomX = UnityEngine.Random.Range(startPos.x - 1f, startPos.x + 20f);
            float randomY = UnityEngine.Random.Range(startPos.y - 1.5f, startPos.y + 1.5f);

            Vector3 spawnPos = new Vector3(randomX, randomY, startPos.z);
            SpawnClouds(spawnPos);
        }
    }
    
}
