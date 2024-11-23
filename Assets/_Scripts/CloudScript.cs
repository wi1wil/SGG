using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;

public class CloudPrefabScript : MonoBehaviour
{
    private float _speed = 2;
    private float _endPosX;

    // Set the speed and end position of the cloud
    public void startFloating(float speed, float endPosX) 
    {
        _speed = speed;
        _endPosX = endPosX;
    }

    // Move the cloud to the right and destroy it when it reaches the end position
    void Update()
    {
        transform.Translate(Vector3.right * (Time.deltaTime * _speed));

        if(transform.position.x > _endPosX){
            Destroy(gameObject);
        }
    }
}