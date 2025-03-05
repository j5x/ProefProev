using UnityEngine;

namespace Enemy
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 10;
        private int currentHealth;

        void Start() => currentHealth = maxHealth;

        public void TakeDamage(int damage)
        {
            if (currentHealth <= 0) return; // Prevent extra calls after death

            currentHealth -= damage;
            Debug.Log($"{gameObject.name} took {damage} damage! HP left: {currentHealth}");

            if (currentHealth <= 0)
            {
                Debug.Log($"{gameObject.name} destroyed!");
                Destroy(gameObject);
            }
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (!collision.CompareTag("Bullet")) return;

            Debug.Log($"{gameObject.name} hit by {collision.gameObject.name}");

            TakeDamage(1);
            Destroy(collision.gameObject);
        }
    }
}