using System.Collections;
using Health;
using UnityEngine;
using UnityEngine.InputSystem;

public class AuraShield : MonoBehaviour
{
    public GameObject shieldPrefab; // The shield prefab to instantiate
    public float invulnerabilityDuration = 3f; // Duration of invulnerability
    public float shieldCooldown = 5f; // Cooldown before the shield can be used again

    private bool isShieldActive = false; // Is the shield currently active?
    private bool isOnCooldown = false; // Is the shield on cooldown?
    private float cooldownEndTime; // Time when the cooldown ends

    private HealthSystem playerHealth; // Reference to the player's HealthSystem component
    private GameObject shieldInstance; // Reference to the instantiated shield

    private void Awake()
    {
        // Get the HealthSystem component from the player
        playerHealth = GetComponent<HealthSystem>();
        if (playerHealth == null)
        {
            Debug.LogError("HealthSystem component not found on the player!");
        }
    }

    // Called by the Input System when aura shield is activated
    public void OnAuraShield(InputAction.CallbackContext context)
    {
        if (context.performed && !isShieldActive && !isOnCooldown)
        {
            ActivateShield();
        }
    }

    private void ActivateShield()
    {
        // Instantiate the shield prefab at the player's position
        shieldInstance = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        shieldInstance.transform.SetParent(transform); // Make the shield a child of the player

        // Make the player invulnerable
        playerHealth.SetInvulnerable(true);

        // Set shield state to active
        isShieldActive = true;

        // Start the invulnerability timer
        StartCoroutine(DeactivateShieldAfterDuration(invulnerabilityDuration));

        // Start the cooldown timer
        StartCoroutine(StartCooldown(shieldCooldown));
    }

    IEnumerator DeactivateShieldAfterDuration(float duration)
    {
        // Wait for the specified duration
        yield return new WaitForSeconds(duration);

        // Make the player vulnerable again
        playerHealth.SetInvulnerable(false);

        // Destroy the shield instance (if it exists)
        if (shieldInstance != null)
        {
            Destroy(shieldInstance);
        }

        // Set shield state to inactive
        isShieldActive = false;
    }

    IEnumerator StartCooldown(float cooldown)
    {
        // Set cooldown state to active
        isOnCooldown = true;

        // Wait for the cooldown duration
        yield return new WaitForSeconds(cooldown);

        // Set cooldown state to inactive
        isOnCooldown = false;
    }
}