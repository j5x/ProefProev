using UnityEngine;

namespace CoinSystem
{
    public class Coin : MonoBehaviour
    {
        public int coinValue = 1; // Value of this coin
        public AudioClip pickupSound; // Sound effect to play when the coin is picked up

        private AudioSource audioSource;

        private void Start()
        {
            // Get the AudioSource component
            audioSource = GetComponent<AudioSource>();

            // If no AudioSource is found, add one dynamically
            if (audioSource == null)
            {
                audioSource = gameObject.AddComponent<AudioSource>();
            }
        }

        private void OnTriggerEnter2D(Collider2D other)
        {
            if (other.CompareTag("Player")) // Check if the player collected the coin
            {
                // Play the pickup sound
                if (pickupSound != null && audioSource != null)
                {
                    audioSource.PlayOneShot(pickupSound);
                }

                // Add coins to the manager
                CoinManager.Instance.AddCoins(coinValue);

                // Disable the coin (so it can't be collected again)
                GetComponent<SpriteRenderer>().enabled = false;
                GetComponent<Collider2D>().enabled = false;

                // Destroy the coin after the sound finishes playing
                Destroy(gameObject, pickupSound != null ? pickupSound.length : 0.1f);
            }
        }
    }
}