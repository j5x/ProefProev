using System.Collections;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public float speed; // Speed of the platform
    public int startingPoint; // Starting point in the points array
    public Transform[] points; // Array of points the platform moves between

    private int i; // Index of the current target point
    private Vector2 previousPosition; // Position of the platform in the previous frame
    public Vector2 PlatformVelocity { get; private set; } // Velocity of the platform

    private void Start()
    {
        // Set the platform's initial position
        transform.position = points[startingPoint].position;
        previousPosition = transform.position;
    }

    private void Update()
    {
        // Move the platform towards the current target point
        if (Vector2.Distance(transform.position, points[i].position) < 0.02f)
        {
            i++;
            if (i == points.Length)
            {
                i = 0; // Loop back to the first point
            }
        }

        transform.position = Vector2.MoveTowards(transform.position, points[i].position, speed * Time.deltaTime);

        // Calculate the platform's velocity
        PlatformVelocity = (Vector2)transform.position - previousPosition;
        previousPosition = transform.position;
    }
}