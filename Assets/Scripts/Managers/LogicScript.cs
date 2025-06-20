using UnityEngine; 
using UnityEngine.UI; 
using UnityEngine.SceneManagement; 
using UnityEngine.Audio;     
using System.Collections;     

// Handles the game's logic including scoring and game state
public class LogicScript : MonoBehaviour
{
    // Tracks the player's current score
    public int playerScore = 0;
    
    // Reference to the UI Text element that displays the score
    public Text scoreText;
    
    // Reference to the Game Over screen UI panel/game object
    public GameObject gameOverScreen;
    
    // References to the score display Text elements
    public Text currentScoreText;
    public Text highScoreText;
    
    // Reference to the BackgroundMusic component
    private BackgroundMusic backgroundMusic;

    private GameOverSound gameOverSound;

    private ScoreSound scoreSound;
    
    // Flag to track if game is already over
    private bool isGameOver = false;

    // ContextMenu attribute allows calling this method from the Unity Editor's context menu
    [ContextMenu("Increase Score")]
    // Adds points to the player's score and updates the UI
    // scoreToAdd: Number of points to add to the current score
    public void addScore(int scoreToAdd)
    {
        if (isGameOver) return;
        // Increase the player's score
        playerScore += scoreToAdd;
        // Update the score display
        scoreText.text = playerScore.ToString();
        scoreSound.PlayMusic();
    }

    private void Start()
    {
        // Find and store reference to BackgroundMusic
        backgroundMusic = FindFirstObjectByType<BackgroundMusic>();
        if (backgroundMusic != null)
        {
            backgroundMusic.PlayMusic();
        }
        else
        {
            Debug.LogWarning("BackgroundMusic not found in the scene!");
        }

        // Find GameOverSound in the scene
        gameOverSound = FindFirstObjectByType<GameOverSound>();
        if (gameOverSound == null)
        {
            Debug.LogWarning("GameOverSound not found in the scene!");
        }

        scoreSound = FindFirstObjectByType<ScoreSound>();
        if (scoreSound == null)
        {
            Debug.LogWarning("ScoreSound not found in the scene!");
        }

    }

    // Restarts the current scene, effectively resetting the game
    public void restartGame()
    {        
        // Reset the game over flag after stopping sounds
        isGameOver = false;
        
        // Add a small delay to ensure all audio stops before scene reload
        StartCoroutine(RestartWithDelay());
    }
    
    private IEnumerator RestartWithDelay()
    {
        // Ensure any playing sounds are stopped
        if (gameOverSound != null)
        {
            gameOverSound.StopMusic();
        }
        
        // Wait for end of frame to ensure all audio stops
        yield return new WaitForEndOfFrame();
        
        // Now reload the scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    // Handles the game over state
    public void gameOver()
    {
        // Prevent multiple game over triggers
        if (isGameOver) return;
        
        isGameOver = true;
        
        // Notify the game over sound system that a new game over state has begun
        if (gameOverSound != null)
        {
            gameOverSound.PlayMusic();
        }
        
        // Save high score if current score is higher
        int currentHighScore = PlayerPrefs.GetInt("highScore", 0);

        if (playerScore > currentHighScore)
        {
            currentHighScore = playerScore;
            PlayerPrefs.SetInt("highScore", currentHighScore);
            PlayerPrefs.Save(); // Ensure the data is saved immediately
        }
        
        // Update the score displays
        if (currentScoreText != null)
        {
            currentScoreText.text = "Score: " + playerScore.ToString();
        }
        
        if (highScoreText != null)
        {
            highScoreText.text = "Best: " + currentHighScore.ToString();
        }
        
        // Show the game over screen by activating its GameObject
        gameOverScreen.SetActive(true);
        
        // Stop the background music after a small delay to avoid cutting off the game over sound
        if (backgroundMusic != null)
        {
            backgroundMusic.StopMusic();
        }
        else
        {
            Debug.LogWarning("BackgroundMusic reference is null in gameOver()");
        }
    }
}
