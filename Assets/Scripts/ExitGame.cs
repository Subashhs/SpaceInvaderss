using UnityEngine;

public class ExitGame : MonoBehaviour
{
    // This method will be called when the button is clicked
    public void QuitGame()
    {
        // Log to confirm the quit action in the editor
        Debug.Log("Game is exiting...");

        // Quit the application
        Application.Quit();
    }
}
