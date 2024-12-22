using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class StudentBehaviourScript : MonoBehaviour
{
    CurrencyManagerScript currencyManager;

    [Header("Enroll Students")]
    [SerializeField] private GameObject[] activateStudents;

    [Header("Student Prefabs")]
    [SerializeField] private GameObject[] students;

    [Header("Traversal Points")]
    [SerializeField] private GameObject[] wayPoints;
    [SerializeField] private GameObject[] sidePoints;
    [SerializeField] private GameObject[] endPoints;

    [SerializeField] private float speed;

    private void Awake() {
        currencyManager = FindObjectOfType<CurrencyManagerScript>();
    }

    void Start()
    {
        LoadStudents();

        for (int i = 0; i < students.Length; i++)
        {
            StartCoroutine(MoveStudentThroughPoints(students[i], i));
        }
    }

    public void LoadStudents()
    {
        for (int i = 0; i < currencyManager.totalStudents; i++)
        {
            activateStudents[i].gameObject.SetActive(true);
        }
    }

    public void ActivateStudent(int index)
    {
        activateStudents[index].gameObject.SetActive(true);
        StartCoroutine(MoveStudentThroughPoints(students[index], index));
    }

    private IEnumerator MoveStudentThroughPoints(GameObject student, int index)
    {
        int waypointIndex = index / 4;
        yield return StartCoroutine(MoveStudent(student, wayPoints[waypointIndex]));

        int sidePointIndex = index / 2;
        yield return StartCoroutine(MoveStudent(student, sidePoints[sidePointIndex]));

        yield return StartCoroutine(MoveStudent(student, endPoints[index]));

        student.transform.position = new Vector3(student.transform.position.x, student.transform.position.y - 0.8f, student.transform.position.z);
    }

    private IEnumerator MoveStudent(GameObject student, GameObject targetPoint)
    {
        Animator animator = student.GetComponent<Animator>();
        Rigidbody2D rb = student.GetComponent<Rigidbody2D>();
        Vector2 targetPosition = targetPoint.transform.position;

        while (Vector2.Distance(student.transform.position, targetPosition) > 0.05)
        {
            animator.SetBool("sWalking", true);
            Vector2 direction = (targetPosition - (Vector2)student.transform.position).normalized;
            Vector2 newPosition = (Vector2)student.transform.position + direction * speed * Time.deltaTime;

            if (direction.x > 0)
            {
                student.transform.localScale = new Vector3(1, 1, 1);
            }
            else if (direction.x < 0)
            {
                student.transform.localScale = new Vector3(-1, 1, 1);
            }

            rb.MovePosition(newPosition);

            yield return null;
        }
        animator.SetBool("sWalking", false);
    }
}
