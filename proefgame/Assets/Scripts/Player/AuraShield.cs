using System.Collections;
using Platformer.Mechanics;
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

    private InputAction m_AuraShieldAction; // Input action for the shield
    private Health playerHealth; // Reference to the player's Health component

    void Awake()
    {
        // Get the Health component from the player
        playerHealth = GetComponent<Health>();

        // Find and enable the AuraShield input action
        m_AuraShieldAction = InputSystem.actions.FindAction("Player/Aurashield");
        m_AuraShieldAction.Enable();
    }

    void Update()
    {
        // Check if the shield input is pressed, the shield is not active, and it's not on cooldown
        if (m_AuraShieldAction.WasPressedThisFrame() && !isShieldActive && !isOnCooldown)
        {
            ActivateShield();
        }
    }

    void ActivateShield()
    {
        // Instantiate the shield prefab at the player's position
        GameObject shieldInstance = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
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

        // Destroy the shield instance
        if (transform.childCount > 0)
        {
            Destroy(transform.GetChild(0).gameObject);
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