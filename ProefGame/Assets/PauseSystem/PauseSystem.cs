using Player;
using UnityEngine;
using UnityEngine.InputSystem;

namespace PauseSystem
{
    public class PauseSystem : MonoBehaviour
    {
        public static PauseSystem Instance;

        public GameObject pauseMenu;
        private bool isPaused = false;

        [SerializeField] private InputActionReference pauseActionReference;
        public Shoot playerShooting; // Reference to PlayerShooting script

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
                return;
            }

            // Ensure the pause action reference is set
            if (pauseActionReference != null)
            {
                pauseActionReference.action.Enable();
                pauseActionReference.action.performed += OnPausePerformed;
            }
            else
            {
                Debug.LogError("Pause action reference is not set! Please assign it in the inspector.");
            }

            // Find the PlayerShooting script (Ensure your player is tagged as "Player")
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                playerShooting = player.GetComponent<Shoot>();
            }
        }

        private void OnDestroy()
        {
            if (pauseActionReference != null)
            {
                pauseActionReference.action.performed -= OnPausePerformed;
                pauseActionReference.action.Disable();
            }
        }

        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            TogglePause();
        }

        public void TogglePause()
        {
            isPaused = !isPaused;
            if (isPaused) PauseGame();
            else ResumeGame();
        }

        private void PauseGame()
        {
            Time.timeScale = 0f;
            BackgroundMusicManager.Instance?.SetPaused(true);
            if (pauseMenu) pauseMenu.SetActive(true);

            // Disable Player Shooting
            if (playerShooting != null)
                playerShooting.enabled = false;

            Debug.Log("Game Paused");
        }

        public void ResumeGame()
        {
            Time.timeScale = 1f;
            BackgroundMusicManager.Instance?.SetPaused(false);
            if (pauseMenu) pauseMenu.SetActive(false);

            // Enable Player Shooting
            if (playerShooting != null)
                playerShooting.enabled = true;

            Debug.Log("Game Resumed");
        }

        public void OnResumeButtonClicked() => ResumeGame();

        public void OnQuitButtonClicked() => Application.Quit();
    }
}
