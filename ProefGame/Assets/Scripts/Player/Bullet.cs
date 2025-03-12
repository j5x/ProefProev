using Enemy;
using UnityEngine;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10f;
        public int damage = 1;
        public float lifetime = 3f; // Bullet disappears after 3 seconds
        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = transform.right * speed;

            // Destroy bullet after 'lifetime' seconds if it doesn't hit anything
            Destroy(gameObject, lifetime);
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            // Ignore collision with player
            if (collision.CompareTag("Player")) return;

            // Check if it hit an enemy
            if (collision.CompareTag("Enemy"))
            {
                Debug.Log($"{gameObject.name} hit {collision.gameObject.name}");

                var enemyHealth = collision.GetComponent<HealthSystem>();
                if (enemyHealth != null)
                {
                    enemyHealth.TakeDamage(damage);
                }

                Destroy(gameObject); // Destroy bullet instantly on impact
            }
        }
    }
}