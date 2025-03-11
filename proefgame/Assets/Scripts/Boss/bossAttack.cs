using UnityEngine;

public class BossAttack : MonoBehaviour
{
    public GameObject bulletPrefab; // Assign your bullet prefab in the Inspector
    public float bulletSpeed = 5f;
    public float attackCooldown = 3f;
    
    private float nextAttackTime = 0f;
    private Vector2[] bulletDirections = {
        Vector2.down,         // South (↓)
        new Vector2(-1, -1),  // South-West (↙)
        new Vector2(1, -1)    // South-East (↘)
    };

    void Update()
    {
        if (Time.time >= nextAttackTime)
        {
            ShootBullets();
            nextAttackTime = Time.time + attackCooldown;
        }
    }

    void ShootBullets()
    {
        foreach (Vector2 direction in bulletDirections)
        {
            GameObject bullet = Instantiate(bulletPrefab, transform.position, Quaternion.identity);
            Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
            rb.linearVelocity = direction.normalized * bulletSpeed;
        }
    }
}