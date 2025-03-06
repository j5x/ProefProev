using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("UI References")]
    public GameObject menuPanel; // Main menu UI panel

    [Header("Level to Load")]
    public string levelToLoad = "Level1"; // Set this in the Inspector

    private void Start()
    {
        // Play menu music when the scene starts
        AudioManager.Instance.PlayMusic("menu");
    }

    public void OnPlayButtonClicked()
    {
        // Hide the main menu UI
        menuPanel.SetActive(false);

        // Start loading the game scene
        LoadingScreen.Instance.LoadScene(levelToLoad);
    }

    public void OnQuitButtonClicked()
    {
        Application.Quit();
    }
}