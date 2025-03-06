using TMPro;
using UnityEngine;

// Add this namespace for TextMeshPro

namespace CoinSystem
{
    public class CoinUI : MonoBehaviour
    {
        public TextMeshProUGUI coinText; // Use TextMeshProUGUI instead of Text

        private void Start()
        {
            // Try to get the TextMeshProUGUI component
            coinText = GetComponent<TextMeshProUGUI>();

            // If the TextMeshProUGUI component is not found, log an error
            if (coinText == null)
            {
                Debug.LogError("CoinText does not have a TextMeshProUGUI component! Make sure the script is attached to a GameObject with a TextMeshProUGUI component.");
                return;
            }

            // Initialize the UI with the current coin amount
            coinText.text = "Coins: " + CoinManager.Instance.GetCoins().ToString();

            // Subscribe to the coin update event
            CoinManager.onCoinsUpdated += UpdateCoinUI;
        }

        private void OnDestroy()
        {
            // Unsubscribe to avoid memory leaks
            CoinManager.onCoinsUpdated -= UpdateCoinUI;
        }

        private void UpdateCoinUI(int newCoinAmount)
        {
            if (coinText != null)
            {
                coinText.text = "Coins: " + newCoinAmount.ToString();
            }
        }
    }
}