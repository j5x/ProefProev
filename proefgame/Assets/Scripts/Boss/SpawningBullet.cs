using UnityEngine;

public class SpawningBullet : MonoBehaviour
{
    public GameObject enemyPrefab; // Assign your enemy prefab in the Inspector
    public float destroyDelay = 0.1f; // Delay before destroying the bullet
    public Vector2 spawnOffset = new Vector2(0, 0.5f); // Adjust spawn position slightly above platform

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Platform"))
        {
            Vector2 spawnPosition = (Vector2)transform.position + spawnOffset;
            Instantiate(enemyPrefab, spawnPosition, Quaternion.identity); // Spawn enemy slightly above platform

            Destroy(gameObject, destroyDelay); // Destroy the bullet after a short delay
        }
    }
}