using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoadingScreen : MonoBehaviour
{
    public static LoadingScreen Instance;

    [Header("UI Settings")]
    public GameObject loadingScreen; // Loading screen UI panel
    public Image loadingGif;
    public Sprite[] gifFrames;
    public float frameRate = 10f;
    public float minimumLoadTime = 3f;

    private AsyncOperation loadingOperation;
    private float frameTimer;
    private int currentFrame;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void LoadScene(string sceneName)
    {
        // Show loading screen
        loadingScreen.SetActive(true);

        // Play loading music
        AudioManager.Instance.PlayMusic("loading");

        // Start async load
        loadingOperation = SceneManager.LoadSceneAsync(sceneName);
        loadingOperation.allowSceneActivation = false;

        StartCoroutine(LoadSceneAsync());
    }

    private System.Collections.IEnumerator LoadSceneAsync()
    {
        float progress = 0f;
        float elapsedTime = 0f;

        while (progress < 1f || elapsedTime < minimumLoadTime)
        {
            elapsedTime += Time.deltaTime;
            progress = Mathf.Clamp01(loadingOperation.progress / 0.9f);

            UpdateGifAnimation();

            if (loadingOperation.progress >= 0.9f && elapsedTime >= minimumLoadTime)
            {
                loadingOperation.allowSceneActivation = true;
            }

            yield return null;
        }

        // Stop loading music when the scene is fully loaded
        AudioManager.Instance.StopMusic();

        // Hide loading screen
        loadingScreen.SetActive(false);
    }

    private void UpdateGifAnimation()
    {
        frameTimer += Time.deltaTime;
        if (frameTimer >= 1f / frameRate)
        {
            frameTimer = 0f;
            currentFrame = (currentFrame + 1) % gifFrames.Length;
            loadingGif.sprite = gifFrames[currentFrame];
        }
    }
}