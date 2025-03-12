using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using Enemy;

public class AuraShield : MonoBehaviour
{
    public GameObject shieldPrefab;
    public float invulnerabilityDuration = 3f;
    public float shieldCooldown = 5f;

    private bool isShieldActive = false;
    private bool isOnCooldown = false;
    private GameObject shieldInstance;
    private Collider2D playerCollider;

    private void Awake()
    {
        playerCollider = GetComponent<Collider2D>();
        if (playerCollider == null)
        {
            Debug.LogError("No Collider2D found on player!");
        }
    }

    public void OnAuraShield(InputAction.CallbackContext context)
    {
        if (context.performed && !isShieldActive && !isOnCooldown)
        {
            ActivateShield();
        }
    }

    private void ActivateShield()
    {
        shieldInstance = Instantiate(shieldPrefab, transform.position, Quaternion.identity);
        shieldInstance.transform.SetParent(transform);
        
        isShieldActive = true;
        IgnoreEnemyProjectiles(true);

        StartCoroutine(DeactivateShieldAfterDuration(invulnerabilityDuration));
        StartCoroutine(StartCooldown(shieldCooldown));
    }

    IEnumerator DeactivateShieldAfterDuration(float duration)
    {
        yield return new WaitForSeconds(duration);
        
        isShieldActive = false;
        IgnoreEnemyProjectiles(false);

        if (shieldInstance != null)
        {
            Destroy(shieldInstance);
        }
    }

    IEnumerator StartCooldown(float cooldown)
    {
        isOnCooldown = true;
        yield return new WaitForSeconds(cooldown);
        isOnCooldown = false;
    }

    private void IgnoreEnemyProjectiles(bool ignore)
    {
        EnemyProjectile[] enemyProjectiles = FindObjectsOfType<EnemyProjectile>();
        foreach (var projectile in enemyProjectiles)
        {
            Collider2D projectileCollider = projectile.GetComponent<Collider2D>();
            if (projectileCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, projectileCollider, ignore);
            }
        }
    }

    //Fix: Make shield status publicly accessible
    public bool IsShieldActive()
    {
        return isShieldActive;
    }
}
