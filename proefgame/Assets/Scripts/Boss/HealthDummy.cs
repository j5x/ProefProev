using UnityEngine;

public class HealthDummy : MonoBehaviour
{
    public int maxHealth = 10; // Max health of the dummy
    private int currentHealth; // Current health of the dummy

    private Boss boss; // Reference to the boss

    // Call this method to set the boss reference
    public void SetBoss(Boss bossReference)
    {
        boss = bossReference;
        Debug.Log($"{gameObject.name} linked to boss: {boss != null}");
    }

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health
    }

    // Call this method when the dummy takes damage
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        Debug.Log($"{gameObject.name} took {damage} damage! HP left: {currentHealth}");

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log($"{gameObject.name} died!");
        boss.DummyDied(); // Notify the boss that this dummy died
        Destroy(gameObject); // Destroy the dummy
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Bullet")) // Make sure bullets have this tag
        {
            TakeDamage(1);
            Destroy(collision.gameObject); // Destroy the bullet on impact
        }
    }
}