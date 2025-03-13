using UnityEngine;
using UnityEngine.SceneManagement;

namespace PauseSystem
{
    public class DeathZone : MonoBehaviour
    {
        private Vector3 spawnPoint; // Store the player's spawn position

        private void Start()
        {
            GameObject player = GameObject.FindGameObjectWithTag("Player");
            if (player != null)
            {
                spawnPoint = player.transform.position; // Set the initial spawn point
            }
            else
            {
                Debug.LogError("Player not found! Make sure the Player has the 'Player' tag.");
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player"))
            {
                RespawnPlayer(other.gameObject);
            }
        }

        private void RespawnPlayer(GameObject player)
        {
            player.transform.position = spawnPoint; // Reset player to spawn position
            ResetGameState();
        }

        private void ResetGameState()
        {
            Time.timeScale = 1f; // Ensure the game is unpaused
            if (PauseSystem.Instance != null)
            {
                PauseSystem.Instance.ResumeGame(); // Ensure PauseSystem is correctly resumed
            }
            Debug.Log("Player respawned!");
        }
    }
}