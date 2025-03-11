using Enemy;
using UnityEngine;

public class EnemyAI : MonoBehaviour
{
    [SerializeField] private float moveSpeed; // Speed at which the enemy moves
    [SerializeField] private float moveRange; // Range within which the enemy moves left and right
    public int damage = 1; // Damage dealt to the player on contact

    private Vector2 startPosition;
    private bool movingRight = true;
    private SpriteRenderer enemySprite;
    private Rigidbody2D rb;
    private Animator anim; // Animator component

    [SerializeField] private float shootingRange = 5f; // Range within which the enemy can shoot
    private Transform player; // Reference to the player's transform
    private bool isPlayerInRange = false; // Whether the player is in shooting range

    [SerializeField] private float idleDuration = 2f; // Time to idle after player leaves range
    private float idleTimer = 0f; // Timer for idling

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        startPosition = rb.position;

        // Get the Animator component attached to the same GameObject
        anim = GetComponent<Animator>();
        if (anim == null)
        {
            Debug.LogError("Animator component not found on the enemy GameObject!");
        }

        // Get the SpriteRenderer component
        enemySprite = GetComponent<SpriteRenderer>();
        if (enemySprite == null)
        {
            Debug.LogError("SpriteRenderer component not found on the enemy GameObject!");
        }

        // Find the player GameObject
        player = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (player == null)
        {
            Debug.LogError("Player not found! Ensure the player GameObject is tagged with 'Player'.");
        }

        // Start moving by default
        anim.SetBool("isMoving", true);
    }

    void Update()
    {
        // Check if the player is in range (only if player is not null)
        if (player != null)
        {
            float distanceToPlayer = Vector2.Distance(transform.position, player.position);
            isPlayerInRange = distanceToPlayer <= shootingRange;

            // Update the inRange parameter in the Animator
            anim.SetBool("inRange", isPlayerInRange);

            if (isPlayerInRange)
            {
                // Player is in range: stop moving and shoot
                StopMoving();
                anim.SetTrigger("Shoot"); // Trigger shooting animation
            }
            else
            {
                // Player is out of range: handle idle and moving behavior
                HandleIdleAndMoving();
            }
        }
    }

    void StopMoving()
    {
        // Stop moving and face the player
        anim.SetBool("isMoving", false);

        // Face the player
        if (player.position.x > transform.position.x)
        {
            enemySprite.flipX = false; // Face right
        }
        else
        {
            enemySprite.flipX = true; // Face left
        }
    }

    void HandleIdleAndMoving()
    {
        if (idleTimer > 0f)
        {
            // Idle for a set duration
            idleTimer -= Time.deltaTime;
            anim.SetBool("isMoving", false); // Ensure the enemy is idle
        }
        else
        {
            // Resume moving after idle duration
            anim.SetBool("isMoving", true);
            MoveEnemy();
        }
    }

    void MoveEnemy()
    {
        // Calculate the target position
        Vector2 targetPosition = movingRight ? startPosition + Vector2.right * moveRange : startPosition - Vector2.right * moveRange;

        // Move towards the target position
        rb.position = Vector2.MoveTowards(rb.position, targetPosition, moveSpeed * Time.deltaTime);

        // Flip the sprite based on movement direction
        if (movingRight)
        {
            enemySprite.flipX = false; // Facing right
        }
        else
        {
            enemySprite.flipX = true; // Facing left
        }

        // If the enemy reaches the target position, change direction
        if (Vector2.Distance(rb.position, targetPosition) < 0.1f)
        {
            movingRight = !movingRight;

            // Add some randomness to the movement
            moveRange = Random.Range(3f, 7f); // Randomize the range
        }
    }

    // Handle collision with the player
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        if (collision.gameObject.CompareTag("Player"))
        {
            Debug.Log("Player hit!");
            HealthSystem playerHealth = collision.gameObject.GetComponent<HealthSystem>();
            if (playerHealth != null)
            {
                Debug.Log("Player health found. Dealing damage.");
                playerHealth.TakeDamage(damage);
            }
            else
            {
                Debug.LogError("Player HealthSystem component not found!");
            }
        }
    }

    // Called when the player leaves the range
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            // Start idling for the set duration
            idleTimer = idleDuration;
        }
    }
}