using UnityEngine;

public class Coin : MonoBehaviour
{
    public int coinValue = 1; // Value of this coin

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player")) // Check if the player collected the coin
        {
            CoinManager.Instance.AddCoins(coinValue); // Add coins to the manager
            Destroy(gameObject); // Destroy the coin
        }
    }
}