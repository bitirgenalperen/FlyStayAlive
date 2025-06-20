using UnityEngine;
using UnityEngine.SceneManagement;

// Manages the playback of game over sound in the game.
// Implements the Singleton pattern to ensure only one instance exists throughout the game.
public class GameOverSound : MonoBehaviour
{
    
    // Singleton instance of the GameOverSound class
    public static GameOverSound instance;
    
    // Reference to the AudioSource component that will play the music
    private AudioSource audioSource;
    private bool hasPlayed = false;
    
    // Called when the script instance is being loaded.
    // Sets up the singleton instance and initializes the AudioSource
    private void Awake()
    {
        // Ensure only one instance of GameOverSound exists
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            audioSource = GetComponent<AudioSource>();
            
            // Make sure the audio doesn't loop
            if (audioSource != null)
            {
                audioSource.loop = false;
            }
            
            // Reset when a new scene is loaded
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    // Called when a new scene is loaded.
    // Resets the hasPlayed flag to ensure the sound can be played again in the new scene
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // Reset the hasPlayed flag when a new scene is loaded
        hasPlayed = false;
    }
    
    // Call this to start playing the music
    public void PlayMusic()
    {
        if (audioSource != null && !audioSource.isPlaying && !hasPlayed)
        {
            audioSource.Play();
            hasPlayed = true;
        }
    }
    
    // Call this to stop the music
    public void StopMusic()
    {
        if (audioSource != null && audioSource.isPlaying)
        {
            audioSource.Stop();
        }
    }
    
    // Called when the script instance is being destroyed.
    // Cleans up the event subscription
    private void OnDestroy()
    {
        if (instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }
}
