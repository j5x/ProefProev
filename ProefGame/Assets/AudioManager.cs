using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [Header("Music")]
    public AudioClip menuMusic; // Music for the main menu
    public AudioClip loadingMusic; // Music during loading

    private AudioSource audioSource;

    private void Awake()
    {
        // Singleton pattern
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true;
    }

    // Play music by type (e.g., "menu", "loading")
    public void PlayMusic(string musicType)
    {
        AudioClip clip = null;

        switch (musicType.ToLower())
        {
            case "menu":
                clip = menuMusic;
                break;
            case "loading":
                clip = loadingMusic;
                break;
        }

        if (clip != null && audioSource.clip != clip)
        {
            audioSource.clip = clip;
            audioSource.Play();
        }
    }

    public void StopMusic()
    {
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
}