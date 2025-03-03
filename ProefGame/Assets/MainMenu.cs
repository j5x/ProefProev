using UnityEngine;

public class MainMenu : MonoBehaviour
{
    [Header("Level to Load")]
    public string levelToLoad; // Name of the level/scene to load

    // Reference to the LoadingScreen script
    private LoadingScreen loadingScreen;

    private void Start()
    {
        // Find the LoadingScreen component in the scene
        loadingScreen = FindObjectOfType<LoadingScreen>();

        if (loadingScreen == null)
        {
            Debug.LogError("LoadingScreen not found in the scene!");
        }

        // Ensure the loading screen is hidden when the menu is shown
        if (loadingScreen != null && loadingScreen.loadingScreen != null)
        {
            loadingScreen.loadingScreen.SetActive(false);
        }
    }

    // Called when the Play button is clicked
    public void OnPlayButtonClicked()
    {
        Debug.Log("Play button clicked.");

        if (loadingScreen != null && !string.IsNullOrEmpty(levelToLoad))
        {
            loadingScreen.LoadGameScene(levelToLoad);
        }
        else
        {
            Debug.LogError("LoadingScreen reference is null or levelToLoad is not set!");
        }
    }

    // Called when the Quit button is clicked
    public void OnQuitButtonClicked()
    {
        Debug.Log("Quit button clicked.");
        Application.Quit();
    }
}