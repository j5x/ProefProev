using UnityEngine;

public class BackgroundMusicManagerStart : MonoBehaviour
{
    public static BackgroundMusicManagerStart Instance;

    public AudioClip menuMusic; // Background music for the main menu
    private AudioSource audioSource;

    private void Awake()
    {
        // Ensure only one instance of the BackgroundMusicManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate instances
            return;
        }

        // Set up the AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = false; // Loop the music
    }

    private void Start()
    {
        // Play the menu music when the game starts
        PlayMenuMusic();
    }

    public void PlayMenuMusic()
    {
        if (menuMusic != null && audioSource != null)
        {
            audioSource.clip = menuMusic;
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}