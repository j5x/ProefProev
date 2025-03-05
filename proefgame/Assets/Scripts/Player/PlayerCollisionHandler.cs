using UnityEngine;

public class PlayerCollisionHandler : MonoBehaviour
{
    private DialogManager dialogManager; // Reference to the DialogManager

    void Start()
    {
        // Find the DialogManager in the scene
        dialogManager = FindObjectOfType<DialogManager>();

        if (dialogManager == null)
        {
            Debug.LogError("DialogManager not found in the scene!");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // Check the tag of the object the player collided with
        if (other.CompareTag("Spawn"))
        {
            dialogManager.StartDialog(dialogManager.spawnDialog);
        }
        if (other.CompareTag("Tutorial"))
        {
            dialogManager.StartDialog(dialogManager.tuturialDialog);
        }
        if (other.CompareTag("BossRoom"))
        {
            dialogManager.StartDialog(dialogManager.bossRoomDialog);
        }
        // Add more conditions for other tags as needed
    }
}