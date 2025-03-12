using UnityEngine;

public class BossBeamShooter : MonoBehaviour
{
    public GameObject beamPrefab;
    public float beamSpeed = 10f;
    public float shootInterval = 2f;
    public float beamLifetime = 3f;

    private float nextShootTime;

    private void Start()
    {
        nextShootTime = Time.time + shootInterval;
    }

    private void Update()
    {
        if (Time.time >= nextShootTime)
        {
            ShootBeams();
            nextShootTime = Time.time + shootInterval;
        }
    }

    private void ShootBeams()
    {
        Vector2[] directions = {
            Vector2.up, Vector2.down, Vector2.left, Vector2.right,
            new Vector2(1, 1).normalized, new Vector2(-1, 1).normalized,
            new Vector2(1, -1).normalized, new Vector2(-1, -1).normalized
        };

        foreach (Vector2 direction in directions)
        {
            GameObject beam = Instantiate(beamPrefab, transform.position, Quaternion.identity);
            Rigidbody2D beamRb = beam.GetComponent<Rigidbody2D>();

            if (beamRb != null)
            {
                beamRb.linearVelocity = direction * beamSpeed;
            }

            Destroy(beam, beamLifetime);
        }
    }
}