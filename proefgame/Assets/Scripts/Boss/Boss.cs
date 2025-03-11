using UnityEngine;

public class Boss : MonoBehaviour
{
    public int maxHealth = 100; // Max health of the boss
    private int currentHealth; // Current health of the boss

    public HealthDummy[] dummies; // Array of dummies
    private int dummiesAlive; // Number of dummies still alive

    private void Start()
    {
        currentHealth = maxHealth; // Initialize boss health
        dummiesAlive = dummies.Length; // Initialize number of dummies
        Debug.Log($"Boss started with {dummiesAlive} dummies and {currentHealth} HP.");

        // Link each dummy to this boss
        foreach (HealthDummy dummy in dummies)
        {
            dummy.SetBoss(this);
        }
    }

    // Called when a dummy dies
    public void DummyDied()
    {
        dummiesAlive--; // Reduce the number of dummies alive
        Debug.Log($"Dummy died! Dummies left: {dummiesAlive}");

        // Calculate the percentage of health to lose
        float healthLossPercentage = 1f / dummies.Length; // 20% for 5 dummies
        int healthLoss = Mathf.RoundToInt(maxHealth * healthLossPercentage);

        // Reduce the boss's health
        currentHealth -= healthLoss;
        Debug.Log($"Boss lost {healthLoss} HP! Current HP: {currentHealth}");

        // Check if all dummies are dead
        if (dummiesAlive <= 0)
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