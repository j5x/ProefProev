using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public GameObject loadingScreen; // Reference to the loading screen UI
    public Image loadingGif; // Reference to the Image component for the GIF
    public Sprite[] gifFrames; // Array of frames for the GIF
    public float frameRate = 10f; // Frames per second for the GIF animation
    public float minimumLoadTime = 3f; // Minimum time to show the loading screen (in seconds)
    public AudioClip loadingMusic; // Background music to play during loading

    private AsyncOperation loadingOperation;
    private float frameTimer;
    private int currentFrame;
    private AudioSource audioSource;

    private void Awake()
    {
        // Add an AudioSource component for playing the loading music
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.loop = true; // Loop the music
    }

    // Call this method to load a specific scene
    public void LoadGameScene(string sceneName)
    {
        // Show the loading screen
        loadingScreen.SetActive(true);

        // Hide the menu panel (optional)
        GameObject menuPanel = GameObject.Find("MenuPanel");
        if (menuPanel != null)
        {
            menuPanel.SetActive(false);
        }

        // Stop the menu music
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.StopMusic();
        }

        // Play the loading music
        if (loadingMusic != null)
        {
            audioSource.clip = loadingMusic;
            audioSource.Play();
        }

        // Start loading the game scene asynchronously
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);

        // Don't allow the scene to activate immediately
        loadingOperation.allowSceneActivation = false;

        // Start the loading process
        StartCoroutine(LoadSceneAsync());
    }

    private System.Collections.IEnumerator LoadSceneAsync()
    {
        float progress = 0f;
        float elapsedTime = 0f;

        // Simulate loading progress
        while (progress < 1f || elapsedTime < minimumLoadTime)
        {
            // Update the elapsed time
            elapsedTime += Time.deltaTime;

            // Update the loading progress
            if (loadingOperation.progress >= 0.9f)
            {
                progress = Mathf.Clamp01(elapsedTime / minimumLoadTime); // Use elapsed time for progress
            }
            else
            {
                progress = Mathf.Clamp01(loadingOperation.progress / 0.9f); // Normalize progress to 0-1
            }

            // Update the GIF animation
            UpdateGifAnimation();

            // Check if loading is complete and minimum load time has passed
            if (loadingOperation.progress >= 0.9f && elapsedTime >= minimumLoadTime)
            {
                // Allow the scene to activate
                loadingOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Stop the loading music
        if (audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // Hide the loading screen (optional, since the new scene will load)
        loadingScreen.SetActive(false);
    }

    private void UpdateGifAnimation()
    {
        // Update the GIF frame based on the frame rate
        frameTimer += Time.deltaTime;
        if (frameTimer >= 1f / frameRate)
        {
            frameTimer = 0f;
            currentFrame = (currentFrame + 1) % gifFrames.Length;
            loadingGif.sprite = gifFrames[currentFrame];
        }
    }
}