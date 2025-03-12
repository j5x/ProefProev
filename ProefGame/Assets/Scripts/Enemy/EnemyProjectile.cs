using Player;
using UnityEngine;

namespace Enemy
{
    public class EnemyProjectile : MonoBehaviour
    {
        public float damage = 1f;
        public float knockbackForce = 5f;

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Player"))
            {
                AuraShield shield = collision.GetComponent<AuraShield>();

                // ✅ Fix: Now properly checks if the shield is active
                if (shield != null && shield.IsShieldActive())
                {
                    return;
                }

                // ✅ Fix: Convert float to int for damage
                collision.GetComponent<HealthSystem>()?.TakeDamage((int)damage);

                // Apply knockback
                Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
                if (rb != null)
                {
                    Vector2 forceDirection = (collision.transform.position - transform.position).normalized;
                    rb.AddForce(forceDirection * knockbackForce, ForceMode2D.Impulse);
                }
            }
        }
    }
}