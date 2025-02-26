using UnityEngine;

public class CoinManager : MonoBehaviour
{
    public static CoinManager Instance;

    private int coins = 0;

    // Event to notify UI when coins are updated
    public delegate void OnCoinsUpdated(int newCoinAmount);
    public static event OnCoinsUpdated onCoinsUpdated;

    private const string CoinKey = "PlayerCoins"; // Key for saving/loading coins

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Persist across scenes
            LoadCoins(); // Load saved coins
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public int GetCoins()
    {
        return coins;
    }

    public void AddCoins(int amount)
    {
        coins += amount;
        onCoinsUpdated?.Invoke(coins); // Notify UI
        SaveCoins(); // Save coins
    }

    public bool SpendCoins(int amount)
    {
        if (coins >= amount)
        {
            coins -= amount;
            onCoinsUpdated?.Invoke(coins); // Notify UI
            SaveCoins(); // Save coins
            return true; // Successfully spent coins
        }
        return false; // Not enough coins
    }

    private void SaveCoins()
    {
        PlayerPrefs.SetInt(CoinKey, coins);
        PlayerPrefs.Save();
    }

    private void LoadCoins()
    {
        coins = PlayerPrefs.GetInt(CoinKey, 0);
        onCoinsUpdated?.Invoke(coins); // Update UI with loaded coins
    }

    public void ResetCoins()
    {
        coins = 0;
        onCoinsUpdated?.Invoke(coins); // Notify UI
        SaveCoins(); // Save reset coins
    }
}