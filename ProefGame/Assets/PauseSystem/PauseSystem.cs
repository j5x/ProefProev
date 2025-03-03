using UnityEngine;
using UnityEngine.InputSystem;

// Add this namespace for Unity Input System

namespace PauseSystem
{
    public class PauseSystem : MonoBehaviour
    {
        public static PauseSystem Instance;

        public GameObject pauseMenu; // Reference to the Pause Menu UI

        private bool isPaused = false;
        private InputAction m_Pausemenu; // Input action for pause

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject); // Persist across scenes
            }
            else
            {
                Destroy(gameObject); // Destroy duplicate instances
            }

            // Find and enable the Pause action
            m_Pausemenu = InputSystem.actions.FindAction("Player/Pause");
            m_Pausemenu.Enable();
            m_Pausemenu.performed += OnPausePerformed; // Subscribe to the Pause action
        }

        private void OnDestroy()
        {
            // Clean up the Input System action
            if (m_Pausemenu != null)
            {
                m_Pausemenu.performed -= OnPausePerformed; // Unsubscribe from the Pause action
                m_Pausemenu.Disable();
            }
        }

        private void OnPausePerformed(InputAction.CallbackContext context)
        {
            // Toggle pause when the Pause action is performed
            TogglePause();
        }

        public void TogglePause()
        {
            isPaused = !isPaused;

            if (isPaused)
            {
                PauseGame();
            }
            else
            {
                ResumeGame();
            }
        }

        private void PauseGame()
        {
            Time.timeScale = 0f; // Stop time
            BackgroundMusicManager.Instance.SetPaused(true); // Pause the music
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(true); // Show the pause menu
            }
            Debug.Log("Game Paused");
        }

        private void ResumeGame()
        {
            Time.timeScale = 1f; // Resume time
            BackgroundMusicManager.Instance.SetPaused(false); // Resume the music
            if (pauseMenu != null)
            {
                pauseMenu.SetActive(false); // Hide the pause menu
            }
            Debug.Log("Game Resumed");
        }

        public void OnResumeButtonClicked()
        {
            ResumeGame();
        }

        public void OnQuitButtonClicked()
        {
            Application.Quit(); // Quit the game
        }
    }
}