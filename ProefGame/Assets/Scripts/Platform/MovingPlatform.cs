using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed;
    public int startingPoint;
    public Transform[] points;

    private int i;
    private Vector2 previousPosition;
    public Vector2 PlatformVelocity { get; private set; }

    private Transform playerTransform; // Reference to the player's transform

    private void Start()
    {
        transform.position = points[startingPoint].position;
        previousPosition = transform.position;
    }

    private void Update()
    {
        // Move the platform
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0;
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

        // Calculate platform velocity
        PlatformVelocity = (Vector2)transform.position - previousPosition;
        previousPosition = transform.position;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Parent the player to the platform
            playerTransform = collision.transform;
            playerTransform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Unparent the player from the platform
            if (playerTransform != null)
            {
                playerTransform.SetParent(null);
                playerTransform = null;
            }
        }
    }
}