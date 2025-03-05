using UnityEngine;
using UnityEngine.UI;

public class DialogManager : MonoBehaviour
{
    public Text dialogText; // Reference to the UI Text element
    public GameObject dialogPanel; // Reference to the UI Panel

    // Store dialog lines for different tags
    public string[] spawnDialog; // Dialog for "Spawn" tag
    public string[] bossRoomDialog; // Dialog for "BossRoom" tag
    public string[] tuturialDialog;

    private string[] currentDialog; // Current dialog lines being displayed
    private int currentLine = 0; // Tracks the current line of dialog
    private bool isDialogActive = false; // Tracks if dialog is currently active
    private GameObject player; // Reference to the Player GameObject

    void Start()
    {
        // Find the player GameObject using its tag
        player = GameObject.FindWithTag("Player");

        if (player == null)
        {
            Debug.LogError("Player not found! Make sure the player has the 'Player' tag.");
        }
        else
        {
            Debug.Log("Player found: " + player.name);
        }
    }

    void Update()
    {
        // Check if the E key is pressed and dialog is active
        if (isDialogActive && Input.GetKeyDown(KeyCode.E))
        {
            ShowNextDialog();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Check if the colliding object is the player
        if (other.gameObject == player)
        {
            // Check the tag of the object the player collided with
            if (gameObject.CompareTag("Spawn"))
            {
                StartDialog(spawnDialog);
            }
            else if (gameObject.CompareTag("Tutorial"))
            {
                StartDialog(tuturialDialog);
            }
            else if (gameObject.CompareTag("BossRoom"))
            {
                StartDialog(bossRoomDialog);
            }
            // Add more conditions for other tags as needed
        }
    }

    // Call this method to start the dialog with the specified lines
    public void StartDialog(string[] dialogLines)
    {
        currentDialog = dialogLines; // Set the current dialog lines
        currentLine = 0; // Reset to the first line
        isDialogActive = true; // Activate the dialog
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
        // Deactivate the dialog panel
        dialogPanel.SetActive(false);
        isDialogActive = false; // Deactivate the dialog
        Debug.Log("Dialog closed.");
    }
}