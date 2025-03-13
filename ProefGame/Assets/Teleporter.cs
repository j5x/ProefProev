using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : MonoBehaviour
{
    [SerializeField] private string levelToLoad; // Name of the scene to load

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Ensure only the player can trigger it
        {
            SceneManager.LoadScene(levelToLoad);
        }
    }
}
