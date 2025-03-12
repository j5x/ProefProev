using TMPro;
using UnityEngine;
using UnityEngine.UI;

// If using TextMeshPro

    namespace Enemy
    {
        public class HealthSystem : MonoBehaviour
        {
            [SerializeField] private int maxHealth = 10;
            private int currentHealth;
            public bool IsInvulnerable { get; private set; } = false;

            [Header("UI Elements")]
            [SerializeField] private Slider healthBar;
            [SerializeField] private TMP_Text healthText;
            [SerializeField] private Button takeDamageButton;

            public delegate void OnDeathHandler();
            public event OnDeathHandler OnDeath; // Event triggered when health reaches zero

            void Start()
            {
                currentHealth = maxHealth;
                UpdateHealthUI();

                if (takeDamageButton != null)
                    takeDamageButton.onClick.AddListener(() => TakeDamage(1));
            }

            public void TakeDamage(int damage)
            {
                if (IsInvulnerable) return;
                if (currentHealth <= 0) return;

                currentHealth -= damage;
                Debug.Log($"{gameObject.name} took {damage} damage! HP left: {currentHealth}");

                // Call the flash effect if the component exists
                HitFlash hitFlash = GetComponent<HitFlash>(); // Use HitFlashShader if using a shader
                if (hitFlash != null)
                {
                    hitFlash.Flash();
                }

                UpdateHealthUI();

                if (currentHealth <= 0)
                {
                    Debug.Log($"{gameObject.name} destroyed!");
                    OnDeath?.Invoke(); // Trigger game-over event
                    Destroy(gameObject);
                }
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