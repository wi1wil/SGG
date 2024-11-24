using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PopUpScript : MonoBehaviour
{
    [Header("Pop Up Settings")]
    public float destroyTime = 2f;
    public Vector3 Offset = new Vector3(0, 1, 0);
    public Vector3 RandomizeIntensity = new Vector3(0.5f, 0.5f, 0);

    void Start () {
        // Destroy the Pop Up after destroyTime
        Destroy(gameObject, destroyTime);
        
        // Randomize the Pop Up Position
        transform.localPosition += Offset;
        transform.localPosition += new Vector3(
            Random.Range(-RandomizeIntensity.x, RandomizeIntensity.x),
            Random.Range(-RandomizeIntensity.y, RandomizeIntensity.y),
            Random.Range(-RandomizeIntensity.z, RandomizeIntensity.z)
        );
    }
}

