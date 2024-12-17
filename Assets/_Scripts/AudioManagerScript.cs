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
    public AudioClip noButton;
    public AudioClip yesButton;
    public AudioClip moneyRainEvent;
    public AudioClip buildingSFX;
    public AudioClip openDoorSFX;
    public AudioClip buySuccessSFX;

    private void Awake() 
    {
        if (instance == null) {
            instance = this;
            DontDestroyOnLoad(gameObject);
        } else if (instance != this) {
            Destroy(gameObject);
        }
    }

    // Start playing the background music
    private void Start() 
    {
        musicSource.clip = backgroundMusic;
        musicSource.Play();
    }

    // Calling the AudioClip to play
    public void PlaySfx(AudioClip clip) 
    {
        sfxSource.PlayOneShot(clip);
    }
}