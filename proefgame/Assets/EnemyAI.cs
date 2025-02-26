using Platformer.Mechanics;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    public float moveSpeed = 2f; // Speed at which the enemy moves
    public float moveRange = 5f; // Range within which the enemy moves left and right
    public int damage = 1; // Damage dealt to the player on contact

    private Vector2 startPosition;
    private bool movingRight = true;

    private Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;
    }

    void Update()
    {
        MoveEnemy();
    }

    void MoveEnemy()
    {
        // Calculate the target position
        Vector2 targetPosition = movingRight ? startPosition + Vector2.right * moveRange : startPosition - Vector2.right * moveRange;

        // Move towards the target position
        rb.position = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);

        // If the enemy reaches the target position, change direction
        if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            movingRight = !movingRight;

            // Add some randomness to the movement
            moveRange = Random.Range(3f, 7f); // Randomize the range
            moveSpeed = Random.Range(1f, 3f); // Randomize the speed
        }
    }

    // Handle collision with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            Health playerHealth = collision.gameObject.GetComponent<Health>();
            if (playerHealth != null)
            {
                Debug.Log("Player health found. Dealing damage.");
                playerHealth.Decrement();
            }
            else
            {
                Debug.LogError("Player Health component not found!");
            }
        }
    }
}