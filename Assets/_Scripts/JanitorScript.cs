using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class JanitorScript : MonoBehaviour
{
    [SerializeField] GameObject pointB;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] Transform currentPoint;

    [SerializeField] float speed = 10f;

    private GameObject nearestCash;
    private bool facingRight = true;

    private void Awake() 
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable() 
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable() 
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SavePosition();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode) 
    {
        if (scene.name != "GameplayScene") 
        {
            gameObject.SetActive(false);
        } 
        else if(scene.name == "GameplayScene" && CurrencyManagerScript.isJanitorHired == 1)
        {
            gameObject.SetActive(true);
            FindPointsInEnvironment();
            LoadPosition();
        }
    }

    private void Start() 
    {   
        FindPointsInEnvironment();

        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        currentPoint = pointB.transform;
        animator.SetBool("isWalking", false);
    }

    private void FindPointsInEnvironment() 
    {
        pointB = GameObject.Find("PointB");
    }

    private void Update() 
    {
        FindNearestCash();

        if (nearestCash != null) 
        {
            animator.SetBool("isWalking", true);
            MoveTowards(nearestCash.transform.position);
        } 
        else 
        {
            animator.SetBool("isWalking", false);
        }
    }

    private void FindNearestCash() 
    {
        GameObject[] cashObjects = GameObject.FindGameObjectsWithTag("Cash");
        float nearestDistance = Mathf.Infinity;
        nearestCash = null;

        foreach (GameObject cash in cashObjects) 
        {
            CashPrefabScript cashScript = cash.GetComponent<CashPrefabScript>();
            if (cashScript != null && cashScript.IsOnGround()) 
            {
                float distance = Vector2.Distance(transform.position, cash.transform.position);
                if (distance < nearestDistance) 
                {
                    nearestDistance = distance;
                    nearestCash = cash;
                }
            }
        }
    }

    private void MoveTowards(Vector2 target) 
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        Vector2 newPosition = new Vector2(rb.position.x + direction.x * speed * Time.deltaTime, rb.position.y);
        rb.MovePosition(newPosition);

        if (direction.x > 0 && !facingRight) 
        {
            Flip();
        } 
        else if (direction.x < 0 && facingRight) 
        {
            Flip();
        }
    }

    private void Flip() 
    {
        facingRight = !facingRight;
        Vector3 scale = transform.localScale;
        scale.x *= -1;
        transform.localScale = scale;
    }

    private void OnCollisionEnter2D(Collision2D other) 
    {
        if (other.gameObject.CompareTag("Cash")) 
        {
            CashPrefabScript cashScript = other.gameObject.GetComponent<CashPrefabScript>();
            if (cashScript != null) 
            {
                cashScript.OnCashCollected();
            }
            else
            {
                Debug.LogError("CashPrefabScript is null");
            }
            Destroy(other.gameObject);
        }
    }

    private void SavePosition() 
    {
        PlayerPrefs.SetFloat("PlayerX", transform.position.x);
        PlayerPrefs.SetFloat("PlayerY", transform.position.y);
        PlayerPrefs.SetFloat("PlayerZ", transform.position.z);
        PlayerPrefs.Save();
    }

    private void LoadPosition() 
    {
        float x = PlayerPrefs.GetFloat("PlayerX", transform.position.x);
        float y = PlayerPrefs.GetFloat("PlayerY", transform.position.y);
        float z = PlayerPrefs.GetFloat("PlayerZ", transform.position.z);
        transform.position = new Vector3(x, y, z);
    }
}