using UnityEngine;

public class Boss : MonoBehaviour
{
    public int maxHealth = 100; // Max health of the boss
    private int currentHealth; // Current health of the boss

    public int numberOfDummies = 5; // Total number of dummies
    private int dummiesAlive; // Number of dummies still alive

    private void Start()
    {
        currentHealth = maxHealth; // Initialize boss health
        dummiesAlive = numberOfDummies; // Initialize number of dummies
        Debug.Log($"Boss started with {dummiesAlive} dummies and {currentHealth} HP.");
    }

    // Called when a dummy dies
    public void DummyDied()
    {
        dummiesAlive--; // Reduce the number of dummies alive
        Debug.Log($"Dummy died! Dummies left: {dummiesAlive}");

        // Calculate the percentage of health to lose
        float healthLossPercentage = 1f / numberOfDummies; // 20% for 5 dummies
        int healthLoss = Mathf.RoundToInt(maxHealth * healthLossPercentage);

        // Reduce the boss's health
        currentHealth -= healthLoss;
        Debug.Log($"Boss lost {healthLoss} HP! Current HP: {currentHealth}");

        // Check if the boss is dead
        if (currentHealth <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss died!");
        // Add any boss death logic here (e.g., play animation, end game, etc.)
        Destroy(gameObject); // Destroy the boss
    }
}