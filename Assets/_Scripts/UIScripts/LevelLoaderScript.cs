using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    [Header("Transition Settings")]
    public Animator transition;
    float transitionTime = 0f;

    public void LoadNextLevel() 
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void loadMenu() 
    {
        StartCoroutine(LoadLevel(0));
    }

    public void LoadPrevLevel() 
    {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex - 1));
    }

    public IEnumerator LoadLevel(int levelIndex) 
    {
        transition.SetTrigger("Start");

        yield return new WaitForSeconds(transitionTime);

        SceneManager.LoadSceneAsync(levelIndex);
    }
}
