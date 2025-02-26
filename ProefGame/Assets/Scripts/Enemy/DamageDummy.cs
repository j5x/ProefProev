using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Enemy
{
    public class DamageDummy : MonoBehaviour
    {
        public int maxHealth = 10;
        private int currentHealth;

        void Start()
        {
            currentHealth = maxHealth;
            Debug.Log("Dummy spawned with " + currentHealth + " HP.");
        }

        void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Bullet")) // Make sure bullets have this tag
            {
                TakeDamage(1);
                Destroy(collision.gameObject); // Destroy the bullet on impact
            }
        }

        void TakeDamage(int damage)
        {
            currentHealth -= damage;
            Debug.Log("Dummy took " + damage + " damage! HP left: " + currentHealth);

            if (currentHealth <= 0)
            {
                Debug.Log("Dummy destroyed!");
                Destroy(gameObject);
            }
        }
    }
}