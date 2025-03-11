using Enemy;
using UnityEngine;

namespace Player
{
    public class PlayerCollisionHandler : MonoBehaviour
    {
        private DialogManager dialogManager; // Reference to the DialogManager
        private HealthSystem healthSystem;  // Reference to the player's HealthSystem

        void Start()
        {
            // Find the DialogManager in the scene
            dialogManager = FindObjectOfType<DialogManager>();

            if (dialogManager == null)
            {
                Debug.LogError("DialogManager not found in the scene!");
            }

            // Get the HealthSystem component attached to the player
            healthSystem = GetComponent<HealthSystem>();

            if (healthSystem == null)
            {
                Debug.LogError("HealthSystem not found on player!");
            }
        }

        void OnTriggerEnter2D(Collider2D other)
        {
            // Check the tag of the object the player collided with
            if (other.CompareTag("Spawn"))
            {
                dialogManager.StartDialog(dialogManager.spawnDialog, dialogManager.spawnIcon);
            }
            else if (other.CompareTag("BossRoom"))
            {
                dialogManager.StartDialog(dialogManager.bossRoomDialog, dialogManager.bossRoomIcon);
            }
            else if (other.CompareTag("Tutorial"))
            {
                dialogManager.StartDialog(dialogManager.tutorialDialog, dialogManager.tutorialIcon);
            }
            else if (other.CompareTag("Enemy")) // Check if the player collided with an enemy
            {
                // Call TakeDamage method from the HealthSystem when colliding with an enemy
                healthSystem.TakeDamage(1); // Adjust damage value as needed
            }
            // Add more conditions for other tags as needed
        }
    }
}