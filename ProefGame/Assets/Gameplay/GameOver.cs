using Enemy;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameOverManager : MonoBehaviour
{
    [SerializeField] private GameObject gameOverUI; // Reference to the Game Over UI
    [SerializeField] private HealthSystem playerHealth;
    [SerializeField] private AudioSource audioSource; // Audio source for sad music
    [SerializeField] private AudioClip sadMusic; // Assign a sad music clip in the Inspector
    [SerializeField] private Button restartButton; // UI Button for restarting the game
    [SerializeField] private Button quitButton; //UI Button for quitting the game
    void Start()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath += HandleGameOver;
        }

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false); // Hide game-over screen at start
        }

        if (restartButton != null)
        {
            restartButton.onClick.AddListener(RestartGame); // Add restart function to the button
        }
        if (quitButton != null)
        {
            quitButton.onClick.AddListener(QuitGame); // Add quit function to the button
        }
    }

    void HandleGameOver()
    {
        Debug.Log("Game Over!");
        if (gameOverUI != null)
        {
            gameOverUI.SetActive(true); // Show game-over UI
        }
        
        Time.timeScale = 0f; // Pause the game

        // Play sad music if assigned
        if (audioSource != null && sadMusic != null)
        {
            audioSource.clip = sadMusic;
            audioSource.loop = false; // Optional: Set to false if you don’t want looping
            audioSource.Play();
        }
    }

    public void RestartGame()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name); // Reload the scene
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}