using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TeacherScript : MonoBehaviour
{
    [Header("Roaming Settings")]
    [SerializeField] private List<GameObject> points;

    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private Animator animator;
    [SerializeField] private Transform currentPoint;

    [SerializeField] private float speed;
    [SerializeField] private float idleTime = 2f;
    private float idleTimer;

    private bool facingRight = true;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

        animator = GetComponent<Animator>();
        currentPoint = points[0].transform; 
        animator.SetBool("tWalking", false);
        idleTimer = idleTime;
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        LoadPosition();
    }

    private void Update()
    {
        if (idleTimer > 0)
        {
            idleTimer -= Time.deltaTime;
            animator.SetBool("tWalking", false);
        }
        else
        {
            MoveBetweenPoints();
        }
    }

    private void MoveBetweenPoints()
    {
        for (int i = 0; i < points.Count; i++)
        {
            if (currentPoint == points[i].transform)
            {
                int nextIndex = (i + 1) % points.Count;
                MoveTowards(points[nextIndex].transform.position);
                if (Vector2.Distance(transform.position, points[nextIndex].transform.position) < 0.1f)
                {
                    currentPoint = points[nextIndex].transform;
                    idleTimer = idleTime;
                }
                break;
            }
        }
    }

    private void ChooseRandomPoint()
    {
        int randomIndex = Random.Range(0, points.Count);
        currentPoint = points[randomIndex].transform;
    }

    private void MoveTowards(Vector2 target)
    {
        Vector2 direction = (target - (Vector2)transform.position).normalized;
        transform.Translate(direction * speed * Time.deltaTime, Space.World);
        animator.SetBool("tWalking", true);

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

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SavePosition();
    }
    
    private void OnApplicationQuit()
    {
        SavePosition();
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadPosition();
    }

    private void OnSceneUnloaded(Scene current)
    {
        SavePosition();
    }

    private void SavePosition()
    {
        PlayerPrefs.SetFloat("TeacherPosX", transform.position.x);
        PlayerPrefs.SetFloat("TeacherPosY", transform.position.y);
        PlayerPrefs.SetFloat("TeacherPosZ", transform.position.z);
    }

    private void LoadPosition()
    {
        float x = PlayerPrefs.GetFloat("TeacherPosX", transform.position.x);
        float y = PlayerPrefs.GetFloat("TeacherPosY", transform.position.y);
        float z = PlayerPrefs.GetFloat("TeacherPosZ", transform.position.z);
        transform.position = new Vector3(x, y, z);
    }
}