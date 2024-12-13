using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MovementScript : MonoBehaviour
{
    CashPrefabScript cashPrefabScript;

    [SerializeField] GameObject pointA;
    [SerializeField] GameObject pointB;
    [SerializeField] GameObject pointC;

    [SerializeField] Rigidbody2D rb;
    [SerializeField] Animator animator;
    [SerializeField] Transform currentPoint;

    [SerializeField] float detectionRadius = 5f;
    [SerializeField] float speed = 10f;

    private GameObject cash;
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
        if (scene.name != "GameplayScene") {
            gameObject.SetActive(false);
        } else {
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
        pointA = GameObject.Find("PointA");
        pointB = GameObject.Find("PointB");
        pointC = GameObject.Find("PointC");
    }

    private void Update() 
    {
        FindCash();

        if (IsCashNearby()) 
        {
            animator.SetBool("isWalking", true);
        } 
        else 
        {
            animator.SetBool("isWalking", false);
        }

        if (animator.GetBool("isWalking")) 
        {
            if (IsCashNearby()) 
            {
                MoveTowards(cash.transform.position);
            } 
            else 
            {
                MoveTowards(pointB.transform.position);
            }
        }
    }

    private void FindCash() 
    {
        if (cash == null) 
        {
            cash = GameObject.FindGameObjectWithTag("Cash");
            if (cash != null) 
            {
                cashPrefabScript = cash.GetComponent<CashPrefabScript>();
            }
        }
    }

    private bool IsCashNearby() 
    {
        if (cash == null) return false;

        return Vector2.Distance(pointA.transform.position, cash.transform.position) <= detectionRadius ||
               Vector2.Distance(pointB.transform.position, cash.transform.position) <= detectionRadius ||
               Vector2.Distance(pointC.transform.position, cash.transform.position) <= detectionRadius;
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
            Destroy(other.gameObject);
            if (cashPrefabScript != null) 
            {
                cashPrefabScript.OnCashCollected();
            }
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