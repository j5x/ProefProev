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

    private void Start()
    {
        mainCamera = Camera.main; // Get the main camera

        if (playerHealth != null)
        {
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
    }

    public void RestartGame()
    {
        if (playerHealth != null)
        {
            playerHealth.OnDeath -= HandleGameOver;
        }

        isGameOver = false;

        if (gameOverUI != null)
        {
            gameOverUI.SetActive(false);
        }

        Time.timeScale = 1f;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);

        Invoke(nameof(ReassignCamera), 0.1f); // Delay to allow scene load
    }

    private void ReassignCamera()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null && mainCamera != null)
        {
            mainCamera.transform.SetParent(player.transform);
            mainCamera.transform.localPosition = new Vector3(0, 0, -10); // Adjust for best view
        }
    }

    public void QuitGame()
    {
        Debug.Log("Quitting Game...");
        Application.Quit();
    }
}
