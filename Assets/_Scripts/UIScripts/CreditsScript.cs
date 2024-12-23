using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditsScript : MonoBehaviour
{
    AudioManagerScript audioManager;

    [Header("Credits Panel")]
    [SerializeField] private GameObject creditPanel;
    [SerializeField] private Button backButton;
    [SerializeField] private Button creditsButton;

    private void Awake() {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
    }

    private void Start()
    {
        creditPanel.SetActive(false);
        backButton.onClick.AddListener(() => {
            creditPanel.SetActive(false);
            audioManager.PlaySfx(audioManager.noButton);
        });

        creditsButton.onClick.AddListener(() => {
            creditPanel.SetActive(true);
            audioManager.PlaySfx(audioManager.yesButton);
        });
    }

}
