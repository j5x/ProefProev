using UnityEngine;
using Platformer.Mechanics;

namespace Player
{
    public class Bullet : MonoBehaviour
    {
        public float speed;
        public int damage = 1;
        private Rigidbody2D rb;

        void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            rb.linearVelocity = transform.right * speed;
        }
        
        void Awake()
        {
            Destroy(gameObject, 3f); // Bullet disappears after 3 seconds
        }

        
        void OnTriggerEnter2D(Collider2D collision)
        {
            // Check if the bullet hit an enemy with a Health component
            Health enemyHealth = collision.GetComponent<Health>();
            if (enemyHealth != null)
            {
                enemyHealth.Decrement(); // Reduce enemy health
                Destroy(gameObject); // Destroy the bullet
            }
        }
    }
}
