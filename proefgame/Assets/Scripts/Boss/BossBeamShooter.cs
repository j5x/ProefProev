using UnityEngine;

public class BossBeamShooter : MonoBehaviour
{
    public GameObject beamPrefab; // Prefab for the beam
    public float beamSpeed = 10f; // Speed of the beams
    public float shootInterval = 2f; // Time between each set of beams
    public float beamLifetime = 3f; // How long the beams last before being destroyed

    private float nextShootTime; // When the next set of beams will be fired

    private void Start()
    {
        nextShootTime = Time.time + shootInterval; // Set the initial shoot time
    }

    private void Update()
    {
        // Check if it's time to shoot beams
        if (Time.time >= nextShootTime)
        {
            ShootBeams();
            nextShootTime = Time.time + shootInterval; // Set the next shoot time
        }
    }

    private void ShootBeams()
    {
        // Define the 8 directions (up, down, left, right, and the four diagonals)
        Vector2[] directions = new Vector2[]
        {
            Vector2.up,
            Vector2.down,
            Vector2.left,
            Vector2.right,
            new Vector2(1, 1).normalized,   // Up-right
            new Vector2(-1, 1).normalized,  // Up-left
            new Vector2(1, -1).normalized,  // Down-right
            new Vector2(-1, -1).normalized  // Down-left
        };

        // Shoot a beam in each direction
        foreach (Vector2 direction in directions)
        {
            GameObject beam = Instantiate(beamPrefab, transform.position, Quaternion.identity);
            Rigidbody2D beamRb = beam.GetComponent<Rigidbody2D>();

            if (beamRb != null)
            {
                beamRb.linearVelocity = direction * beamSpeed; // Set the beam's velocity
            }

            Destroy(beam, beamLifetime); // Destroy the beam after its lifetime
        }

        Debug.Log("Beams fired in 8 directions!");
    }
}