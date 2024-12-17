using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    private IEnumerator MoveStudentThroughPoints(GameObject student, int index)
    {
        int waypointIndex = index / 4;
        yield return StartCoroutine(MoveStudent(student, wayPoints[waypointIndex]));

        yield return new WaitForSeconds(1f);

        int sidePointIndex = index / 2;
        yield return StartCoroutine(MoveStudent(student, sidePoints[sidePointIndex]));

        yield return new WaitForSeconds(1f);

        yield return StartCoroutine(MoveStudent(student, endPoints[index]));
    }

    private IEnumerator MoveStudent(GameObject student, GameObject targetPoint)
    {
        Rigidbody2D rb = student.GetComponent<Rigidbody2D>();
        Vector2 targetPosition = targetPoint.transform.position;

        while (Vector2.Distance(student.transform.position, targetPosition) > 0.01)
        {
            Vector2 direction = (targetPosition - (Vector2)student.transform.position).normalized;
            Vector2 newPosition = (Vector2)student.transform.position + direction * speed * Time.deltaTime;

            rb.MovePosition(newPosition);

            yield return null;
        }
        Debug.Log("Location reached: " + student.name);
    }
}
