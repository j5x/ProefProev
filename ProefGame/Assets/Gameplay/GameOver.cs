using Enemy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI; // Game Over UI
    [SerializeField] private HealthSystem playerHealth; // Player Health System
    [SerializeField] private AudioSource audioSource; // Audio Source for music
    [SerializeField] private AudioClip sadMusic; // Sad music clip
    [SerializeField] private Button restartButton; // Restart button
    [SerializeField] private Button quitButton; // Quit button

    private bool isGameOver = false; // Prevent multiple triggers

    private void Start()
    {
        // Subscribe to player's death event
        if (playerHealth != null)
        {
            playerHealth.OnDeath += HandleGameOver;
        }

        // Ensure Game Over UI is hidden initially
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        // Assign button listeners
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
        if (isGameOver) return; // Prevent multiple triggers

        isGameOver = true;
        Debug.Log("Game Over!");

        // Show Game Over UI
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true);
        }

        // Pause the game
        Time.timeScale = 0f;

        // Play sad music
        if (audioSource != null && sadMusic != null)
        {
            audioSource.clip = sadMusic;
            audioSource.loop = false;
            audioSource.Play();
        }
    }

    public void RestartGame()
    {
        // Unsubscribe from the event to prevent duplicate UI flashes
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= HandleGameOver;
        }

        // Reset state before reloading
        isGameOver = false;

        // Hide UI before restarting
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        // Reset time scale
        Time.timeScale = 1f;

        // Reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
