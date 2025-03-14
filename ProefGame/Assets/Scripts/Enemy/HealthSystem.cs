using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Enemy
{
    public class HealthSystem : MonoBehaviour
    {
        [SerializeField] private int maxHealth = 10;
        private int currentHealth;
        public bool IsInvulnerable { get; private set; } = false;

        private Vector3 startingPosition; // Store player spawn position

        [Header("UI Elements")]
        [SerializeField] private Slider healthBar;
        [SerializeField] private TMP_Text healthText;
        [SerializeField] private Button takeDamageButton;

        public delegate void OnDeathHandler();
        public event OnDeathHandler OnDeath; // Event triggered when health reaches zero

        void Start()
        {
            startingPosition = transform.position; // Store initial spawn point
            currentHealth = maxHealth;
            UpdateHealthUI();

            if (takeDamageButton != null)
                takeDamageButton.onClick.AddListener(() => TakeDamage(1));
        }

        public void TakeDamage(int damage)
        {
            if (IsInvulnerable || currentHealth <= 0) return;

            currentHealth -= damage;
            Debug.Log($"{gameObject.name} took {damage} damage! HP left: {currentHealth}");

            HitFlash hitFlash = GetComponent<HitFlash>();
            if (hitFlash != null) hitFlash.Flash();

            UpdateHealthUI();

            if (currentHealth <= 0)
            {
                Debug.Log($"{gameObject.name} is out of HP!");
                OnDeath?.Invoke(); // Trigger game-over event
                HandleDeath();
            }
        }

        private void HandleDeath()
        {
            gameObject.SetActive(false); // Instead of Destroying the player
        }

        public void ResetHealth()
        {
            currentHealth = maxHealth;
            UpdateHealthUI();
            gameObject.SetActive(true); // Ensure player is reactivated
        }


        public void SetInvulnerable(bool invulnerable)
        {
            IsInvulnerable = invulnerable;
        }

        private void UpdateHealthUI()
        {
            if (healthBar != null)
                healthBar.value = (float)currentHealth / maxHealth;

            if (healthText != null)
                healthText.text = $"{currentHealth} / {maxHealth}";
        }
    }
}
