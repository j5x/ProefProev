using TMPro;
using UnityEngine;
using UnityEngine.UI;

// Add this namespace for TextMeshPro

namespace CoinSystem
{
    public class ShopItem : MonoBehaviour
    {
        public int price = 10; // Price of the item
        public Button buyButton;
        public TextMeshProUGUI priceText; // Use TextMeshProUGUI instead of Text

        private void Start()
        {
            // Update the price text
            if (priceText != null)
            {
                priceText.text = "Price: " + price + " Coins";
            }

            buyButton.onClick.AddListener(BuyItem);
        }

        private void BuyItem()
        {
            if (CoinManager.Instance.SpendCoins(price))
            {
                Debug.Log("Item purchased!");
                // Add logic to give the player the item (e.g., increase health)
            }
            else
            {
                Debug.Log("Not enough coins!");
            }
        }
    }
}