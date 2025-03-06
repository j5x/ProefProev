using Enemy;
using Health;
using UnityEngine;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        public float speed = 10f;
        public int damage = 1;
        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = transform.right * speed;
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Enemy")) return;

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