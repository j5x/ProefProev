using UnityEngine;

namespace CoinSystem
{
    public class CoinManager : MonoBehaviour
    {
        public static CoinManager Instance;

        private int coins = 0;

        public delegate void OnCoinsUpdated(int newCoinAmount);
        public static event OnCoinsUpdated onCoinsUpdated;

        private const string CoinKey = "PlayerCoins";

        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
                LoadCoins();
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
            onCoinsUpdated?.Invoke(coins);
            SaveCoins();
        }

        public bool SpendCoins(int amount)
        {
            if (coins >= amount)
            {
                coins -= amount;
                onCoinsUpdated?.Invoke(coins);
                SaveCoins();
                return true;
            }
            return false;
        }

        private void SaveCoins()
        {
            PlayerPrefs.SetInt(CoinKey, coins);
            PlayerPrefs.Save();
        }

        private void LoadCoins()
        {
            coins = PlayerPrefs.GetInt(CoinKey, 0);
            onCoinsUpdated?.Invoke(coins);
        }

        public void ResetCoins()
        {
            coins = 0;
            onCoinsUpdated?.Invoke(coins);
            SaveCoins();
        }
    }
}