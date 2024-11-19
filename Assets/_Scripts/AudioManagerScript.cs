using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManagerScript : MonoBehaviour
{
    private static AudioManagerScript instance;

    [Header("----------------------------------- Audio Sources -----------------------------------")]
    [SerializeField] AudioSource musicSource;
    [SerializeField] AudioSource sfxSource;

    [Header("-----------------------------------  Audio Clips  -----------------------------------")]
    public AudioClip backgroundMusic;
    public AudioClip moneyGrab;
    public AudioClip buttonClick;

    private void Awake() {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    private void Start() {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    public void PlaySfx(AudioClip clip) {
        sfxSource.PlayOneShot(clip);
    }
}