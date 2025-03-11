using UnityEngine;

public class HealthDummy : MonoBehaviour
{
    public int maxHealth = 10; // Max health of the dummy
    private int currentHealth; // Current health of the dummy

    [SerializeField] private Boss boss; // Reference to the Boss script (not GameObject)

    private void Start()
    {
        currentHealth = maxHealth; // Initialize health
        boss = FindObjectOfType<Boss>(); // Find the Boss script in the scene
        if (boss == null)
        {
            Debug.LogError("Boss not found in the scene!");
        }
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
}