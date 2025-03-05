using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text dialogText; // Reference to the UI Text element
    public GameObject dialogPanel; // Reference to the UI Panel
    public Image dialogIcon; // Reference to the UI Image element for the icon

    // Store dialog lines and icons for different tags
    public string[] spawnDialog; // Dialog for "Spawn" tag
    public Sprite spawnIcon; // Icon for "Spawn" dialog

    public string[] bossRoomDialog; // Dialog for "BossRoom" tag
    public Sprite bossRoomIcon; // Icon for "BossRoom" dialog

    public string[] tutorialDialog; // Dialog for "Tutorial" tag
    public Sprite tutorialIcon; // Icon for "Tutorial" dialog

    private string[] currentDialog; // Current dialog lines being displayed
    private int currentLine = 0; // Tracks the current line of dialog
    private bool isDialogActive = false; // Tracks if dialog is currently active

    void Update()
    {
        // Check if the E key is pressed and dialog is active
        if (isDialogActive && Input.GetKeyDown(KeyCode.E))
        {
            ShowNextDialog();
        }
    }

    // Call this method to start the dialog with the specified lines and icon
    public void StartDialog(string[] dialogLines, Sprite icon)
    {
        currentDialog = dialogLines; // Set the current dialog lines
        currentLine = 0; // Reset to the first line
        isDialogActive = true; // Activate the dialog

        // Set the icon
        dialogIcon.sprite = icon;
        dialogIcon.gameObject.SetActive(true); // Show the icon

        ShowDialog(); // Show the first dialog line
    }

    void ShowDialog()
    {
        // Activate the dialog panel and show the current line
        dialogPanel.SetActive(true);
        dialogText.text = currentDialog[currentLine];
        Debug.Log("Showing dialog: " + currentDialog[currentLine]);
    }

    void ShowNextDialog()
    {
        // Move to the next line
        currentLine++;

        // Check if there are more lines to show
        if (currentLine < currentDialog.Length)
        {
            ShowDialog();
        }
        else
        {
            // If no more lines, close the dialog
            CloseDialog();
        }
    }

    void CloseDialog()
    {
        // Deactivate the dialog panel and icon
        dialogPanel.SetActive(false);
        dialogIcon.gameObject.SetActive(false); // Hide the icon
        isDialogActive = false; // Deactivate the dialog
        Debug.Log("Dialog closed.");
    }
}