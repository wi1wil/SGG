
using System.Collections;
using UnityEngine;

public class CountDownTimer : MonoBehaviour
{
    public float remainingTime;
    private bool isRunning;

    public void StartTimer(float duration)
    {
        remainingTime = duration;
        isRunning = true;
        StartCoroutine(UpdateTimer());
    }

    private IEnumerator UpdateTimer()
    {
        while (remainingTime > 0)
        {
            remainingTime -= Time.deltaTime;
            yield return null;
        }
        isRunning = false;
    }

    public bool IsRunning()
    {
        return isRunning;
    }
}