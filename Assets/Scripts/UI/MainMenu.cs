using UnityEngine;
using UnityEngine.SceneManagement;

// Handles the main menu functionality
public class MainMenu : MonoBehaviour
{
    public void StartGame()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GamePlayScene"); // Load the "Game" scene
    }

    public void QuitGame()
    {
        Application.Quit(); // Quit the application
    }
}
