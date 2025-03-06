using UnityEngine;
using UnityEngine.UI;

public class CoinUI : MonoBehaviour
{
    public Text coinText;

    private void Start()
    {
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
        coinText.text = "Coins: " + newCoinAmount.ToString();
    }
}