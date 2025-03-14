using Enemy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI;
    [SerializeField] private HealthSystem playerHealth;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip sadMusic;
    [SerializeField] private Button restartButton;
    [SerializeField] private Button quitButton;

    private bool isGameOver = false;
    private Camera mainCamera;
    private Vector3 playerStartPosition;
    private Quaternion playerStartRotation;

    private void Start()
    {
        mainCamera = Camera.main; // Get the main camera

        if (playerHealth != null)
        {
            playerStartPosition = playerHealth.transform.position;
            playerStartRotation = playerHealth.transform.rotation;
            playerHealth.OnDeath += HandleGameOver;
        }

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame);
        }

        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame);
        }
    }

    private void HandleGameOver()
    {
        if (isGameOver) return;

        isGameOver = true;
        Debug.Log("Game Over!");

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        Time.timeScale = 0f;

        if (audioSource != null && sadMusic != null)
        {
            audioSource.clip = sadMusic;
            audioSource.loop = false;
            audioSource.Play();
        }

        // STOP BACKGROUND MUSIC WHEN PLAYER DIES
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.StopMusic();
        }
    }

    public void RestartGame()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= HandleGameOver; // Unsubscribe before resetting
            ResetPlayer(); // Reset player instead of reloading scene
            playerHealth.OnDeath += HandleGameOver; // Reattach event after reset
        }

        isGameOver = false;

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        Time.timeScale = 1f;

        // STOP SAD MUSIC WHEN RESTARTING
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }

        // RESTART BACKGROUND MUSIC
        if (BackgroundMusicManager.Instance != null)
        {
            BackgroundMusicManager.Instance.PlayMusic();
        }
    }


    private void ResetPlayer()
    {
        playerHealth.ResetHealth();
        playerHealth.transform.position = playerStartPosition;
        playerHealth.transform.rotation = playerStartRotation;
        playerHealth.gameObject.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
