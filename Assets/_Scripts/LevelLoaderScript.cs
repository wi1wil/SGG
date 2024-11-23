using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoaderScript : MonoBehaviour
{
    [Header("Transition Settings")]
    public Animator transition;
    float transitionTime = 0f;

    public void LoadNextLevel() {
        StartCoroutine(LoadLevel(SceneManager.GetActiveScene().buildIndex + 1));
    }

    public void loadMenu() {
        StartCoroutine(LoadLevel(0));
    }

    public IEnumerator LoadLevel(int levelIndex) {
        // Trigger the Animations
        transition.SetTrigger("Start");

        // Wait for Transition Time
        yield return new WaitForSeconds(transitionTime);

        // Load the Scene in the Background
        SceneManager.LoadSceneAsync(levelIndex);
    }
}
